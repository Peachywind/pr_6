using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket server;
        public MainWindow()
        {
            InitializeComponent();
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect("26.83.193.10", 8888);
            ReceiveMessage();
        }

        private async Task SendMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(bytes, SocketFlags.None);
        }

        private async Task ReceiveMessage()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);

                if (message.StartsWith("[user_name]:"))
                {
                    string[] parts = message.Split(':');
                    if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
                    {
                        MessageLbx.Items.Add(parts[1]);
                    }
                   
                }
                else if (message.StartsWith("[name]:"))
                {
                    string[] part = message.Split(':');
                    if (part.Length > 1 && !string.IsNullOrWhiteSpace(part[1]))
                    {
                        clients.Items.Add(part[1]);
                    }
                    if (part[1] == "")
                    {
                        MessageBox.Show("EYF");
                    }
                                        }
            }
        }

        private async Task Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string name = Tag as string;

            SendMessage("[disconnect]:" + name);
            SendMessage("[log]:" + name + " Отключился");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = Tag as string;
            if (MessageTxt.Text == "/disconnect")
            {
                Reg reg = new Reg();
                this.Close();
                reg.Show();
                SendMessage("[disconnect]:" + name);
                SendMessage("[log]:" + name + " Отключился");
            }
            else if (!string.IsNullOrWhiteSpace(MessageTxt.Text))
            {
                SendMessage("[name]:" + " " + "Сообщение от " + name + " " + MessageTxt.Text);
            }
        }

        private async Task Button_Click_1(object sender, RoutedEventArgs e)
        {
            Reg reg = new Reg();
            string name = Tag as string;
            this.Close();
            reg.Show();
            SendMessage("[disconnect]:" + name);
            SendMessage("[log]:" + name + " Отключился");
        }
    }
}
