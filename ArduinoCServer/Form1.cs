using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
   


namespace ArduinoCServer
{
    public partial class Form1 : Form
    {
        private cServerClass Server;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string StringHost;
            string StrIP;

            try
            {
                StringHost = System.Net.Dns.GetHostName();
                StrIP = Dns.GetHostByName(StringHost).AddressList[2].ToString();
                label2.Text = "Host Name:" + StringHost + "IP Address:" + StrIP;
            }
            catch(Exception ex)
            {

            }

            Server = new cServerClass();

            try
            {
                richTextBox1.AppendText("-------Arduino Server Running-------");
            }
            catch (Exception ex)
            {

            }

            Server.Message += RecInfo;

        }

        private delegate void UpdateTxtBox(RichTextBox Richbox1, string txt);

        private void UpdateTxt(RichTextBox Richbox1, string txt)
        {
            if (InvokeRequired)
                Richbox1.Invoke(new UpdateTxtBox(UpdateTxt), new object[] { richTextBox1, txt });
            else if (txt != null)
                Richbox1.AppendText(txt + "\n");

        }
        private void RecInfo(cServerClass sender, string data)
        {
            UpdateTxt(richTextBox1, data);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.IsLiserning = false;
        }
    }
}
