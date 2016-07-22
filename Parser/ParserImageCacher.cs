namespace Parser
{
    using System;
    using System.IO;
    using System.Net;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.AspNet.SignalR.Client;

    public class ParserImageCacher
    {
        public ImageRepository ImageRepository = new ImageRepository();

        public void CacheImages(IHubProxy hubProxy, string hubUrl, string picturesPath, string absolutePicturesPath)
        {
            var images = ImageRepository.GetAllUncached();
            var index = 0;

            if (!Directory.Exists(absolutePicturesPath))
            {
                Directory.CreateDirectory(absolutePicturesPath);
            }

            using (var webClient = new WebClient())
            {
                foreach (var image in images)
                {
                    index++;
                    hubProxy.Invoke("PushStatus", $"Getting images... {index}/{images.Count}");
                    image.CachedUrl = CacheImage(image, webClient, hubUrl, picturesPath, absolutePicturesPath);
                }
            }

            ImageRepository.EditRange(images);
        }

        private string CacheImage(AdvertImage image, WebClient webClient, string hubUrl, string picturesPath, string absolutePicturesPath)
        {
            var pictureFileName = $"{image.AdvertId}-{Guid.NewGuid()}.jpg";
            webClient.DownloadFile(image.Url, $"{absolutePicturesPath}/{pictureFileName}");
            return $"{hubUrl}/{picturesPath}/{pictureFileName}";
        }
    }
}
