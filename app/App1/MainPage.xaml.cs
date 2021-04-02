using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //TODO : A portszámot külső fájlból  
        static Int32 PortNumber = 13000;
        static string IP = "192.168.8.253";
        public MainPage()
        {
            this.InitializeComponent();
            NumberOutputBox.Text =SendData(IP, "Start", PortNumber);
        }

        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            string sendData = NumberInputBox.Text;
            int number;
            
            if (!(string.IsNullOrWhiteSpace(sendData)) && Int32.TryParse(sendData, out number))
            {
                NumberOutputBox.Text = SendData(IP, sendData, PortNumber);
                NumberInputBox.Text = "";
            }
           

            //ReceiveData();
         }

        public string SendData(String server, String message, Int32 port)
        {
            try
            {
                
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                                                                    
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    data = new Byte[256];
                    String response = String.Empty;
                
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                               
                stream.Close();
                client.Close();
                return response;
            }
            catch (Exception e)
            {
                StatusField.Text = e.Message;
                return null;
            }
            
        }
                      
    }
}
