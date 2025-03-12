using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MCProtocolPLCEmulator
{
    public partial class Mainform : Form
    {

        private List<PLCWordMemory> _plcWordMemories = new List<PLCWordMemory>();
        private PLCWordMemory _currentWordMemory = null;


        private void InitPLCWordMemories()
        {
            _plcWordMemories.Clear();
            _plcWordMemories.Add(new PLCWordMemory('D'));
            _plcWordMemories.Add(new PLCWordMemory('W'));

            //
            for (int i = 0; i < _plcWordMemories.Count; i++)
            {
                _plcWordMemories[i].MemoryWritten += Mainform_MemoryWritten;
            }
            //

            //
            cbDevice.SelectedIndex = 0;
            cbDevice.SelectionChangeCommitted += CbDevice_SelectionChangeCommitted;

            //
            wordListGrid1.SelectionChanged += wordListGrid1_SelectionChanged;
            RefreshWordsGridView();
        }

        private void Mainform_MemoryWritten(object sender, EventArgs e)
        {
            // 여기서 하면 메모리 Leak 이 많이 나는 것 같은데...
            //RefreshWordsGridView();
        }

        private void CbDevice_SelectionChangeCommitted(object sender, EventArgs e)
        {
            tbStartAddress.Text = cbDevice.Text.Substring(1);
        }

        private void btnSetStartAddress_Click(object sender, EventArgs e)
        {
            RefreshWordsGridView();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshWordsGridView();
        }

        private void RefreshWordsGridView()
        {
            if (_plcWordMemories.Find(x => x.DeviceLetter == cbDevice.Text[0]) is PLCWordMemory words)
            {
                _currentWordMemory = words;

                string address_num_format = string.Empty;
                int start_address = 0x00;
                
                if (words.DeviceLetter == 'W')
                {
                    address_num_format = "X4";
                    start_address = Convert.ToInt32(tbStartAddress.Text, 16);
                }
                else
                {
                    address_num_format = "D6";
                    start_address = Convert.ToInt32(tbStartAddress.Text, 10);
                }

                int count = Convert.ToInt32(tbWordCount.Text);

                //
                if (start_address + count > words.Words.Length)
                {
                    MessageBox.Show($"Set Address Range: 0..{words.Words.Length - 1}", "Invalid Addressing Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //



                //
                Dictionary<string,ushort> list = new Dictionary<string,ushort>();

                for (int i = 0; i < count; i++)
                {
                    int the_address = start_address + i;
                    string address = $"{words.DeviceLetter}" + the_address.ToString(address_num_format);
                    list.Add(address, (ushort)words.Words[the_address]);
                }

                wordListGrid1.BeginInvoke(new MethodInvoker(() =>
                    {
                        wordListGrid1.SetWords(list);
                    }));
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
