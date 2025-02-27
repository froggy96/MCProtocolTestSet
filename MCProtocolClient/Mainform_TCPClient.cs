using MCProtocol;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MCProtocolClient
{
    public partial class Mainform : Form
    {

        McProtocolTcp _mcProtocolTcpClient;

        private void OpenConnection(string addr, string port)
        {
            int portNumber = int.Parse(port);
            _mcProtocolTcpClient = new McProtocolTcp(addr, portNumber);
            _mcProtocolTcpClient.Open();
            _mcProtocolTcpClient.ConnectionChanged += OnConnectionChanged;
        }

        private void OnConnectionChanged(object sender, bool e)
        {
#if DEBUG
            Console.WriteLine($"Connection: {e}");
#endif
            tbClientConnectionStatus.BeginInvoke(new MethodInvoker(() =>
            {
                tbClientConnectionStatus.BackColor = e ? Color.Lime : Color.DarkGray;
                tbClientConnectionStatus.Text = $"Connected: {e}";
            }));
        }


        private void CloseConnection()
        {
            if (_mcProtocolTcpClient != null) { _mcProtocolTcpClient?.Close(); }
        }
    }
}
