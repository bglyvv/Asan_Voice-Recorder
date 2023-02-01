using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VoiceRecorder
{
    public partial class Form1 : Form
    {
        
        DesktopHub hub = new DesktopHub("http://localhost:5000/recordhub");
        public Form1()
        {
            InitializeComponent();
            string IP = "";
            var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(c => !c.ToString().Contains(":") && c.ToString().Split('.').Length == 4);
            if (ip != null)
            {
                IP = ip.ToString();
            }
            hub.IP= IP;
            ipadress.Text = IP;
            hub.Connect();
           

            Console.ReadLine();

        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            hub.Disconnect();
        }
    }
}
