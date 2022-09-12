using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using YoutubeExplode;
using YoutubeExplode.Videos.ClosedCaptions;
using YoutubeExplode.Videos.Streams;

public class YoutubePlayer : MonoBehaviour
{
    private string youtubeUrl;

    private YoutubeClient youtubeClient;
    private static YoutubePlayer instance;
    private void Start()
    {
        instance = this;
    }
    public void SetUrl(string url)
    {
        youtubeUrl = url;
    }
    public static YoutubePlayer GetYoutubePlayerInstance()
    {
        return instance;
    }
    private void Awake()
    {
        youtubeClient = new YoutubeClient();
    }
    public async Task<string> DownloadVideoAsync(string destinationFolder = null, string videoUrl = null,
        IProgress<double> progress = null, CancellationToken cancellationToken = default)
    {
        try
        {
            videoUrl = youtubeUrl;
            var video = await youtubeClient.Videos.GetAsync(videoUrl);
            var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(videoUrl);
                
            cancellationToken.ThrowIfCancellationRequested();
            var streamInfo = streamManifest.GetMuxed().WithHighestVideoQuality();
            if (streamInfo == null)
                throw new NotSupportedException($"No supported streams in youtube video '{videoUrl}'");

            var fileExtension = streamInfo.Container;
            var fileName = $"{video.Title}.{fileExtension}";

            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar.ToString(), "_");
            }

            var filePath = fileName;
            if (!string.IsNullOrEmpty(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
                filePath = Path.Combine(destinationFolder, fileName);
            }

            await youtubeClient.Videos.Streams.DownloadAsync(streamInfo, filePath, progress, cancellationToken);
            return filePath;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
        
}