namespace YoutubeHomemadeDownloader
{
    partial class YouTubeDownloader
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtUrls;
        private System.Windows.Forms.TextBox txtDownloadPath;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtUrls = new TextBox();
            txtDownloadPath = new TextBox();
            btnDownload = new Button();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // txtUrls
            // 
            txtUrls.Location = new Point(12, 12);
            txtUrls.Multiline = true;
            txtUrls.Name = "txtUrls";
            txtUrls.PlaceholderText = "Enter YouTube video URLs, one per line";
            txtUrls.Size = new Size(460, 150);
            txtUrls.TabIndex = 0;
            // 
            // txtDownloadPath
            // 
            txtDownloadPath.Location = new Point(12, 170);
            txtDownloadPath.Name = "txtDownloadPath";
            txtDownloadPath.PlaceholderText = "Enter download path";
            txtDownloadPath.Size = new Size(460, 23);
            txtDownloadPath.TabIndex = 1;
            txtDownloadPath.Click += this.txtDownloadPath_Click;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(12, 200);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(460, 30);
            btnDownload.TabIndex = 2;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 240);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 3;
            // 
            // YouTubeDownloader
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 261);
            Controls.Add(lblStatus);
            Controls.Add(btnDownload);
            Controls.Add(txtDownloadPath);
            Controls.Add(txtUrls);
            Name = "YouTubeDownloader";
            Text = "YouTube Downloader";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
