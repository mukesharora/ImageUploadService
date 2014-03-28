using ImageUploadService.ImageServiceManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace ImageUploadService
{
    public class ImageProcessing : IImageProcessing
    {
        string imageUploadDirectory = ConfigurationManager.AppSettings.Get("ATSImageUploadDirectory");

        public ImageData DownloadImage(string imageName)
        {
            ImageData data = null;

            try
            {
                if (string.IsNullOrWhiteSpace(imageUploadDirectory))
                {
                    return data;
                }

                ImageManager imgManager = new ImageManager();
                data = imgManager.DownloadImage(imageName, imageUploadDirectory);
            }
            catch (Exception ex)
            {
                throw new FaultException<CustomFault>(new CustomFault("Error occured in method DownloadImage", ex.Message, (ex.InnerException == null) ? string.Empty : ex.InnerException.ToString(), ex.StackTrace), new FaultReason(ex.Message));
            }

            return data;
        }

        public bool UploadImage(ImageData imgData)
        {
            bool uploadStatus = false;

            try
            {
                if (string.IsNullOrWhiteSpace(imageUploadDirectory))
                {
                    return false;
                }

                ImageManager imgManager = new ImageManager();
                uploadStatus = imgManager.UploadImage(imgData, imageUploadDirectory);
            }
            catch (Exception ex)
            {
                throw new FaultException<CustomFault>(new CustomFault("Error occured in method UploadImage", ex.Message, (ex.InnerException == null) ? string.Empty : ex.InnerException.ToString(), ex.StackTrace), new FaultReason(ex.Message));
            }

            return uploadStatus;
        }
    }
}
