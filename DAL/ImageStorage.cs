using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using System.IO;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using System.Drawing;

namespace ImageSharingWithCloudServices.DAL
{
    public class ImageStorage
    {
        public const bool USE_BLOG_STORAGE = true;
        public const string ACCOUNT = "imagesharingmou";
        public const string CONTAINER = "images";

        public static void SaveFile(HttpServerUtilityBase server,
                                    HttpPostedFileBase imageFile,
                                    int imageId)
        {
            if (USE_BLOG_STORAGE)
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(
                            ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
                //CloudStorageAccount account =
                //       CloudStorageAccount.Parse(
                //           CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                CloudBlockBlob blob = container.GetBlockBlobReference(FilePath(server, imageId));
                blob.UploadFromStream(imageFile.InputStream);
            }
            else
            { 
                string imgFileName = FilePath(server, imageId);
                imageFile.SaveAs(imgFileName);
            }
        }
        public static string FilePath(HttpServerUtilityBase server, int imageId)
        {
            if (USE_BLOG_STORAGE)
            {
                string imgBlogName = imageId + ".jpg";
                return imgBlogName;
            }
            else
            {
                string imgFileName = server.MapPath("~/Content/Images/img-" + imageId + ".jpg");
                return imgFileName;
            }
        }


        public static string ImageURI(UrlHelper urlHelper, int imageId)
        {
            if (USE_BLOG_STORAGE)
            {
                return "https://" + ACCOUNT + ".blob.core.windows.net/" + CONTAINER + "/" + imageId + ".jpg";
            }
            else
            {
                return urlHelper.Content("~/Content/Images/img-" + imageId + ".jpg");
            }            
        }

        public static Boolean Validate(int id)
        {
            if (USE_BLOG_STORAGE)
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(
                            ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
                //CloudStorageAccount account =
                //       CloudStorageAccount.Parse(
                //           CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(CONTAINER);
                CloudBlockBlob blob = container.GetBlockBlobReference(FilePath(null, id));
                //blob.UploadFromStream(imageFile.InputStream);

                MemoryStream imageStream = new MemoryStream();
                blob.DownloadToStream(imageStream);

                Image image = Image.FromStream(imageStream);

                if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //else
            //{
            //    //string imgFileName = FilePath(server, imageId);
            //    //imageFile.SaveAs(imgFileName);
            //}
        }
    }
}