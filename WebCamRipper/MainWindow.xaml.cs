using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebCamRipper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Output = "";
        private static string lastImageName;

        public MainWindow()
        {
            InitializeComponent();

            //TODO Remove this hard link for testing. 
            URLInput.Text = @"http://images.drivebc.ca/bchighwaycam/pub/cameras/65.jpg";
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        { 
            var  imageURL = URLInput.Text;
            var  imagePath = @"c:\temp\manningPark\";

            //TODO check if the dir is there if not make it

            if(String.IsNullOrEmpty(imageURL))
            {
                AddToOutput("Please enter a URL");

            }
            else
            {
                //catch invalid URL error here?
                AddToOutput("Downloading first Image: " + imageURL);
                savePic(imageURL, imagePath);

                //Loop over pause for 2 mins
                while (true)
                {
                    AddToOutput("Sleeping for 30 seconds");
                    Thread.Sleep(30000);

                    AddToOutput("Saving temp file to disk to compare against last downloaded picture.");
                    //Get the new picture but name it temp.
                    savePic(imageURL, imagePath, true);

                    //Get the MD5 for both files.

                    var MD5ofLastPic = CalculateMD5(lastImageName);
                    var MD5ofTemp = CalculateMD5(@"c:\temp\manningPark\temp.jpg");


                    //If they dont match its a new pic save it to disk.
                    if (!(MD5ofLastPic == MD5ofTemp))
                    {
                        AddToOutput("New picture saving to disk.");
                        savePic(imageURL, imagePath);
                    }
                    else
                    {
                        AddToOutput("SSame Picture not saving.");
                    }
                }
            }
        }

        public static void savePic(string url, string fileName, bool nameTemp = false)
        {
            using (WebClient client = new WebClient())
            {
                var datetimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                fileName = fileName + datetimeStamp + ".jpg";

                //only commit the last filename saved if your not saving a temp file.
                if(nameTemp == false)
                    lastImageName = fileName;

                if (nameTemp)
                    fileName = @"c:\temp\manningPark\" + @"temp.jpg";

                client.DownloadFile(new Uri(url), fileName);

                //OR do we want to use this?
                //client.DownloadFileAsync(new Uri(url), @"c:\temp\image35.png");
            }
        }

        private void AddToOutput(string input, Boolean DoubleNewLine = false, bool showDateTime = true)
        {
            var datetimeStamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            if (showDateTime)
                Output += datetimeStamp + " " + input;
            else
                Output += input;


            if (!DoubleNewLine)
                Output += Environment.NewLine;
            else
                Output += Environment.NewLine + Environment.NewLine;

            MainOutputTextBox.Text = Output;
        }

        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        } 
    }
}
