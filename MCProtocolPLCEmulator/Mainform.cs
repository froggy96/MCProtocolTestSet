using System;
using System.Windows.Forms;

namespace MCProtocolPLCEmulator
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

            //
            InitPLCWordMemories();

            //
            btnLaunchTcpServer_Click(this, null);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            //
            splitContainer1.SplitterDistance = 930;
        }
    }
}
