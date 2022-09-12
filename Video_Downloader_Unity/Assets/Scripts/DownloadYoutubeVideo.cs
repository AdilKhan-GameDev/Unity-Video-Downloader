using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DownloadYoutubeVideo : MonoBehaviour, IProgress<double>
{
    string destinationPath;
    //Environment.SpecialFolder destination;
    private float progress;
    private int progressint;
    public Image downloadProgress;
    public TMP_InputField urlfield;
    public GameObject downloadbar;
    public Text downloadstatus;
    private void Start()
    {
        destinationPath = Path.Combine(Application.persistentDataPath, "Download");
        //destination = Environment.SpecialFolder.CommonVideos;
        downloadbar.SetActive(false);
        if (downloadProgress.sprite == null)
        {
            var texture = Texture2D.whiteTexture;
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100);
            downloadProgress.sprite = sprite;
        }
    }

    public async void Download()
    {
        downloadbar.SetActive(true);
        YoutubePlayer.GetYoutubePlayerInstance().SetUrl(urlfield.text.ToString());
        Debug.Log("Got the URL");
        Debug.Log("Downloading, please wait..." + progress.ToString());
            
        var videoDownloadTask = YoutubePlayer.GetYoutubePlayerInstance().DownloadVideoAsync(destinationPath, null, this);
        Debug.Log("Done" + progress.ToString());

        var filePath = await videoDownloadTask;
            
            
        Debug.Log($"Video saved to {Path.GetFullPath(filePath)}");
    }

    public void Report(double value)
    {
        progress = (float) value;
    }

    private void Update()
    {
        progressint = (int)(progress * 100);
        if(downloadbar.activeSelf)
        {
            downloadstatus.text = "Downloading..." + progressint + "%";
            downloadProgress.fillAmount = progress;
        }
        if (progressint == 100)
            downloadstatus.text = "Done!!! Saved on " + destinationPath.ToString();
    }
}
