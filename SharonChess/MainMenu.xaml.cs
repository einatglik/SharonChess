using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using ChessServer;

namespace SharonChess
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
       
        public static int OFFLINE = 0;
        public static int SINGLE = 1;
        public static int MULTI = 2;

        private Server s;
        private TcpClient client;
        private String name;
        private TextBox nickname;
        private TextBox portInput;
        private Player myself;
        private Player oponent;

        private MainWindow w;
        private int mode = SINGLE; 


        public MainMenu()
        {
            InitializeComponent();
            Button offlineGameButton = new Button();
            offlineGameButton.Width = 80;
            offlineGameButton.Height = 30;
            offlineGameButton.Content = "Play offline";
            MainCanvas.Children.Add(offlineGameButton);
            Canvas.SetLeft(offlineGameButton, 213);
            Canvas.SetTop(offlineGameButton, 190);
            offlineGameButton.Click += OfflineGameButtonClick;           
            Button onlineGameButton = new Button();
            onlineGameButton.Width = 80;
            onlineGameButton.Height = 30;
            onlineGameButton.Content = "Play online";
            onlineGameButton.Click += OnlineGameButton_Click;
            MainCanvas.Children.Add(onlineGameButton);
            Canvas.SetLeft(onlineGameButton, 506);
            Canvas.SetTop(onlineGameButton, 190);
            Button waitForPlayer = new Button();
            waitForPlayer.Width = 80;
            waitForPlayer.Height = 30;
            waitForPlayer.Content = "Wait for player";
            waitForPlayer.Click += WaitForPlayer_Click;
            MainCanvas.Children.Add(waitForPlayer);
            Canvas.SetLeft(waitForPlayer, 400);
            Canvas.SetTop(waitForPlayer, 190);
            TextBlock enterName = new TextBlock();
            enterName.Width = 200;
            enterName.Height = 30;
            MainCanvas.Children.Add(enterName);
            Canvas.SetLeft(enterName, 150);
            Canvas.SetTop(enterName, 50);
            enterName.Text = "Enter your nickname below:";
            nickname = new TextBox();
            nickname.Width = 200;
            nickname.Height = 30;           
            MainCanvas.Children.Add(nickname);
            Canvas.SetLeft(nickname, 150);
            Canvas.SetTop(nickname, 80);
            TextBlock enterPort = new TextBlock();
            enterPort.Width = 200;
            enterPort.Height = 30;
            MainCanvas.Children.Add(enterPort);
            Canvas.SetLeft(enterPort, 500);
            Canvas.SetTop(enterPort, 50);
            enterPort.Text = "Enter the port:";
            portInput = new TextBox();
            portInput.Width = 200;
            portInput.Height = 30;
            MainCanvas.Children.Add(portInput);
            Canvas.SetLeft(portInput, 500);
            Canvas.SetTop(portInput, 80);
            w = new MainWindow();
        }

        public void SetMode(int mode)
        {
            this.mode = mode;
        }

        private void WaitForPlayer_Click(object sender, RoutedEventArgs e)
        {
            Console.Out.WriteLine("Starting server");
            s = new Server(this);
            s.Start();
        }

        private void OnlineGameButton_Click(object sender, RoutedEventArgs e)
        {

            Int32 port = Int32.Parse(portInput.Text);

            String address = "127.0.0.1";
            name = nickname.Text;
            Random rand = new Random();
            bool randomBoolean = rand.Next(2) == 0;
            myself = new Player(randomBoolean, name);
            Connect(address, port);
            String nameOponent = SendMessage(myself.getName());
           // String response = SendMessage((!randomBoolean).ToString());
           
            w.Show();
            
            Console.Out.WriteLine(myself.ToString());

            oponent = new Player(!randomBoolean, nameOponent);
            Console.Out.WriteLine(oponent.ToString());
            w.SetPlayers(myself, oponent);
            this.Close();
        }

        [STAThread]
        public void StartGame()
        {
            w.Show();
            name = nickname.Text;
            myself = new Player(false, name);
            Console.Out.WriteLine(myself.ToString());

            oponent = new Player(true, s.GetMessage());
            Console.Out.WriteLine(oponent.ToString());
            w.SetPlayers(myself, oponent);
            this.Close();

        }

        private void OfflineGameButtonClick(object sender, EventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            //this.Visibility = Visibility.Collapsed;
            this.Close();

        }

    

        public void Connect(String server, Int32 port)
        {
           
            try
            {
                // Create a TcpClient.
             
                client = new TcpClient(server, port);
                // Explicit close is not necessary since TcpClient.Dispose() will be called automatically.
                // stream.Close();
                // client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.Out.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.Out.WriteLine("SocketException: {0}", e);
            }
        }


        public String SendMessage(String message)
        {
            String responseData = "";
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.Out.WriteLine("Sent: {0}", message);

                // Receive the server response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.Out.WriteLine("Received: {0}", responseData);
            }
            catch (ArgumentNullException e)
            {
                Console.Out.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.Out.WriteLine("SocketException: {0}", e);
            }

            //Console.Out.WriteLine("\n Press Enter to continue...");
            //Console.Read();
            return responseData;

        }
    }
}
