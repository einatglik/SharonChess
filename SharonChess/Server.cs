using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using SharonChess;


namespace ChessServer
{
    class Server
    {
        Thread socketThread;
        private String data = null;
        private MainMenu menu;
        private bool gameStarted = false;

        public Server(MainMenu menu)
        {
            this.menu = menu;
        }

        public void Start()
        {
            socketThread = new Thread(SocketThreadFunc);
            socketThread.Start();
        }

        public void SocketThreadFunc(object state)
        { 
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 9000.
                Random rand = new Random();
                int portinc = rand.Next(100);
                Int32 port = 9000 + portinc;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                Console.Out.WriteLine("Port: " + port);
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);
                Console.Out.Write("Start Listening ");
                // Start listening for client requests.
                server.Start();

              
                Console.Out.WriteLine("Starting server");
                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.Out.WriteLine("Connected!");

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    ReadClientMessage(stream);


                    if (!gameStarted)
                    {
                        menu.SetMode(MainMenu.MULTI);
                        //Dispatcher.CurrentDispatcher.BeginInvoke(new Action(menu.StartGame), null);
                        Dispatcher.CurrentDispatcher.Invoke(new Action(menu.StartGame), null);
                        // Dispatcher.Invoke(() =>
                        gameStarted = true;

                    }
                    else
                    {
                        Console.Out.WriteLine("Sent: {0}", data);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            //Console.WriteLine("\nHit enter to continue...");
            //Console.Read();
        }

        private void ReadClientMessage(NetworkStream stream)
        {
            
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            // Loop to receive all the data sent by the client.
            Int32 responseBytes = stream.Read(bytes, 0, bytes.Length);
            
            // Translate data bytes to a ASCII string.
            data = Encoding.ASCII.GetString(bytes, 0, responseBytes);
            Console.Out.WriteLine("Received: {0}", data);
            // Process the data sent by the client.
            byte[] msg = Encoding.ASCII.GetBytes("Black");
            // Send back a response.
            stream.Write(msg, 0, msg.Length);
            Console.Out.WriteLine("Sent: {0}", "Black");
            
        }


        public String GetMessage()
        {
            return data;
        }
    }


}
