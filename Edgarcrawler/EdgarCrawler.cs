using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edgarcrawler
{
    public partial class EdgarCrawl : Form
    {
        List<List<string>> CIKtable = new List<List<string>>();
        long totalBytes = 0;
        string path = "empty";
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\NathanPettorini\\Edgarcrawler\\";
        bool mainThreadAlive = true;
        float completed = 0;
        Queue<List<string>> pages = new Queue<List<string>>();


        public EdgarCrawl()
        {
            InitializeComponent();
            Directory.CreateDirectory(appdata);
        }

        private void partialScanCheck()
        {
            if (File.Exists(appdata + "path"))
            {
                if (MessageBox.Show("Unfinished crawl detected. Recover and resume previous crawl? Selecting \"no\" will remove the unfinished scan.", "Resume crawl", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    path = File.ReadAllText(appdata + "path");
                    int count = int.Parse(File.ReadAllText(appdata + "bookmark"));
                    totalBytes = long.Parse(File.ReadAllText(appdata + "size"));
                    CIKtable = CIKtable.Skip(count).ToList();
                    completed = count;
                    Task queueFillTask = Task.Run(() => populatePages());
                    scan();
                }
                else
                {
                    File.Delete(appdata + "bookmark");
                    File.Delete(appdata + "path");
                    File.Delete(appdata + "size");
                    directoryLabel.Visible = true;
                    directoryPanel.Visible = true;

                }
            }
            else {
                directoryLabel.Visible = true;
                directoryPanel.Visible = true;
            }
        }


        private void GUIload(Object sender, EventArgs e)
        {
            Task<List<List<string>>> CIKtask = Task<List<List<string>>>.Run(() => FetchCIK());
            while (!CIKtask.Wait(60))
            {
                Application.DoEvents();
            }
            CIKtable = CIKtask.Result;
            label3.Visible = false;
            CIKupdateProgressBar.Visible = false;
            directoryButton.Visible = true;
            scanButton.Visible = true;
            partialScanCheck();
        }

        private async Task<List<List<string>>> FetchCIK()
        {
            string HTML = webGet("https://www.sec.gov/edgar/NYU/cik.coleft.c",0);
            List<string> datasplit = HTML.Split('\n').ToList();
            List<List<string>> table = new List<List<string>>();

            this.Invoke((MethodInvoker)delegate
            {
                label3.Text = "Updating CIK data from www.sec.gov...";
                CIKupdateProgressBar.Maximum = datasplit.Count;
                CIKupdateProgressBar.Step = 1;
            });
            int stepSize = datasplit.Count/4;
            Task<List<List<string>>> thread1 = Task<List<List<string>>>.Factory.StartNew(() => CIKparse(datasplit.GetRange(0, stepSize)));
            Task<List<List<string>>> thread2 = Task<List<List<string>>>.Factory.StartNew(() => CIKparse(datasplit.GetRange(stepSize, stepSize)));
            Task<List<List<string>>> thread3 = Task<List<List<string>>>.Factory.StartNew(() => CIKparse(datasplit.GetRange(stepSize*2, stepSize)));
            Task<List<List<string>>> thread4 = Task<List<List<string>>>.Factory.StartNew(() => CIKparse(datasplit.Skip(stepSize*3).ToList()));
            Task<List<List<string>>>[] threads = new Task<List<List<string>>>[] { thread1, thread2, thread3, thread4 };

            foreach (Task<List<List<string>>> thread in threads)
            {
                table = table.Concat(await thread).ToList();
            }


            return table;
        }

        private List<List<string>> CIKparse(List<string> datalist)
        {
            List<List<string>> subtable = new List<List<string>>();
            foreach (string row in datalist)
            {
                string[] rowsplit = row.Split(':');
                if (rowsplit.Count() > 3)
                {
                    string companyName = "";
                    for (int j = 0; j < rowsplit.Count() - 2; j++)
                    {
                        companyName += rowsplit[j] + ":";
                    }
                    companyName = companyName.Substring(0, companyName.Length - 1);
                    string CIK = rowsplit[rowsplit.Count() - 2];
                    subtable.Add(new List<string> { companyName, CIK });
                }
                else if (rowsplit.Count().Equals(3))
                {
                    subtable.Add(new List<string> { rowsplit[0], rowsplit[1] });
                }

                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        CIKupdateProgressBar.Increment(1);
                    });
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
            return subtable;
        }






        private void writePdfs(List<string> urls, string companyName)
        {
            if (urls.Count.Equals(1))
            {
                writeFile(urls[0], companyName);
            }         
            else if(urls.Count > 1){
                writeFile(urls[0], companyName);
                int i = 1;
                foreach (string url in urls.Skip(1))
                {
                    writeFile(url, companyName + "(" + i.ToString() + ")");
                    i++;
                }
            }
        }

        private void writeFile(string url, string fileName)
        {
            fileName = fileName.Substring(0, Math.Min(259, fileName.Length));
            foreach (string item in new string[] {":","/","\\","*","?","\"","|","<",">" })
            {
                fileName = fileName.Replace(item, String.Empty);
            }
            WebClient pdfWebClient = new WebClient();
            byte[] pdfdata = pdfWebClient.DownloadData(url);
            File.WriteAllBytes(path + fileName + ".pdf", pdfdata);
            totalBytes += pdfdata.Length;
        }

        private List<string> extractPdfURL(List<string> urls)
        {
            List<string> PDFurls = new List<string>();
            List<string> htmls = webGet(urls).Result;
            foreach (string HTML in htmls)
            {
                string[] data = HTML.Split('\n');
                foreach (string line in data)
                {
                    if (line.Contains("scanned.pdf</a></td>"))
                    {
                        PDFurls.Add("http://www.sec.gov" + line.Substring(37, (line.Length - 59)));
                    }
                }
            }
            return PDFurls;
        }

        private List<string> extractDocuments(string HTML)
        {
            string[] datasplit = HTML.Split('\n');
            List<string> documents = new List<string>();
            foreach (string line in datasplit)
            {
                if (line.Contains("id=\"documentsbutton\">&nbsp;Documents</a></td>"))
                {
                    documents.Add("http://www.sec.gov"+line.Substring(29, line.Length - 76));
                }
            }
            return documents;
        }

        private async Task<List<string>> webGet(List<string> urls)
        {
            List<string> htmls = new List<string>();
            List<Task<string>> tasks = new List<Task<string>>();
            foreach (string url in urls)
            {
                tasks.Add(webGetAsync(url));
            }
            foreach (Task<string> task in tasks)
            {
                htmls.Add(await task);
            }
            return htmls;
        }

        private async Task<List<byte[]>> pfgGet(List<string> urls)
        {
            List<byte[]> bytes = new List<byte[]>();
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();
            foreach (string url in urls)
            {
                tasks.Add(pdfGetAsync(url));
            }
            foreach (Task<byte[]> task in tasks)
            {
                bytes.Add(await task);
            }
            return bytes;
        }

        private async Task<string> webGetAsync(string url)
        {
            Task<string> getHtml = Task<string>.Factory.StartNew(() => webGet(url, 0));
            string html = await getHtml;
            return html;
        }

        private async Task<byte[]> pdfGetAsync(string url)
        {
            Task<byte[]> getBytes = Task.Factory.StartNew(() => {
                WebClient pdfWebClient = new WebClient();
                byte[] pdfdata = pdfWebClient.DownloadData(url);
                return pdfdata;
            });
            byte[] data = await getBytes;
            return data;

        }

        private string webGet(string url, int trycount)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 60000;
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string html = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return html;
            }
            catch (WebException)
            {
                if (trycount > 60)
                {

                    if (MessageBox.Show("Unable to connect to \"http://www.sec.gov\". Retry connection?", "Network error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        return webGet(url, 0);
                    }
                    else
                    {
                        Application.Exit();
                        return null;
                    }

                }
                else
                {
                    System.Threading.Thread.Sleep(10000);
                    File.WriteAllText(appdata + "reconnects", (int.Parse(File.ReadAllText(appdata+"reconnects"))+trycount).ToString());
                    return webGet(url, trycount + 1);
                }
            }
            
        }



        private void startScan(object sender, EventArgs e)
        {
            Task queueFillTask = Task.Run(() => populatePages());
            scan();
        }

        private async void populatePages()
        {

            Task<List<string>> thread1 = Task<List<string>>.Factory.StartNew(() => webGetThreadWrapper(CIKtable[0][1], CIKtable[0][0]));
            Task<List<string>> thread2 = Task<List<string>>.Factory.StartNew(() => webGetThreadWrapper(CIKtable[1][1], CIKtable[1][0]));
            Task<List<string>> thread3 = Task<List<string>>.Factory.StartNew(() => webGetThreadWrapper(CIKtable[2][1], CIKtable[2][0]));
            Task<List<string>> thread4 = Task<List<string>>.Factory.StartNew(() => webGetThreadWrapper(CIKtable[3][1], CIKtable[3][0]));
            Task<List<string>>[] threads = new Task<List<string>>[] { thread1, thread2, thread3, thread4 };

            int i = 4;

            while (i < CIKtable.Count)
            {
                if (!mainThreadAlive)
                {
                    break;
                }
                if (pages.Count < 20)
                {
                    int j = 0;
                    while (j < 4)
                    {
                        if (threads[j].IsCompleted)
                        {
                            List<string> result = await threads[j];
                            pages.Enqueue(result);
                            threads[j] = Task<List<string>>.Factory.StartNew(() => webGetThreadWrapper(CIKtable[i][1], CIKtable[i][0]));
                            i++;
                        }
                        j++;
                    }
                }
                Application.DoEvents();

            }
        }

        private List<string> webGetThreadWrapper(string CIK, string companyName)
        {
            return new List<string> { webGet("http://www.sec.gov/cgi-bin/browse-edgar?action=getcompany&CIK=" + CIK + "&type=regdex&dateb=&owner=exclude&count=100", 0), companyName};
        }

        private void scan()
        {
            if (path.Equals("empty"))
            {
                MessageBox.Show("Please select a directory for the PDF files.");
            }
            else {
                scanInfoPanel.Visible = true;
                directoryButton.Visible = false;
                scanButton.Visible = false;
                directoryLabel.Visible = false;
                estimatesLabel.Visible = true;
                directoryPanel.Visible = false;
                int countTotal = CIKtable.Count;
                int grandTotal = countTotal + Convert.ToInt32(completed);
                progressBar.Visible = true;
                progressBar.Maximum = countTotal + Convert.ToInt32(completed);
                progressBar.Value = Convert.ToInt32(completed);
                progressBar.Step = 1;
                float processed = 0;
                float sumTime = 450;
                float remainingTime = countTotal * sumTime; //est. time to completion in miliseconds

                File.WriteAllText(appdata + "path", path);

                for (int i = 0; i<CIKtable.Count; i++)
                {
                    long ts = DateTime.Now.Ticks;
                    while (pages.Count.Equals(0))   //wait for the queue to fill back up
                    {
                        if (!mainThreadAlive)
                        {
                            break;
                        }
                        Application.DoEvents();
                    }
                    if (!mainThreadAlive)
                    {
                        break;
                    }
                    List<string> page = pages.Dequeue();
                    Task t = Task.Run(() => writePdfs((extractPdfURL(extractDocuments(page[0]))), page[1]));
                    while (!t.Wait(60))
                    {
                        Application.DoEvents();
                    }
                    progressBar.Increment(1);
                    label1.Text = "now processing: " + page[1].Substring(0, Math.Min(15, page[1].Length));
                    progressBar.Refresh();
                    label1.Refresh();
                    completed += 1;
                    processed += 1;
                    long timeTaken = (DateTime.Now.Ticks - ts) / 10000;
                    sumTime = (sumTime + timeTaken);
                    remainingTime = (countTotal - processed) * sumTime / processed;
                    if (processed <= 300)
                    {
                        estimatesLabel.Text = "Calculating est. time to completion...";
                        estimatesLabel.Text = estimatesLabel.Text + "\nEst. size on disk: calculating...";
                    }
                    else if (remainingTime > 86400000)
                    {
                        estimatesLabel.Text = Math.Round((remainingTime / 86400000), 2, MidpointRounding.AwayFromZero).ToString() + " days remaining.";
                        estimatesLabel.Text = estimatesLabel.Text + "\nEst. size on disk: " + Math.Round(((totalBytes / completed) * grandTotal) / 1073741824, 2).ToString() + "GB";
                    }
                    else if (remainingTime > 3600000)
                    {
                        estimatesLabel.Text = Math.Round((remainingTime / 3600000), 2, MidpointRounding.AwayFromZero).ToString() + " hours remaining.";
                        estimatesLabel.Text = estimatesLabel.Text + "\nEst. size on disk: " + Math.Round(((totalBytes / completed) * grandTotal) / 1073741824, 2).ToString() + "GB";
                    }
                    else if (remainingTime > 60000)
                    {
                        estimatesLabel.Text = Math.Round((remainingTime / 60000), 2, MidpointRounding.AwayFromZero).ToString() + " minutes remaining.";
                        estimatesLabel.Text = estimatesLabel.Text + "\nEst. size on disk: " + Math.Round(((totalBytes / completed) * grandTotal) / 1073741824, 2).ToString() + "GB";
                    }
                    else
                    {
                        estimatesLabel.Text = "< 1 minute remaining";
                        estimatesLabel.Text = estimatesLabel.Text + "\nEst. size on disk: " + Math.Round(((totalBytes / completed) * grandTotal) / 1073741824, 2).ToString() + "GB";

                    }
                    File.WriteAllText(appdata + "bookmark", completed.ToString());
                    File.WriteAllText(appdata + "size", totalBytes.ToString());
                }
                if (mainThreadAlive)
                {
                    File.Delete(appdata + "bookmark");
                    File.Delete(appdata + "path");
                    File.Delete(appdata + "size");
                    label1.Text = "";
                    estimatesLabel.Text = "All done!";
                }
            }
        }

        private void chooseDirectory(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            path = fbd.SelectedPath + "\\";
            directoryLabel.Text = "Chosen directory: "+path;

        }

        private void threadCloser(object sender, FormClosedEventArgs e)
        {
            mainThreadAlive = false;
        }
    }
}
