using MCProtocol;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MCProtocolClient
{
    public partial class Mainform : Form
    {

        private void RefreshWordsGridView(string start_address, int count, int[] data)
        {
            if (count > 0)
            {

                string device_letter = _mcProtocolTcpClient.GetDeviceType(start_address).ToString();
                
                // conver the start address number into HEX first
                int idx = Convert.ToInt32(start_address.Substring(device_letter.Length), 16);
                string address_num_format = "X4";

                // if the device is NOT HEX device, convert it into DECIMAL again
                if (!_mcProtocolTcpClient.IsHexDevice(_mcProtocolTcpClient.GetDeviceType(start_address)))
                {
                    idx = Convert.ToInt32(start_address.Substring(device_letter.Length), 10);
                    address_num_format = "D6";
                }

                //
                Dictionary<string, ushort> list = new Dictionary<string, ushort>();

                for (int i = 0; i < count; i++)
                {
                    int the_address = idx + i;
                    string address = $"{device_letter}" + the_address.ToString(address_num_format);
                    list.Add(address, (ushort)data[i]);
                }

                wordListGrid1.SetWords(list);
            }
        }

        private void wordListGrid1_SelectionChanged(object sender, EventArgs e)
        {
            if (wordListGrid1.SelectedRows.Count > 0)
            {
                int idx = wordListGrid1.SelectedRows[0].Index;
                tbTargetAddress.Text = wordListGrid1.Rows[idx].Cells[0].Value.ToString().Trim();
            }
        }

    }
}
