using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageUploadService.ImageServiceManager
{
    public class ImageManager
    {
        public ImageData DownloadImage(string imageName, string imageDownloadDirectory)
        {
            ImageData imgData = new ImageData();
            FileStream fileStream = null;
            BinaryReader reader = null;
            string imagePath;
            byte[] imageBytes;

            try
            {
                if (imgData != null)
                {
                    if (!string.IsNullOrWhiteSpace(imageName))
                    {
                        imagePath = Path.Combine(imageDownloadDirectory, imageName);

                        if (File.Exists(imagePath))
                        {
                            fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                            reader = new BinaryReader(fileStream);
                            imageBytes = reader.ReadBytes((int)fileStream.Length);

                            imgData.Name = imageName;
                            imgData.ImageBinary = imageBytes;
                        }
                    }
                }

                return imgData;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }

        }

        public bool UploadImage(ImageData imgData, string imageUploadDirectory)
        {
            FileStream fileStream = null;
            BinaryWriter writer = null;
            string filePath = string.Empty;

            try
            {
                if (imgData != null)
                {
                    if (!string.IsNullOrWhiteSpace(imgData.Name))
                    {
                        filePath = Path.Combine(imageUploadDirectory, imgData.Name);

                        fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                        writer = new BinaryWriter(fileStream);
                        writer.Write(imgData.ImageBinary);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
                if (writer != null)
                    writer.Close();
            }
        }
    }
}