using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void upload()
        {
            try
            {
                string url = ConfigurationManager.AppSettings["url"];
                //string requestUrl = string.Format("{0}/{1}/{2}", url, "Upload", "de.jpg");

                //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                //request.Method = "POST";
                //request.ContentType = "text/plain";

                //byte[] fileToSend = File.ReadAllBytes(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg");
                //request.ContentLength = fileToSend.Length;

                //using (Stream requestStream = request.GetRequestStream())
                //{
                //    requestStream.Write(fileToSend, 0, fileToSend.Length);
                //    requestStream.Close();
                //}

                //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //    Console.WriteLine("HTTP/{0} {1} {2}", response.ProtocolVersion, (int)response.StatusCode, response.StatusDescription);

                //byte[] fileToSend = File.ReadAllBytes(@"C:\Users\Public\Pictures\Sample Pictures\Tulips.jpg");

                //string url = ConfigurationManager.AppSettings["url"];
                //string requestUrl = string.Format("{0}/{1}/{2}/{3}", url, "Upload", "Desert.jpg", fileToSend);

                //WebClient request = new WebClient();
                //string imgString = request.DownloadString(requestUrl);

                ImageData imgData = new ImageData();
                imgData.Name = "de.jpg";
                imgData.ImageContents = Encoding.UTF8.GetString(File.ReadAllBytes(@"E:\Projects\Omni-ID\Bartender Images\Images\Or.png"));

                string imageXml = this.GetXml<ImageData>(imgData);

                //using (WebClient client = new WebClient())
                //{
                //    client.Encoding = System.Text.Encoding.UTF8;
                //    client.Headers.Add("Content-Type", "application/xml");
                //    client.UploadString(new Uri(string.Format("{0}/{1}", url, "UploadImage"), UriKind.RelativeOrAbsolute), "POST", imageXml);
                //}

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", url, "UploadImage"), UriKind.RelativeOrAbsolute));
                request.ContentType = "application/xml";
                request.Method = "POST";
                request.KeepAlive = true;

                using (Stream requestStream = request.GetRequestStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(imageXml);
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();
                var abc = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
               
            }
        }

        private void Download()
        {
            try
            {
                string url = ConfigurationManager.AppSettings["url"];
                string requestUrl = string.Format("{0}/{1}/{2}", url, "Download", "Desert.jpg");

                WebClient request = new WebClient();
                string imgString = request.DownloadString(requestUrl);
                byte[] data = (byte[])DeserializeXML(imgString, typeof(byte[]));
                File.WriteAllBytes(System.IO.Path.Combine(ConfigurationManager.AppSettings["Path"], "811.jpg"), data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // upload();
           // Download();
        }


        private string GetXml<T>(T obj)
        {
            string xmlStr = string.Empty;
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Position = 0;
                serializer.WriteObject(stream, obj);
                xmlStr = Encoding.Default.GetString(stream.ToArray());
            }

            return xmlStr;
        }

        private string GetXmlUsingXmlSerializer<T>(T obj)
        {
            string xmlStr = string.Empty;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Position = 0;
                serializer.Serialize(stream, obj, ns);
                xmlStr = Encoding.Default.GetString(stream.ToArray());
            }

            return xmlStr;
        }

        private object DeserializeXML(string Xml, Type ObjType)
        {
            DataContractSerializer ser;
            ser = new DataContractSerializer(ObjType);
            StringReader stringReader;
            stringReader = new StringReader(Xml);
            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);
            object obj;
            obj = ser.ReadObject(xmlReader);
            xmlReader.Close();
            stringReader.Close();
            return obj;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Download();
            upload();
        }

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
        public string ImageContents
        {
            get;
            set;
        }
    }
}
