using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace WpfApp7
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private Socket socket;
        private List<Socket> clients = new List<Socket>();
        private Socket client;
        int a = 1;
        

        public Admin()
        {
            InitializeComponent();
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            log.Items.Add(currentTime + " admin");
            ListenToClients();
        }
        private async Task ListenToClients()
        {
            while (true)
            {
                var client = await socket.AcceptAsync();
                clients.Add(client);
                RecieveMessage(client);
            }
        }
        private async Task RecieveMessage(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                string[] parts = message.Split(':');
                if (message.StartsWith("[user_name]:"))
                {
                    List_for_user.Items.Add(parts[1]);
                }
                else if (message.StartsWith("[name]:"))
                {
                    MessagesLbx.Items.Add(parts[1]);
                }
                else if (message.StartsWith("[disconnect]:"))
                {
                    List_for_user.Items.Remove(" " + parts[1]);
                }
                if (message.StartsWith("[log]:"))
                {
                    log.Items.Add(parts[1]);
                }
                foreach (var item in clients)
                {
                    SendMessage(item, message);
                }
            }
        }


        private async Task SendMessage(Socket client, string message)
        {

            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes, SocketFlags.None);


        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            SenddMess("[name]:" + " " + "Сообщение от " + "Admin" + " " + MessageTxt.Text);

        }
        private async Task SenddMess(string message)
        {
            foreach (var item in clients)
            {
                SendMessage(item, message);

            }
            string[] parts = message.Split(':');
            MessagesLbx.Items.Add(parts[1]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            a = a + 1;
            if (a % 2 == 0)
            {
                log.Visibility = Visibility.Collapsed;
            }
            else
            {
                log.Visibility = Visibility.Visible;
            }

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Reg rg = new Reg();
            rg.Show();
        }
    }
    
}

       
    
   

