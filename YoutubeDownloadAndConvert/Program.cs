using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using NAudio.Wave;
using NAudio.Lame;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter the YouTube video URL:");
        string videoUrl = Console.ReadLine();

        try
        {
            var youtube = new YoutubeClient();

            // Retrieve video information
            Console.WriteLine("Retrieving video info...");
            var video = await youtube.Videos.GetAsync(videoUrl);

            string videoTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars())); // Sanitize filename
            string tempFile = Path.Combine(Path.GetTempPath(), $"{videoTitle}.m4a");
            string outputFile = $"{videoTitle}.mp3";

            // Download audio stream
            Console.WriteLine("Downloading audio...");
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
            var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            if (audioStreamInfo != null)
            {
                await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, tempFile);

                // Convert to MP3
                Console.WriteLine("Converting to MP3...");
                ConvertToMp3(tempFile, outputFile);

                Console.WriteLine($"Done! MP3 file saved as: {outputFile}");
            }
            else
            {
                Console.WriteLine("No audio stream available for this video.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ConvertToMp3(string inputPath, string outputPath)
    {
        try
        {
            using (var reader = new AudioFileReader(inputPath)) // Supports .wav, .mp3, etc.
            using (var writer = new LameMP3FileWriter(outputPath, reader.WaveFormat, LAMEPreset.VBR_90))
            {
                reader.CopyTo(writer);
            }

            // Clean up temporary file
            File.Delete(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during MP3 conversion: {ex.Message}");
        }
    }
}
