namespace YoutubeHomemadeDownloader
{
    public partial class YouTubeDownloader : Form
    {
        List<string> listVideoNames = new List<string>();

        public YouTubeDownloader()
        {
            InitializeComponent();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            string downBtnPrevTxt = btnDownload.Text;
            btnDownload.Text = "Please wait...";
            var outputPath = txtDownloadPath.Text;

            var downloader = new YDownerLibrary.YoutubeDownloader();

            try
            {
                await downloader.DownloadAndConvertAsync(
                    listVideoNames,
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
        private async void txtUrls_TextChanged(object sender, EventArgs e)
        {
            var nameRetriever = new YDownerLibrary.YoutubeNameRetrieve();
            var urls = txtUrls.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            // Update list with urls
            listVideoNames = urls;

            txtUrls.Text = "";
            foreach (var videoUrl in urls)
            {
                try
                {
                    // Retrieve the video name asynchronously
                    var retrievedName = await nameRetriever.RetrieveYoutubeNameAsync(videoUrl);

                    // Add the video name or URL to the list
                    txtUrls.Text += retrievedName ?? videoUrl;
                }
                catch
                {
                    // Fallback to URL if an error occurs
                    txtUrls.Text += videoUrl;
                }
            }
        }

        private async void txtUrls_KeyDown(object sender, KeyEventArgs e)
        {
            // Detect Ctrl+V or Shift+Insert for paste action
            if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
            {
                await HandlePasteAsync();
            }
        }

        private async Task HandlePasteAsync()
        {
            var nameRetriever = new YDownerLibrary.YoutubeNameRetrieve();

            // Get the pasted text
            var clipboardText = Clipboard.GetText();
            var urls = listVideoNames = clipboardText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            listVideoNames = urls;

            // Update list with URLs
            var newNameList = new List<string>();
            foreach (var videoUrl in urls)
            {
                try
                {
                    // Retrieve video name asynchronously
                    var retrievedName = await nameRetriever.RetrieveYoutubeNameAsync(videoUrl);
                    newNameList.Add(retrievedName ?? videoUrl);
                }
                catch
                {
                    // Fallback to URL if an error occurs
                }
            }

            // Update TextBox with the retrieved names or URLs
            txtUrls.Text = string.Join(Environment.NewLine, newNameList);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_PASTE = 0x0302;

            // Detect paste action
            if (m.Msg == WM_PASTE)
            {
                _ = HandlePasteAsync();
                return;
            }

            base.WndProc(ref m);
        }
    }
}
