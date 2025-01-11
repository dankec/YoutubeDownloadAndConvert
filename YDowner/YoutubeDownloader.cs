using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using NAudio.Wave;
using NAudio.Lame;

namespace YDownerLibrary
{
    public class YoutubeDownloader
    {
        /// <summary>
        /// Downloads and converts a list of YouTube video URLs to MP3 format.
        /// </summary>
        /// <param name="videoUrls">List of YouTube video URLs.</param>
        /// <param name="outputPath">Path to save the MP3 files.</param>
        /// <param name="progressCallback">Callback to report progress (optional).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DownloadAndConvertAsync(List<string> videoUrls, string outputPath, Action<string> progressCallback = null)
        {
            if (videoUrls == null || !videoUrls.Any())
                throw new ArgumentException("The video URLs list is empty.");

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Output path cannot be empty.");

            var youtube = new YoutubeClient();

            foreach (var videoUrl in videoUrls)
            {
                try
                {
                    progressCallback?.Invoke($"Processing: {videoUrl}");

                    var video = await youtube.Videos.GetAsync(videoUrl);
                    string videoTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));
                    string tempFile = Path.Combine(Path.GetTempPath(), $"{videoTitle}.m4a");
                    string outputFile = Path.Combine(outputPath, $"{videoTitle}.mp3");

                    // Download audio stream
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
                    var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                    if (audioStreamInfo != null)
                    {
                        await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, tempFile);

                        // Convert to MP3
                        ConvertToMp3(tempFile, outputFile);
                        progressCallback?.Invoke($"Completed: {videoUrl}");
                    }
                    else
                    {
                        progressCallback?.Invoke($"No audio stream found for: {videoUrl}");
                    }
                }
                catch (Exception ex)
                {
                    progressCallback?.Invoke($"Error processing {videoUrl}: {ex.Message}");
                }
            }

            progressCallback?.Invoke("All downloads completed.");
        }

        private void ConvertToMp3(string inputPath, string outputPath)
        {
            using (var reader = new AudioFileReader(inputPath))
            using (var writer = new LameMP3FileWriter(outputPath, reader.WaveFormat, LAMEPreset.VBR_90))
            {
                reader.CopyTo(writer);
            }

            // Clean up temporary file
            File.Delete(inputPath);
        }
    }
}
