using System;
using System.Drawing;
using System.Windows.Forms;

namespace MCProtocolPLCEmulator
{
    public partial class Mainform : Form
    {

        MCProtocolServer _mcProtocolServer;
        SuperSimpleTcp.SimpleTcpServer _tcpServer = null;

        private void btnLaunchTcpServer_Click(object sender, EventArgs e)
        {
            //
            tbConnectionLog.Text = string.Empty;
            //

            _mcProtocolServer = new MCProtocolServer();
            _mcProtocolServer.Logger = AddLoggingMessage;

            _tcpServer = _mcProtocolServer.CreateServerInstance(tbServerAddress.Text, Convert.ToInt32(tbServerPort.Text), _plcWordMemories);
            if (_tcpServer != null)
            {
                tbServerAddress.BackColor = Color.Lime;
                tbServerAddress.ReadOnly = true;

                tbServerPort.BackColor = Color.Lime;
                tbServerPort.ReadOnly = true;

                _tcpServer.Events.ClientConnected += Events_ClientConnected;
                _tcpServer.Events.ClientDisconnected += Events_ClientDisconnected;

                //
                _tcpServer.Start();
                //
            }
        }

        private void Events_ClientConnected(object sender, SuperSimpleTcp.ConnectionEventArgs e)
        {
            tbClientConnectionStatus.BeginInvoke(new MethodInvoker(() =>
            {
                tbClientConnectionStatus.BackColor = Color.Lime;
                tbClientConnectionStatus.Text = $"Client Connected From: {e.IpPort}";
            }));
        }

        private void Events_ClientDisconnected(object sender, SuperSimpleTcp.ConnectionEventArgs e)
        {
            tbClientConnectionStatus.BeginInvoke(new MethodInvoker(() =>
            {
                tbClientConnectionStatus.BackColor = Color.DarkGray;
                tbClientConnectionStatus.Text = "No Clients";
            }));
        }

        private void AddLoggingMessage(object sender, string msg)
        {
            tbConnectionLog?.BeginInvoke(new MethodInvoker(() => { AddLoggingMessageInvoker(msg); }));
        }

        private void AddLoggingMessageInvoker(string msg)
        {
            tbConnectionLog.Text += msg + Environment.NewLine;
            tbConnectionLog.SelectionStart = tbConnectionLog.Text.Length;
            tbConnectionLog.ScrollToCaret();
        }
    }
}
