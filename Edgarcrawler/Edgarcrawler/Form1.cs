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
        bool isScanning = false;
        string path = "#@!";
        bool mainThreadAlive = true;


        public EdgarCrawl()
        {
            InitializeComponent();
        }

        private async void GUIload(Object sender, EventArgs e)
        {
            Task t = Task.Run(() => FetchCIK());
            activeTasks.Add(t);
            await t;
            activeTasks.Remove(t);
            label3.Visible = false;
            progressBar1.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
        }

        private void FetchCIK()
        {
            string HTML = webGet("https://www.sec.gov/edgar/NYU/cik.coleft.c");
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
                string fileName = companyName.Replace(":", string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty).Replace("*", string.Empty).Replace("?", string.Empty).Replace("\"", string.Empty).Replace("|", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty);
                WebClient mywebClient = new WebClient();
                byte[] pdfdata = mywebClient.DownloadData(urls[0]);
                File.WriteAllBytes(path + fileName.Substring(0, Math.Min(259, fileName.Length)) + ".pdf", pdfdata);
                pdfDocuments += 1;
                totalBytes += pdfdata.Length;
            }
            
            else {
                int i = 0;
                foreach (string url in urls)
                {
                    string fileName = companyName.Replace(":", string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty).Replace("*", string.Empty).Replace("?", string.Empty).Replace("\"", string.Empty).Replace("|", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty);
                    WebClient mywebClient = new WebClient();
                    byte[] pdfdata = mywebClient.DownloadData(url);
                    File.WriteAllBytes(path + fileName.Substring(0, Math.Min(259, fileName.Length)) + "(" + i.ToString() + ").pdf", pdfdata);
                    i++;
                    pdfDocuments += 1;
                    totalBytes += pdfdata.Length;
                }
            }
        }

        private List<string> extractPdfURL(List<string> urls)
        {
            List<string> PDFurls = new List<string>();
            foreach (string url in urls)
            {
                string HTML = webGet(url);
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
            string HTML = webGet(url);
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

        private string webGet(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 60;
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string html = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return html;
            }
            catch (System.Net.WebException)
            {
                if (MessageBox.Show("Unable to connect to \"http://www.sec.gov\". Retry connection?", "Network error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    return webGet(url);
                }
                else
                {
                    Application.Exit();
                    return null;
                }
            }
            
        }

        private void scan(object sender, EventArgs e)
        {
            if (path.Equals("#@!"))
            {
                MessageBox.Show("Please select a directory for the PDF files.");
            }
            else if (isScanning)
            {
                Task t = Task.Run(() => MessageBox.Show("Scan is already in progress."));
            }
            else {
                isScanning = true;
                int countTotal = CIKtable.Count;
                progressBar.Visible = true;
                progressBar.Maximum = countTotal;
                progressBar.Value = 0;
                progressBar.Step = 1;
                float avgTime = 450;
                float completed = 0;
                float remainingTime = (countTotal - completed) * avgTime; //est. time to completion in miliseconds

                foreach (List<string> page in CIKtable)
                {
                    if (!mainThreadAlive)
                    {
                        break;
                    }
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
                    if (remainingTime > 86400000)
                    {
                        label2.Text = Math.Round((remainingTime / 86400000), 2, MidpointRounding.AwayFromZero).ToString() + " days remaining.";
                    }
                    else if (remainingTime > 3600000)
                    {
                        label2.Text = Math.Round((remainingTime / 3600000), 2, MidpointRounding.AwayFromZero).ToString() + " hours remaining.";
                    }
                    else if (remainingTime > 60000)
                    {
                        label2.Text = Math.Round((remainingTime / 60000), 2, MidpointRounding.AwayFromZero).ToString() + " minutes remaining.";
                    }
                    else
                    {
                        label2.Text = "< 1 minute remaining";
                    }

                    if (completed <= 300)
                    {
                        label2.Text = label2.Text + "\nEst. size on disk: calculating...";
                    }
                    else
                    {
                        label2.Text = label2.Text + "\nEst. size on disk: " + Math.Round(((totalBytes / completed) * countTotal) / 1073741824, 2).ToString() + "GB";
                    }

                }
            }
        }

        private void chooseDirectory(object sender, EventArgs e)
        {
            if (!isScanning)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();
                path = fbd.SelectedPath+"\\";
            }
            else
            {
                MessageBox.Show("Cannot change directory while scan is in progress.");
            }
        }

        private void threadCloser(object sender, FormClosedEventArgs e)
        {
            mainThreadAlive = false;
        }
    }
}
