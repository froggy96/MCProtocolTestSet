using System;
using System.Windows.Forms;

namespace MCProtocolClient
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        private void MoveWindowToActiveScreen()
        {
            Screen screen = Screen.FromPoint(Cursor.Position);
            this.Location = screen.Bounds.Location;
            this.Location = new System.Drawing.Point(Location.X + screen.Bounds.Width / 8, Location.Y + screen.Bounds.Height / 8);
            this.Size = new System.Drawing.Size(screen.Bounds.Width * 3 / 4, screen.Bounds.Height * 3 / 4);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //
            MoveWindowToActiveScreen();

            //
            wordListGrid1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            wordListGrid1.SelectionChanged += wordListGrid1_SelectionChanged;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            //
            splitContainer1.SplitterDistance = 930;
        }



        private void btnOpenConnection_Click(object sender, EventArgs e)
        {
            OpenConnection(tbServerAddress.Text, tbServerPort.Text);
        }

        private void btnCloseConnection_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            int count = int.Parse(tbWordCount.Text);
            int[] words = new int[count];
            if (ReadBlock(tbStartAddress.Text, count, words))
            {
                RefreshWordsGridView(tbStartAddress.Text, count, words);
            }
        }

        private void btnWriteHex_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTargetAddress.Text.Trim()))
            {
                MessageBox.Show("Select Target Memory Address", "Warning");
                return;
            }

            if (string.IsNullOrEmpty(tbValueHex.Text.Trim()))
            {
                MessageBox.Show("Input Value To Write", "Warning");
                tbValueHex.Focus();
                return;
            }

            int[] data = new int[1];
            data[0] = Convert.ToInt32(tbValueHex.Text, 16);
            WriteBlock(tbTargetAddress.Text, data.Length, data);
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

            int[] data = new int[1];
            data[0] = Convert.ToInt32(tbValueDecimal.Text, 10);
            WriteBlock(tbTargetAddress.Text, data.Length, data);
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

            string str = tbValueString.Text;
            int word_count_for_string = str.Length / 2 + str.Length % 2;
            int[] words = new int[word_count_for_string];

            int w = 0;
            for (int i = 0; i < str.Length; i += 2)
            {
                ushort ch1 = (ushort)str[i];
                ushort ch2 = i + 1 < str.Length ? (ushort)str[i + 1] : (ushort)0x00;
                words[w] = (ushort)(ch1 << 8 | ch2);
                w++;
            }

            WriteBlock(tbTargetAddress.Text, words.Length, words);
        }

        private void btnWriteFloat_Click(object sender, EventArgs e)
        {
            // checking user input -- skipped...

            //
            float float_4bytes = float.Parse(tbValueFloat.Text.Trim());
            byte[] bytes = BitConverter.GetBytes(float_4bytes);

            int[] words = new int[bytes.Length / 2];
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = BitConverter.ToUInt16(bytes, i * 2);
            }
            WriteBlock(tbTargetAddress.Text, words.Length, words);
        }

        private void btnReadFloat_Click(object sender, EventArgs e)
        {
            int[] words = new int[2];
            ReadBlock(tbTargetAddress.Text, words.Length, words);

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
            // checking user input -- skipped...

            //
            double double_8bytes = double.Parse(tbValueDouble.Text.Trim());
            byte[] bytes = BitConverter.GetBytes(double_8bytes);

            int[] words = new int[bytes.Length / 2];
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = BitConverter.ToUInt16(bytes, i * 2);
            }
            WriteBlock(tbTargetAddress.Text, words.Length, words);
        }

        private void btnReadDouble_Click(object sender, EventArgs e)
        {
            int[] words = new int[4];
            ReadBlock(tbTargetAddress.Text, words.Length, words);

            byte[] bytes = new byte[words.Length * 2];
            for (int i = 0; i < words.Length; i++)
            {
                bytes[i * 2] = (byte)words[i];
                bytes[i * 2 + 1] = (byte)(words[i] >> 8);
            }

            tbValueDouble.Text = BitConverter.ToDouble(bytes, 0).ToString();
        }
    }
}
