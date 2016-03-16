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
        List<Task> activeTasks = new List<Task>();
        int pdfDocuments = 0;
        long totalBytes = 0;
        long newBytes = 0;
        string path = "#@!";
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\NathanPettorini\\Edgarcrawler\\";
        bool mainThreadAlive = true;
        float completed = 0;


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

        private void test()
        {
            File.WriteAllText(appdata+"test.txt", "Tasd");
        }

        private async void GUIload(Object sender, EventArgs e)
        {
            Task t = Task.Run(() => FetchCIK());
            activeTasks.Add(t);
            await t;
            activeTasks.Remove(t);
            label3.Visible = false;
            progressBar1.Visible = false;
            directoryButton.Visible = true;
            scanButton.Visible = true;
            partialScanCheck();
        }

        private void FetchCIK()
        {
            string HTML = webGet("https://www.sec.gov/edgar/NYU/cik.coleft.c",0);
            string[] datasplit = HTML.Split('\n');
            List<List<string>> table = new List<List<string>>();

            this.Invoke((MethodInvoker)delegate
            {
                label3.Text = "Updating CIK data from www.sec.gov...";
                progressBar1.Maximum = datasplit.Length;
                progressBar1.Step = 1;
            });

            foreach (string row in datasplit)
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
                    table.Add(new List<string> { companyName, CIK });
                }
                else if (rowsplit.Count().Equals(3))
                {
                    table.Add(new List<string> { rowsplit[0], rowsplit[1] });
                }

                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        progressBar1.Increment(1);
                    });
                }
                catch (System.ObjectDisposedException)
                {
                    break;
                }
            }

            CIKtable = table;
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
            newBytes += pdfdata.Length;
            pdfDocuments += 1;
        }

        private List<string> extractPdfURL(List<string> urls)
        {
            List<string> PDFurls = new List<string>();
            foreach (string url in urls)
            {
                string HTML = webGet(url,0);
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

        private List<string> extractDocuments(string url)
        {
            string HTML = webGet(url,0);
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
            scan();
        }

        private void scan()
        {
            if (path.Equals("#@!"))
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
                progressBar.Maximum = countTotal+ Convert.ToInt32(completed);
                progressBar.Value = Convert.ToInt32(completed);
                progressBar.Step = 1;
                float processed = 0;
                float sumTime = 450;
                float remainingTime = countTotal*sumTime; //est. time to completion in miliseconds

                File.WriteAllText(appdata + "path", path);

                foreach (List<string> page in CIKtable)
                {
                    if (!mainThreadAlive)
                    {
                        break;
                    }
                    long ts = DateTime.Now.Ticks;
                    string url = "http://www.sec.gov/cgi-bin/browse-edgar?action=getcompany&CIK=" + page[1] + "&type=regdex&dateb=&owner=exclude&count=100";
                    Task t = Task.Run(() => writePdfs((extractPdfURL(extractDocuments(url))), page[0]));
                    activeTasks.Add(t);
                    while (!t.Wait(30))
                    {
                        Application.DoEvents();
                    }
                    activeTasks.Remove(t);
                    progressBar.Increment(1);
                    label1.Text = "now processing: " + page[0].Substring(0, Math.Min(15, page[0].Length));
                    progressBar.Refresh();
                    label1.Refresh();
                    completed += 1;
                    processed += 1;
                    long timeTaken = (DateTime.Now.Ticks - ts) / 10000;
                    sumTime = (sumTime + timeTaken);
                    remainingTime = (countTotal - processed) * sumTime/processed;
                    if (processed <= 200)
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
