using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO Remove this hard link for testing. 
            URLInput.Text = @"http://images.drivebc.ca/bchighwaycam/pub/cameras/65.jpg";

            var imageURL = URLInput.Text;
            var imagePath = @"c:\temp\manningPark\";
            var lastImageName = "";

            //TODO check if the dir is there if not make it

            if(String.IsNullOrEmpty(imageURL))
            {
                AddToOutput("Please enter a URL");

            }
            else
            {
                //catch invalid URL error here?
                AddToOutput("Downloading Image: " + imageURL);
                savePic(imageURL, imagePath);

                //Loop over pause for 2 mins
                while (true)
                {
                    Thread.Sleep(120000);

                    //Get the new picture but name it temp.

                    //Get the MD5 for both files.

                    //If they match do nothing

                    //Else save the file.




                }


            }


        }

        public static void savePic(string url, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                var datetimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                client.DownloadFile(new Uri(url), fileName + datetimeStamp + ".jpg");

                //OR do we want to use this?
                //client.DownloadFileAsync(new Uri(url), @"c:\temp\image35.png");
            }


        }

        private void AddToOutput(string input, Boolean DoubleNewLine = false)
        {

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
