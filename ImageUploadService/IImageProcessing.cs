using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace ImageUploadService
{
    [ServiceContract]
    public interface IImageProcessing
    {
        [OperationContract]
        [FaultContract(typeof(CustomFault))]
        ImageData DownloadImage(string imageName);

        [OperationContract]
        [FaultContract(typeof(CustomFault))]
        bool UploadImage(ImageData imgData);
    }

    [DataContract]
    public class ImageData
    {
        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public byte[] ImageBinary
        {
            get;
            set;
        }
    }

    [DataContract]
    public class CustomFault
    {
        public CustomFault(string title, string message, string innerException, string stackTrace)
        {
            ErrorTitle = title;
            ErrorMessage = message;
            InnerException = innerException;
            StackTrace = stackTrace;
        }

        [DataMember]
        public string ErrorTitle { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string InnerException { get; set; }

        [DataMember]
        public string StackTrace { get; set; }
    }
}
