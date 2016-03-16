namespace Edgarcrawler
{
    partial class EdgarCrawl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.directoryButton = new System.Windows.Forms.Button();
            this.scanInfoPanel = new System.Windows.Forms.Panel();
            this.directoryPanel = new System.Windows.Forms.Panel();
            this.directoryLabel = new System.Windows.Forms.Label();
            this.estimatesLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.scanButton = new System.Windows.Forms.Button();
            this.scanInfoPanel.SuspendLayout();
            this.directoryPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(39, 32);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(206, 23);
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 13);
            this.label1.TabIndex = 1;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.directoryButton.Location = new System.Drawing.Point(142, 32);
            this.directoryButton.Name = "button1";
            this.directoryButton.Size = new System.Drawing.Size(103, 23);
            this.directoryButton.TabIndex = 2;
            this.directoryButton.Text = "scan";
            this.directoryButton.UseVisualStyleBackColor = true;
            this.directoryButton.Visible = false;
            this.directoryButton.Click += new System.EventHandler(this.startScan);
            // 
            // panel1
            // 
            this.scanInfoPanel.Controls.Add(this.estimatesLabel);
            this.scanInfoPanel.Controls.Add(this.label1);
            this.scanInfoPanel.Location = new System.Drawing.Point(12, 61);
            this.scanInfoPanel.Name = "panel1";
            this.scanInfoPanel.Size = new System.Drawing.Size(260, 49);
            this.scanInfoPanel.TabIndex = 3;
            // 
            // panel2
            // 
            this.directoryPanel.Controls.Add(this.directoryLabel);
            this.directoryPanel.Location = new System.Drawing.Point(14, 61);
            this.directoryPanel.Name = "panel2";
            this.directoryPanel.Size = new System.Drawing.Size(262, 39);
            this.directoryPanel.TabIndex = 3;
            this.directoryPanel.Visible = false;
            // 
            // label4
            // 
            this.directoryLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryLabel.Location = new System.Drawing.Point(0, 0);
            this.directoryLabel.Name = "label4";
            this.directoryLabel.Size = new System.Drawing.Size(262, 39);
            this.directoryLabel.TabIndex = 0;
            this.directoryLabel.Text = "No directory chosen";
            this.directoryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.estimatesLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.estimatesLabel.Location = new System.Drawing.Point(0, 15);
            this.estimatesLabel.Name = "label2";
            this.estimatesLabel.Size = new System.Drawing.Size(260, 34);
            this.estimatesLabel.TabIndex = 2;
            this.estimatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.estimatesLabel.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(39, 58);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(203, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(39, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Connecting to www.sec.gov...";
            // 
            // button2
            // 
            this.scanButton.Location = new System.Drawing.Point(39, 32);
            this.scanButton.Name = "button2";
            this.scanButton.Size = new System.Drawing.Size(103, 23);
            this.scanButton.TabIndex = 6;
            this.scanButton.Text = "Choose Directory";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Visible = false;
            this.scanButton.Click += new System.EventHandler(this.chooseDirectory);
            // 
            // EdgarCrawl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 133);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.directoryPanel);
            this.Controls.Add(this.scanInfoPanel);
            this.Controls.Add(this.directoryButton);
            this.Controls.Add(this.scanButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EdgarCrawl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edgar Crawler";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.threadCloser);
            this.Shown += new System.EventHandler(this.GUIload);
            this.scanInfoPanel.ResumeLayout(false);
            this.directoryPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button directoryButton;
        private System.Windows.Forms.Panel scanInfoPanel;
        private System.Windows.Forms.Label estimatesLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.Panel directoryPanel;
        private System.Windows.Forms.Label directoryLabel;
    }
}

