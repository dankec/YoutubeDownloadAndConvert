using System.Threading.Tasks;
using YoutubeExplode;

namespace YDownerLibrary
{
    public class YoutubeNameRetrieve
    {
        public async Task<string?> RetrieveYoutubeNameAsync(string videoUrl)
        {
            try
            {
                var youtube = new YoutubeClient();
                var video = await youtube.Videos.GetAsync(videoUrl);

                return video.Title;
            }
            catch {
                return null;
            }
        }
    }
}
