using server;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;
using static server.Models.Dataview;
using static server.Data;

class Server
    {
        TcpListener server = null;
        public Server(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();
            StartListener();
        }

        public void StartListener()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }

        public void HandleDeivce(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
                        
            Byte[] bytes = new Byte[256];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    var utf8Reader = new Utf8JsonReader(bytes);
                    NumberSend data = JsonSerializer.Deserialize<NumberSend>(ref utf8Reader);

                                  
                    // Get the constructor and create an instance of MagicClass

                    Type callClassType = Type.GetType("server.Controller.AppMain");
                    ConstructorInfo callClassConstructor = callClassType.GetConstructor(Type.EmptyTypes);
                    object callClassObject = callClassConstructor.Invoke(new object[] { });

                    // Get the ItsMagic method and invoke with a parameter value of 100

                    MethodInfo callMethod = callClassType.GetMethod(data.Command);
                    object returnData = callMethod.Invoke(callClassObject, new object[] { data.Nums });

                                   
                    Console.WriteLine("{1}: Received: {0}", data.Command, Thread.CurrentThread.ManagedThreadId);
                                    
                    byte[] reply = JsonSerializer.SerializeToUtf8Bytes(returnData, options);
                    

                    stream.Write(reply, 0, reply.Length);
                    Console.WriteLine("{1}: Sent: {0}", returnData, Thread.CurrentThread.ManagedThreadId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
        }
    }
