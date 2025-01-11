namespace YoutubeHomemadeDownloader
{
    public partial class YouTubeDownloader : Form
    {
        public YouTubeDownloader()
        {
            InitializeComponent();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            string downBtnPrevTxt = btnDownload.Text;
            btnDownload.Text = "Please wait...";
            var urls = txtUrls.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var outputPath = txtDownloadPath.Text;

            var downloader = new YDowner.YoutubeDownloader();

            try
            {
                await downloader.DownloadAndConvertAsync(
                    urls,
                    outputPath,
                    progress => lblStatus.Text = progress // Update the status label
                );

                MessageBox.Show("Download completed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                btnDownload.Enabled = true;
                btnDownload.Text = downBtnPrevTxt;
            }
        }
        private void txtDownloadPath_Click(object sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select a folder to save downloaded files";
            folderDialog.ShowNewFolderButton = true;

            // Show the dialog and get the result
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtDownloadPath.Text = folderDialog.SelectedPath; // Set the selected folder to the textbox
            }
        }
    }
}
