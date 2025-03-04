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

            if (string.IsNullOrEmpty(tbValueString.Text))
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

        #region [Float/Double]

        private void writeWords(PLCWordMemory memory, int start, byte[] bytes)
        {
            ushort[] values = new ushort[bytes.Length/2];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = BitConverter.ToUInt16(bytes, i * 2);
            }
            memory.Write(start, values.Length, values);
        }


        private void btnWriteFloat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTargetAddress.Text.Trim()))
            {
                MessageBox.Show("Select Target Memory Address", "Warning");
                return;
            }

            if (string.IsNullOrEmpty(tbValueFloat.Text.Trim()))
            {
                MessageBox.Show("Input Float To Write", "Warning");
                tbValueString.Focus();
                return;
            }

            try
            {
                float float_4bytes = float.Parse(tbValueFloat.Text.Trim());
                byte[] bytes = BitConverter.GetBytes(float_4bytes);
                writeWords(_currentWordMemory, _start_address, bytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception!");
            }
        }

        private void btnReadFloat_Click(object sender, EventArgs e)
        {
            _currentWordMemory.Read(_start_address, 2, out ushort[] words);

            byte[] bytes = new byte[words.Length * 2];
            for (int i = 0; i < words.Length; i++)
            {
                bytes[i * 2] = (byte)words[i];
                bytes[i * 2 + 1] = (byte)(words[i] >> 8);
            }

            tbValueFloat.Text = BitConverter.ToSingle(bytes, 0).ToString();
        }

        private void btnWriteDouble_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTargetAddress.Text.Trim()))
            {
                MessageBox.Show("Select Target Memory Address", "Warning");
                return;
            }

            if (string.IsNullOrEmpty(tbValueDouble.Text.Trim()))
            {
                MessageBox.Show("Input Float To Write", "Warning");
                tbValueString.Focus();
                return;
            }

            try
            {
                double double_8bytes = double.Parse(tbValueDouble.Text.Trim());
                byte[] bytes = BitConverter.GetBytes(double_8bytes);
                writeWords(_currentWordMemory, _start_address, bytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception!");
            }
        }

        private void btnReadDouble_Click(object sender, EventArgs e)
        {
            _currentWordMemory.Read(_start_address, 4, out ushort[] words);

            byte[] bytes = new byte[words.Length * 2];
            for (int i = 0; i < words.Length; i++)
            {
                bytes[i * 2] = (byte)words[i];
                bytes[i * 2 + 1] = (byte)(words[i] >> 8);
            }

            tbValueDouble.Text = BitConverter.ToDouble(bytes, 0).ToString();
        }


        #endregion
    }
}
