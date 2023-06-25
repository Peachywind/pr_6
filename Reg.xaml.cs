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
using System.Windows.Shapes;

namespace WpfApp7
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window

    {
        public class DataModel
        {
            public string MyData { get; set; }
        }

        private Socket server;
        


        public Reg()
        {
            
            InitializeComponent();
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        }

        private async Task SendMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(bytes, SocketFlags.None);
        }
        


        private void send_Click(object sender, RoutedEventArgs e)
        {
           
            string ip = ID.Text;
            server.Connect(ip, 8888);
            MainWindow window = new MainWindow();
            if (Name.Text.StartsWith(""))
            {
                MessageBox.Show("error");
            }
            else
            {
                SendMessage("[user_name]: " + Name.Text);
                this.Close();
                string name = Tag as string;
                

                SendMessage("[log]:" + "Пользователь по именем " + Name.Text + " Подключился ");
                window.Tag = Name.Text;
                window.Show();
            }
        }

        private void HandleClient(Socket clientSocket)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Admin df = new Admin();
            df.Show();
            this.Close();
            
           
            
        }
    }
}