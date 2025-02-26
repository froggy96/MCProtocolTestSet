using System;
using System.Windows.Forms;

namespace MCProtocolPLCEmulator
{
    public partial class Mainform : Form
    {

        private int _start_address = 0x00;

        private void tbTargetAddress_TextChanged(object sender, EventArgs e)
        {
            if (tbTargetAddress.Text[0] == 'W')
            {
                _start_address = Convert.ToInt32(tbTargetAddress.Text.Substring(1), 16);
            }
            else
            {
                _start_address = Convert.ToInt32(tbTargetAddress.Text.Substring(1), 10);
            }
        }

        private void writeWord(PLCWordMemory memory, int start, ushort value)
        {
            ushort[] words = new ushort[1];
            words[0] = value;
            memory.Write(start, 1, words);
        }

        private void btnWriteHex_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbTargetAddress.Text.Trim()))
            {
                MessageBox.Show("Select Target Memory Address", "Warning");
                return;
            }

            if(string.IsNullOrEmpty(tbValueHex.Text.Trim()))
            {
                MessageBox.Show("Input Value To Write", "Warning");
                tbValueHex.Focus();
                return;
            }

            try
            {
                ushort value = Convert.ToUInt16(tbValueHex.Text, 16);
                writeWord(_currentWordMemory, _start_address, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception!");
            }
        }

        private void btnWriteDecimal_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTargetAddress.Text.Trim()))
            {
                MessageBox.Show("Select Target Memory Address", "Warning");
                return;
            }

            if (string.IsNullOrEmpty(tbValueDecimal.Text.Trim()))
            {
                MessageBox.Show("Input Value To Write", "Warning");
                tbValueDecimal.Focus();
                return;
            }

            try
            {
                ushort value = Convert.ToUInt16(tbValueDecimal.Text, 10);
                writeWord(_currentWordMemory, _start_address, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception!");
            }
        }

        private void btnWriteString_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTargetAddress.Text.Trim()))
            {
                MessageBox.Show("Select Target Memory Address", "Warning");
                return;
            }

            if (string.IsNullOrEmpty(tbValueString.Text.Trim()))
            {
                MessageBox.Show("Input String To Write", "Warning");
                tbValueString.Focus();
                return;
            }

            try
            {
                _currentWordMemory.WriteString(_start_address, tbValueString.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception!");
            }
        }
    }
}
