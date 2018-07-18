using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Data.OleDb;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using RemoteBase;
namespace RemotingClient
{
    public partial class frmLogin : Form
    {
        TcpChannel chan;        
        ArrayList alOnlineUser = new ArrayList();
        frmChatWin objChatWin;
        OleDbConnection con = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\\Downloads\\ChatRoom_localized\\USERS.mdb;User ID=;Password=; Persist Security Info=False");
        OleDbDataAdapter Ad = new OleDbDataAdapter();
        DataSet ds = new DataSet();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            JoinToChatRoom();


            DBase_write();
            
            //try
            //{
            //    JoinToChatRoom();

            //}
            //catch(System.Net.Sockets.SocketException)
            //{
            //    MessageBox.Show("Ќевозможно подсоединитьс€, технические неполадки на сервере","ќшибка" ,MessageBoxButtons.OK,MessageBoxIcon.Error);
            //}

        }
        private void DBase_write()
        {
            try
            {
                Ad.InsertCommand = new OleDbCommand("insert into SERVER_CONFIG values (@USERNAME,@SERVER)", con);
                //Ad.InsertCommand.Parameters.Add("@SERVER", OleDbType.VarChar).Value = txtServerAdd.Text.ToString();
                Ad.InsertCommand.Parameters.Add("@USERNAME", OleDbType.VarChar).Value = txtName.Text.ToString();
                con.Open();
                Ad.InsertCommand.ExecuteNonQuery();
                con.Close();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void JoinToChatRoom()
        {
            if (chan == null && txtName.Text.Trim().Length != 0)
            {
                chan = new TcpChannel();
                ChannelServices.RegisterChannel(chan,false);
                try
                {
                    // Create an instance of the remote object
                    objChatWin = new frmChatWin();
                    objChatWin.remoteObj = (SampleObject)Activator.GetObject(typeof(RemoteBase.SampleObject), txtServerAdd.Text);

                    if (!objChatWin.remoteObj.JoinToChatRoom(txtName.Text))
                    {
                        MessageBox.Show(txtName.Text + " пользователь с таким именем уже находитс€ в диаоге, выберите другое");
                        ChannelServices.UnregisterChannel(chan);
                        chan = null;
                        objChatWin.Dispose();
                        return;
                    }
                    objChatWin.key = objChatWin.remoteObj.CurrentKeyNo();

                    objChatWin.yourName = txtName.Text;

                    this.Hide();
                    objChatWin.Show();

                }
                catch (System.Net.Sockets.SocketException)
                {
                    MessageBox.Show("Ќевозможно подсоединитьс€, технические неполадки на сервере", "ќшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //// Create an instance of the remote object
                //objChatWin = new frmChatWin();
                //objChatWin.remoteObj = (SampleObject)Activator.GetObject(typeof(RemoteBase.SampleObject), txtServerAdd.Text);

                //if (!objChatWin.remoteObj.JoinToChatRoom(txtName.Text))
                //{
                //    MessageBox.Show(txtName.Text+ " пользователь с таким именем уже находитс€ в диаоге, выберите другое");
                //    ChannelServices.UnregisterChannel(chan);
                //    chan = null;
                //    objChatWin.Dispose();
                //    return;
                //}
                //objChatWin.key = objChatWin.remoteObj.CurrentKeyNo();
                
                //objChatWin.yourName= txtName.Text;

                //this.Hide();
                //objChatWin.Show();
                
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void txtServerAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.BMP;*JPG;*.GIF;*.PNG)|*.BMP;*JPG;*.GIF;*.PNG|All files(*.*)|*.*";

            if (ofd.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image =new  Bitmap(ofd.FileName);
                }
                catch
                {
                    MessageBox.Show("Unable to upload an image","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}