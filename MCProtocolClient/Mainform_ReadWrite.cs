using MCProtocol;
using System;
using System.Windows.Forms;

namespace MCProtocolClient
{
    public partial class Mainform : Form
    {
        private bool ReadBlock(string addr, int count, int[] data)
        {
            return _mcProtocolTcpClient.ReadDeviceBlock(addr, count, data) == 0x00;
        }

        private bool WriteBlock(string addr, int count, int[] data)
        {
            return _mcProtocolTcpClient.WriteDeviceBlock(addr, count, data) != 0x00;
        }
    }
}
