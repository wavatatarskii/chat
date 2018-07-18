using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using RemoteBase;
namespace RemoteServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpChannel channel;
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (channel == null)
            {
                channel = new TcpChannel(1080);
                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(SampleObject), "dialog", WellKnownObjectMode.Singleton);

                lblStatus.Text = "Работает...";
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
                channel = null;
                lblStatus.Text = "Остановлен.";
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

