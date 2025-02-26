using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MCProtocolPLCEmulator
{
    public partial class WordListGrid : DataGridView
    {
        public WordListGrid()
        {
            InitializeComponent(); // 이 생성자는 Design Mode 에서 불리고...
        }

        public WordListGrid(IContainer container)
        {
            container.Add(this);

            InitializeComponent(); // 이 생성자는 Running Time 에 불린다...
            Init();
        }

        private void Init()
        {
            //
            DoubleBuffered = true;

            //
            EnableHeadersVisualStyles = false;
            ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            //
            RowHeadersWidth = 24;
            ColumnHeadersHeight = 24;

            // Address | Word Value(Hex) | Word Value (Dec) | 16 bits | ASCII Value
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeColumns = false;
            this.AllowUserToResizeRows = false;
            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ReadOnly = true;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.ScrollBars = ScrollBars.Both;

            Rows.Clear();
            Columns.Clear();

            Columns.Add("Address", "Address");
            Columns.Add("Hex", "Hex");
            Columns.Add("Dec", "Dec");

            for(int i = 15; i>=0; i--)
            {
                string s = i.ToString("X1");
                Columns.Add(s, s);
            }

            Columns.Add("ASCII", "ASCII");

            for (int i = 0; i < 128; i++)
            {
                object[] values = new object[Columns.Count];

                values[0] = $"D{i:D6}";
                values[1] = $"{i:X4}";
                values[2] = $"{i:D6}";

                values[3] = (i % 2);
                values[4] = (i / 2) % 2;
                values[5] = (i % 2);
                values[6] = (i / 2) % 2;
                values[7] = (i % 2);
                values[8] = (i / 2) % 2;
                values[9] = (i % 2);
                values[10] = (i / 2) % 2;

                values[11] = (i % 2);
                values[12] = (i / 2) % 2;
                values[13] = (i % 2);
                values[14] = (i / 2) % 2;
                values[15] = (i % 2);
                values[16] = (i / 2) % 2;
                values[17] = (i % 2);
                values[18] = (i / 2) % 2;

                values[19] = Convert.ToChar(i+30);

                Rows.Add(values);
            }
        }


        public void SetWords(Dictionary<string, ushort> words)
        {
            int row_index = 0;
            Rows.Clear();

            foreach (KeyValuePair<string, ushort> word in words)
            {
                SetWord(row_index++, word.Key, word.Value);
            }
        }

        public void SetWord(int row_index, string address, ushort value)
        {
            if (row_index < Rows.Count)
            {
                Rows[row_index].SetValues(getRowValueObjects(address, value));
            }
            else
            {
                Rows.Add(getRowValueObjects(address, value));
            }
        }

        private object[] getRowValueObjects(string address, ushort value)
        {
            object[] values = new object[Columns.Count];

            values[0] = address;
            values[1] = $"{value:X4}";
            values[2] = $"{value:D6}";

            for (int i = 0; i <= 15; i++)
            {
                var bit = value >> (15 - i) & 0x0001;
                values[i + 3] = bit;
            }

            var hiByte = (byte)(value >> 8 & 0x00FF);
            var loByte = (byte)(value & 0x00FF);

            values[19] = Convert.ToChar(hiByte).ToString() + Convert.ToChar(loByte).ToString();

            return values;
        }

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            base.OnCellFormatting(e);

            //
            if (e.ColumnIndex >= 3 && e.ColumnIndex <= 18) // bit value cells
            {
                if (e.Value != null && e.Value.ToString() == "1")
                {
                    e.CellStyle.BackColor = Color.Blue;
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightGray;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

    }
}
