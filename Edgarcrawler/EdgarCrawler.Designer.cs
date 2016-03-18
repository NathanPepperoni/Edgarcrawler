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
            this.estimatesLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.directoryPanel = new System.Windows.Forms.Panel();
            this.directoryLabel = new System.Windows.Forms.Label();
            this.CIKupdateProgressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.scanButton = new System.Windows.Forms.Button();
            this.FilterCheck = new System.Windows.Forms.CheckBox();
            this.afterDatePicker = new System.Windows.Forms.DateTimePicker();
            this.beforeDatePicker = new System.Windows.Forms.DateTimePicker();
            this.beforeCheck = new System.Windows.Forms.CheckBox();
            this.afterCheck = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.filterLabel = new System.Windows.Forms.Label();
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
            // directoryButton
            // 
            this.directoryButton.Location = new System.Drawing.Point(142, 32);
            this.directoryButton.Name = "directoryButton";
            this.directoryButton.Size = new System.Drawing.Size(103, 23);
            this.directoryButton.TabIndex = 2;
            this.directoryButton.Text = "scan";
            this.directoryButton.UseVisualStyleBackColor = true;
            this.directoryButton.Visible = false;
            this.directoryButton.Click += new System.EventHandler(this.startScan);
            // 
            // scanInfoPanel
            // 
            this.scanInfoPanel.Controls.Add(this.estimatesLabel);
            this.scanInfoPanel.Controls.Add(this.panel1);
            this.scanInfoPanel.Controls.Add(this.label1);
            this.scanInfoPanel.Location = new System.Drawing.Point(12, 61);
            this.scanInfoPanel.Name = "scanInfoPanel";
            this.scanInfoPanel.Size = new System.Drawing.Size(260, 49);
            this.scanInfoPanel.TabIndex = 3;
            // 
            // estimatesLabel
            // 
            this.estimatesLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.estimatesLabel.Location = new System.Drawing.Point(0, 15);
            this.estimatesLabel.Name = "estimatesLabel";
            this.estimatesLabel.Size = new System.Drawing.Size(260, 34);
            this.estimatesLabel.TabIndex = 2;
            this.estimatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.estimatesLabel.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(53, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(162, 26);
            this.panel1.TabIndex = 15;
            // 
            // directoryPanel
            // 
            this.directoryPanel.Controls.Add(this.directoryLabel);
            this.directoryPanel.Location = new System.Drawing.Point(14, 61);
            this.directoryPanel.Name = "directoryPanel";
            this.directoryPanel.Size = new System.Drawing.Size(262, 39);
            this.directoryPanel.TabIndex = 3;
            this.directoryPanel.Visible = false;
            // 
            // directoryLabel
            // 
            this.directoryLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryLabel.Location = new System.Drawing.Point(0, 0);
            this.directoryLabel.Name = "directoryLabel";
            this.directoryLabel.Size = new System.Drawing.Size(262, 39);
            this.directoryLabel.TabIndex = 0;
            this.directoryLabel.Text = "No directory chosen";
            this.directoryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CIKupdateProgressBar
            // 
            this.CIKupdateProgressBar.Location = new System.Drawing.Point(39, 58);
            this.CIKupdateProgressBar.Name = "CIKupdateProgressBar";
            this.CIKupdateProgressBar.Size = new System.Drawing.Size(203, 23);
            this.CIKupdateProgressBar.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(39, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Connecting to www.sec.gov...";
            // 
            // scanButton
            // 
            this.scanButton.Location = new System.Drawing.Point(39, 32);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(103, 23);
            this.scanButton.TabIndex = 6;
            this.scanButton.Text = "Choose Directory";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Visible = false;
            this.scanButton.Click += new System.EventHandler(this.chooseDirectory);
            // 
            // FilterCheck
            // 
            this.FilterCheck.AutoSize = true;
            this.FilterCheck.Location = new System.Drawing.Point(79, 131);
            this.FilterCheck.Name = "FilterCheck";
            this.FilterCheck.Size = new System.Drawing.Size(128, 17);
            this.FilterCheck.TabIndex = 8;
            this.FilterCheck.Text = "Apply date range filter";
            this.FilterCheck.UseVisualStyleBackColor = true;
            this.FilterCheck.Visible = false;
            this.FilterCheck.CheckedChanged += new System.EventHandler(this.dateFilter);
            // 
            // afterDatePicker
            // 
            this.afterDatePicker.Location = new System.Drawing.Point(39, 194);
            this.afterDatePicker.Name = "afterDatePicker";
            this.afterDatePicker.Size = new System.Drawing.Size(200, 20);
            this.afterDatePicker.TabIndex = 9;
            // 
            // beforeDatePicker
            // 
            this.beforeDatePicker.Location = new System.Drawing.Point(39, 254);
            this.beforeDatePicker.Name = "beforeDatePicker";
            this.beforeDatePicker.Size = new System.Drawing.Size(200, 20);
            this.beforeDatePicker.TabIndex = 10;
            // 
            // beforeCheck
            // 
            this.beforeCheck.AutoSize = true;
            this.beforeCheck.Checked = true;
            this.beforeCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.beforeCheck.Location = new System.Drawing.Point(39, 231);
            this.beforeCheck.Name = "beforeCheck";
            this.beforeCheck.Size = new System.Drawing.Size(147, 17);
            this.beforeCheck.TabIndex = 11;
            this.beforeCheck.Text = "Only get filings older than:";
            this.beforeCheck.UseVisualStyleBackColor = true;
            // 
            // afterCheck
            // 
            this.afterCheck.AutoSize = true;
            this.afterCheck.Checked = true;
            this.afterCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.afterCheck.Location = new System.Drawing.Point(39, 171);
            this.afterCheck.Name = "afterCheck";
            this.afterCheck.Size = new System.Drawing.Size(148, 17);
            this.afterCheck.TabIndex = 12;
            this.afterCheck.Text = "Only get filing newer than:";
            this.afterCheck.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(99, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Set";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.setFilter);
            // 
            // filterLabel
            // 
            this.filterLabel.Location = new System.Drawing.Point(90, 110);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(101, 13);
            this.filterLabel.TabIndex = 14;
            this.filterLabel.Text = "No filter set";
            this.filterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.filterLabel.Visible = false;
            // 
            // EdgarCrawl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 161);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.afterCheck);
            this.Controls.Add(this.beforeCheck);
            this.Controls.Add(this.beforeDatePicker);
            this.Controls.Add(this.afterDatePicker);
            this.Controls.Add(this.FilterCheck);
            this.Controls.Add(this.CIKupdateProgressBar);
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button directoryButton;
        private System.Windows.Forms.Panel scanInfoPanel;
        private System.Windows.Forms.Label estimatesLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar CIKupdateProgressBar;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.Panel directoryPanel;
        private System.Windows.Forms.Label directoryLabel;
        private System.Windows.Forms.CheckBox FilterCheck;
        private System.Windows.Forms.DateTimePicker afterDatePicker;
        private System.Windows.Forms.DateTimePicker beforeDatePicker;
        private System.Windows.Forms.CheckBox beforeCheck;
        private System.Windows.Forms.CheckBox afterCheck;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label filterLabel;
    }
}

