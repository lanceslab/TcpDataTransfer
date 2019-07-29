using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace TcpDataTransfer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ProcessHelper ph = new ProcessHelper();
        //public TCPServer obj_server;
        public IPAddress[] ipAddressList;
        public static string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        TCPServer obj_server;
        public static string threadProcessIdString = "TcpDataTransfer.vshost.exe";
        public static int threadProcessId = 0;
        System.Threading.Thread obj_thread;

        private void btn_start_server_Click(object sender, EventArgs e)
        {
            btn_start_server.Enabled = false;
            bt_send.Enabled = true;
            string path = Path.Combine(dir, @"Audio\ironmanDataBase.wav");
            PlayBackgroundSoundFile(path);
            label2.BackColor = Color.PaleGreen;
            label2.Text = "Server Started...";//

            IPAddress ip = getIpFromTextFile();

            labelSocket.Text = "Socket opened for " + ip.ToString() + ":" + 11000.ToString();

            obj_server = new TCPServer(ip);
            //obj_server = new TCPServer((IPAddress)comboBoxIP.SelectedItem);

            obj_thread = new System.Threading.Thread(obj_server.StartServer);
            threadProcessId = obj_thread.ManagedThreadId;
            //System.Threading.Thread obj_thread = new System.Threading.Thread(obj_server.StartServer);

            obj_thread.Start();            

        }

        private IPAddress getIpFromTextFile()
        {
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = Path.Combine(dir, @"IpTextFile\thisIpAddress.txt");
            string readText = File.ReadAllText(path);
            IPAddress ip = IPAddress.Parse(readText);
            return ip;
        }

        private void bt_send_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlg_open_file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    String selected_file = dlg_open_file.FileName;
                    String file_name = Path.GetFileName(selected_file);
                    FileStream fs = new FileStream(selected_file, FileMode.Open);
                    //TcpClient tc = new TcpClient("127.0.0.1", 11000);

                    TcpClient tc = new TcpClient(comboBoxIP.SelectedText, 11000);

                    NetworkStream ns = tc.GetStream();
                    // Created packet with command 125 and the file name
                    byte[] data_tosend = CreateDataPacket(Encoding.UTF8.GetBytes("124"), Encoding.UTF8.GetBytes(file_name));
                    // Now we need the responce from the server
                    ns.Write(data_tosend, 0, data_tosend.Length);
                    ns.Flush();
                    Boolean loop_break = false;
                    while (true)
                    {
                        if (ns.ReadByte() == 2)
                        {
                            byte[] cmd_buffer = new byte[3];
                            ns.Read(cmd_buffer, 0, cmd_buffer.Length);
                            // Now we have the command and data from ReadStream
                            byte[] recv_data = ReadStream(ns);
                            switch (Convert.ToInt32(Encoding.UTF8.GetString(cmd_buffer)))
                            {
                                case 125:// 126://===========126========SEEK
                                    long recv_file_pointer = long.Parse(Encoding.UTF8.GetString(recv_data));
                                    if (recv_file_pointer != fs.Length)
                                    {
                                        fs.Seek(recv_file_pointer, SeekOrigin.Begin);
                                        int temp_buff_length = (int)(fs.Length - recv_file_pointer < 20000 ? fs.Length - recv_file_pointer : 20000);
                                        byte[] temp_buff = new byte[temp_buff_length];
                                        fs.Read(temp_buff, 0, temp_buff.Length);
                                        byte[] data_to_send = CreateDataPacket(Encoding.UTF8.GetBytes("126"), temp_buff);
                                        ns.Write(data_to_send, 0, data_to_send.Length);
                                        ns.Flush();
                                        // Now update the progress bar
                                        pb_upload.Value = (int)Math.Ceiling((double)recv_file_pointer / (double)fs.Length * 100);

                                    }
                                    else
                                    {
                                        byte[] data_to_send = CreateDataPacket(Encoding.UTF8.GetBytes("127"), Encoding.UTF8.GetBytes("Close"));
                                        ns.Write(data_to_send, 0, data_to_send.Length);
                                        ns.Flush();
                                        fs.Close();
                                        loop_break = true;
                                        pb_upload.Value = 0;// reset the progress bar
                                        labelResults.Text = "File transfer complete.";
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (loop_break == true)
                        {
                            ns.Close();
                            break;
                        }
                    }
                }
            }catch(Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "ERROR");
                MessageBox.Show(ex.ToString(), "ERROR");
            }
            string path = Path.Combine(dir, @"Audio\jarvisTheRenderIsComplete2.wav");
            PlayBackgroundSoundFile(path);
        }

        // PLAY WAV FILE IN THE BACKGROUND
        public void PlayBackgroundSoundFile(string filePath)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = filePath;// @"C:\Users\Lance\Documents\Visual Studio 2013\AtYoureService.wav";
            player.Play();
        }

        public byte[] ReadStream(NetworkStream ns)
        {
            // Convert java code into C# !!!!!!!!!!!!!

            byte[] data_buff = null;

            int b = 0;
            String buff_length = "";
            while ((b = ns.ReadByte()) != 4)
            {
                buff_length += (char)b;
            }
            int data_length = Convert.ToInt32(buff_length);
            data_buff = new byte[data_length];
            int byte_read = 0;
            int byte_offset = 0;
            while (byte_offset < data_length)
            {
                byte_read = ns.Read(data_buff, byte_offset, (data_length - byte_offset));

                byte_offset += byte_read;
            }

            return data_buff;
        }

        private byte[] CreateDataPacket(byte[]  cmd, byte[] data)
        {
            byte[] initialize = new byte[1];
            initialize[0] = 2;
            byte[] separator = new byte[1];
            separator[0] = 4;
            byte[] dataLength = Encoding.UTF8.GetBytes(Convert.ToString(data.Length));
            MemoryStream ms = new MemoryStream();
            ms.Write(initialize, 0, initialize.Length);
            ms.Write(cmd, 0, cmd.Length);
            ms.Write(dataLength, 0, dataLength.Length);
            ms.Write(separator, 0, separator.Length);
            ms.Write(data, 0, data.Length);
            return ms.ToArray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            //bool isLocalIp = false;
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //isLocalIp = IsLocalIpAddress(ipHostInfo.HostName);
            //IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
            //comboBoxIP.DataSource = ipHostInfo.AddressList;
            //comboBoxIP.SelectedIndex = 4;
            //comboBoxIP.DataSource = ReturnIpUsingStringBuilder();
            comboBoxIP.Enabled = false;
        }

        //public string ReturnIpUsingStringBuilder()
        public IPAddress[] ReturnIpUsingStringBuilder()
        {
            StringBuilder sb = new StringBuilder();
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostByName(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            IPAddress[] ipList = new IPAddress[ipEntry.AddressList.Length];
            for (int i = 0; i < addr.Length; i++)
            {
                sb.Append(addr[i].ToString() + Environment.NewLine);
                ipList[i] = addr[i];
            }
            string myip = sb.ToString();
            //ipList.Reverse();
            return ipList;
        }


        public static bool IsLocalIpAddress(string host)
        {
            try
            { // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private void buttonShowFiles_Click(object sender, EventArgs e)
        {
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = Path.Combine(dir, @"Uploads\");
             DirectoryInfo d = new DirectoryInfo(path);
            //FileInfo[] Files = d.GetFiles("*.jpg"); //Getting img files
            FileInfo[] Files = d.GetFiles(); 
            string str = "";// "Files in the download folder" + Environment.NewLine;
            int count = 0;
            foreach (FileInfo file in Files)
            {
                str = str + file.Name + Environment.NewLine;
                count++;
            }
            tx_recv_data.Text = count.ToString() + " Files in the Folder Now! " + Environment.NewLine;
            tx_recv_data.Text += str;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            label2.BackColor = Color.LightBlue;
            label2.Text = "Stopping Server";
            //obj_thread.ExecutionContext;// MAY HAVE SOMETHING HERE
            string closingMessage = obj_server.StopServer();
            tx_recv_data.Text = "Closed Socket for " + Environment.NewLine + closingMessage;

            labelSocket.Text = "All Stopped";
            string path = Path.Combine(dir, @"Audio\jarvisPoweringDownForDiagnostics.wav");
            PlayBackgroundSoundFile(path);
            btn_start_server.Enabled = true;
            //string processOwner = ph.GetProcessOwner(tempProcessID);
            // It finds the "EXCEL" processes, which started 12 hours ago and belongs to a local user "appdev01" in the machine "myPCName", and kill them.
            //ph.KillProcessByNameAndUser("EXCEL", "myPCName\appdev01", 12);
            //bool isClosing = ph.KillProcessByNameAndUser("TcpDataTransfer.exe", @"LanceLapTop\LanceTaylor", 0);

            //if (isClosing)
            //    labelSocket.Text = "Stoped thread";
            //else 
            //    labelSocket.Text = "Failed stopping thread";


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //string path = Path.Combine(dir, @"Audio\jarvisPoweringDownForDiagnostics.wav");
            //PlayBackgroundSoundFile(path);
            try
            {
                string closingMessage = obj_server.StopServer();
            }
            catch (Exception closeExeption)
            {
                tx_recv_data.ForeColor = Color.Red;
                tx_recv_data.Text = "closeExeption";
            }
            //this.Close();

            //bool isClosing = ph.KillProcessByNameAndUser("TcpDataTransfer.exe", @"LanceLapTop\LanceTaylor", 0);

            //if (isClosing)
            //    labelSocket.Text = "Stoped thread";
            //else
            //    labelSocket.Text = "Failed stopping thread";

            //obj_thread.Abort();
        }
    }



    class TCPServer
    {
        TcpListener obj_server;
        bool isListening = false;
        public TCPServer(IPAddress chosen)
        {
            ////obj_server = new TcpListener(IPAddress.Any, 11000);
            obj_server = new TcpListener(chosen, 11000);
        }

        public void StartServer()
        {
            isListening = true;
            obj_server.Start();
            while (isListening)//while (true)
            {
                try
                {
                    TcpClient tc = obj_server.AcceptTcpClient();
                    // pass the TcpClient object to the SocketHandler
                    SocketHandler obj_handler = new SocketHandler(tc);
                    System.Threading.Thread obj_thread = new System.Threading.Thread(obj_handler.ProcessSocketRequest);
                    // i was missing 
                    obj_thread.Start();
                } catch (SocketException ex)
                {
                   // MessageBox.Show("Problem because of stoppingthe socket");
                    break;
                }
            }          
        }
        public string StopServer()
        //public void StopServer()
        {
            isListening = false;
            obj_server.Stop();

            string p = obj_server.LocalEndpoint.ToString();
            return p;
            //while (true)
            //{
            //    TcpClient tc = obj_server.AcceptTcpClient();
            //    // pass the TcpClient object to the SocketHandler
            //    SocketHandler obj_handler = new SocketHandler(tc);
            //    System.Threading.Thread obj_thread = new System.Threading.Thread(obj_handler.ProcessSocketRequest);
            //    // i was missing 
            //    obj_thread.Start();

            //}
        }


        class SocketHandler
        {
            NetworkStream ns;
            public SocketHandler(TcpClient tc)
            {
                ns = tc.GetStream();
            }
            // Function to run as Thread
            public void ProcessSocketRequest()
            {
                FileStream fs = null;
                long current_file_pointer = 0;
                Boolean loop_break = false;
                String fName = "";
                while (true)
                {
                    if (ns.ReadByte() == 2)
                    {
                        byte[] cmd_buffer = new byte[3];
                        ns.Read(cmd_buffer, 0, cmd_buffer.Length);
                        // Now we have the command and data from ReadStream
                        byte[] recv_data = ReadStream();
                        int switchCaseValue = Convert.ToInt32(Encoding.UTF8.GetString(cmd_buffer));
                        switch (Convert.ToInt32(Encoding.UTF8.GetString(cmd_buffer)))
                        {
                            case 124:// 125:// Got it all
                                {
                                    string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                                    string path = Path.Combine(dir, @"Uploads\");
                                    String pathToSave2 = path;// @"C:\Users\GSSPA\Documents\Visual Studio 2015\Projects\TcpDataTransfer\Uploads\";


                                    

                                    fs = new FileStream(pathToSave2 + Encoding.UTF8.GetString(recv_data),                   FileMode.Create);

                                    //fName = Encoding.UTF8.GetString(recv_data);
                                    
                                    //Form1.ActiveForm.Invoke(new MethodInvoker(delegate
                                    //{
                                    //    ((TextBox)Form1.ActiveForm.Controls.Find                            ("tx_recv_data", true)[0]).Text += 
                                    //    Environment.NewLine + fName;

                                    //}));
// creating packet with command 126 and pointer to                  initial file
                                    byte[] data_to_send = CreateDataPacket(Encoding.UTF8.GetBytes("125"),               Encoding.UTF8.GetBytes(Convert.ToString(current_file_pointer)));
                                    ns.Write(data_to_send, 0, data_to_send.Length);
                                    ns.Flush();
                                }
                                break;
                            case 126:// 127:// seek for the rest
                                { 
                                    fs.Seek(current_file_pointer, SeekOrigin.Begin);
                                    fs.Write(recv_data, 0, recv_data.Length);
                                    // now update the current file pointer
                                    current_file_pointer = fs.Position;
                                    byte[] data_to_send = CreateDataPacket(Encoding.UTF8.GetBytes("125"), Encoding.UTF8.GetBytes(Convert.ToString(current_file_pointer)));
                                    ns.Write(data_to_send, 0, data_to_send.Length);
                                    ns.Flush();
                                    //int curPercent = (int)Math.Ceiling((double)current_file_pointer / (double)fs.Length * 100);
                                    //Form1.ActiveForm.Invoke(new MethodInvoker(delegate
                                    //{
                                    //    ((ProgressBar)Form1.ActiveForm.Controls.Find("pb_upload", true)                     [0]).Value = curPercent;

                                    //}));
                                }
                                break;
                            case 127:// 128://labelSocket//tx_recv_data
                                {
                                    //Form1.ActiveForm.Invoke(new MethodInvoker(delegate
                                    //{
                                    //    ((Label)Form1.ActiveForm.Controls.Find("labelSocket", true)[0]).Text += Encoding.UTF8.GetString(recv_data);

                                    //}));
                                    //Form1.ActiveForm.Invoke(new MethodInvoker(delegate
                                    //{
                                    //    ((TextBox)Form1.ActiveForm.Controls.Find("tx_recv_data", true)[0]).Text += "  " + fName;

                                    //}));
       
                                    fs.Close();
                                    loop_break = true;
                                }
                                break;
                            case 129:
                                //string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                                //string path = Path.Combine(dir, @"Uploads\");
                                //String pathToSave2 = path;

                                //string licenseInfo = new FileStream(pathToSave2 + Encoding.UTF8.GetString(recv_data), FileMode.Create);


                                break;
                            default:
                                break;
                        }

                        
                    }
                    if(loop_break == true)
                    {
                        ns.Close();
                        
                        break;
                    }
                }
            }

            public byte[] ReadStream()
            {
                // Convert java code into C# !!!!!!!!!!!!!

                byte[] data_buff = null;

                int b = 0;
                String buff_length = "";
                while ((b = ns.ReadByte()) != 4)
                {
                    buff_length += (char)b;
                }
                int data_length = Convert.ToInt32(buff_length);
                data_buff = new byte[data_length];
                int byte_read = 0;
                int byte_offset = 0;
                while (byte_offset < data_length)
                {
                    byte_read = ns.Read(data_buff, byte_offset, (data_length - byte_offset));

                    byte_offset += byte_read;
                }

                return data_buff;
            }

            private byte[] CreateDataPacket(byte[] cmd, byte[] data)
            {
                byte[] initialize = new byte[1];
                initialize[0] = 2;
                byte[] separator = new byte[1];
                separator[0] = 4;
                byte[] dataLength = Encoding.UTF8.GetBytes(Convert.ToString(data.Length));
                MemoryStream ms = new MemoryStream();
                ms.Write(initialize, 0, initialize.Length);
                ms.Write(cmd, 0, cmd.Length);
                ms.Write(dataLength, 0, dataLength.Length);
                ms.Write(separator, 0, separator.Length);
                ms.Write(data, 0, data.Length);
                return ms.ToArray();
            }

        }





    }




}

