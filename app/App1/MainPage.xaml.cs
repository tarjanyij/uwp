using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Net.Sockets;
using System.Xml;
using System.Configuration;
using static App1.Models.Dataview;

using System.Text.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        //static Int32 PortNumber = 13000;
        //static string IP = "192.168.8.253";
        static string IP = ConfigurationManager.AppSettings.Get("ip");
        static Int32 PortNumber = Int32.Parse(ConfigurationManager.AppSettings.Get("port"));
        

        public MainPage()
        {
            this.InitializeComponent();
            NumberSend number = new NumberSend() { Command = "ReadColoum" };
            var reply = SendData(IP, number, PortNumber);
            var utf8Reader = new Utf8JsonReader(reply);

            NumberReceive response = JsonSerializer.Deserialize<NumberReceive>(ref utf8Reader);
            NumberOutputBox.Text = response.Nums.ToString();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
           
            if (CheckNumber(NumberInputBox.Text))
            {
                NumberSend number = new NumberSend(){ Command = "AddNumber" , Nums = Int32.Parse(NumberInputBox.Text) };
                var reply = SendData(IP, number, PortNumber);
                var utf8Reader = new Utf8JsonReader(reply);

                NumberReceive response = JsonSerializer.Deserialize<NumberReceive>(ref utf8Reader);
                NumberOutputBox.Text = response.Nums.ToString();
                NumberInputBox.Text = "";
            }
                       
        }
        private void SubtractsButtonClick(object sender, RoutedEventArgs e)
        {
           
            if (CheckNumber(NumberInputBox.Text))
            {
           
                NumberSend number = new NumberSend() { Command = "SubtractsNumber", Nums = Int32.Parse(NumberInputBox.Text) };
                var reply = SendData(IP, number, PortNumber);
                var utf8Reader = new Utf8JsonReader(reply);

                NumberReceive response = JsonSerializer.Deserialize<NumberReceive>(ref utf8Reader);
                NumberOutputBox.Text = response.Nums.ToString();
                NumberInputBox.Text = "";
            }
           

        }

        private bool CheckNumber(string boxNum)
        {
            int n;
            
            if (!(string.IsNullOrWhiteSpace(boxNum)) && Int32.TryParse(boxNum, out n))
            {
                if (Int32.Parse(boxNum) >= 0)
                {
                    return true;
                }
                StatusMessage("Kérem pozitív számot írjon be");
                return false;
            }
            else
            {
                StatusMessage("Kérem számot írjon be");
                return false;
            }
                
        }

        public byte[] SendData(String server, object message, Int32 port)
        {
            try
            {

                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();

                
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                byte[] data = JsonSerializer.SerializeToUtf8Bytes(message, options);
                stream.Write(data, 0, data.Length);
                
                byte[] bytes = new Byte[256];
                
                int data1 = stream.Read(bytes, 0, bytes.Length);
                

                stream.Close();
                client.Close();
                return bytes;
            }
            catch (Exception e)
            { 
                StatusMessage(e.Message);
                return null;
            }

        }

        private void StatusMessage(string msg)
        {
            StatusField.Text = msg;
        }
                
    }
}
