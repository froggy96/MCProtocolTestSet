namespace MCProtocolPLCEmulator
{
    partial class Mainform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.wordListGrid1 = new MCProtocolPLCEmulator.WordListGrid(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.tbWordCount = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSetStartAddress = new System.Windows.Forms.Button();
            this.tbStartAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDevice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnWriteString = new System.Windows.Forms.Button();
            this.btnWriteDecimal = new System.Windows.Forms.Button();
            this.tbValueString = new System.Windows.Forms.TextBox();
            this.tbValueDecimal = new System.Windows.Forms.TextBox();
            this.btnWriteHex = new System.Windows.Forms.Button();
            this.tbValueHex = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbTargetAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbConnectionLog = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLaunchTcpServer = new System.Windows.Forms.Button();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.tbServerAddress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbClientConnectionStatus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnWriteFloat = new System.Windows.Forms.Button();
            this.tbValueFloat = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnWriteDouble = new System.Windows.Forms.Button();
            this.tbValueDouble = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnReadFloat = new System.Windows.Forms.Button();
            this.btnReadDouble = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wordListGrid1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.wordListGrid1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1MinSize = 900;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1584, 761);
            this.splitContainer1.SplitterDistance = 930;
            this.splitContainer1.TabIndex = 1;
            // 
            // wordListGrid1
            // 
            this.wordListGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.wordListGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wordListGrid1.Location = new System.Drawing.Point(0, 48);
            this.wordListGrid1.Name = "wordListGrid1";
            this.wordListGrid1.RowTemplate.Height = 23;
            this.wordListGrid1.Size = new System.Drawing.Size(926, 709);
            this.wordListGrid1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.tbWordCount);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnSetStartAddress);
            this.panel1.Controls.Add(this.tbStartAddress);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbDevice);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(926, 48);
            this.panel1.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(401, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 15);
            this.label9.TabIndex = 7;
            this.label9.Text = "Count:";
            // 
            // tbWordCount
            // 
            this.tbWordCount.Location = new System.Drawing.Point(450, 16);
            this.tbWordCount.MaxLength = 6;
            this.tbWordCount.Name = "tbWordCount";
            this.tbWordCount.Size = new System.Drawing.Size(50, 23);
            this.tbWordCount.TabIndex = 6;
            this.tbWordCount.Text = "128";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(757, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(123, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSetStartAddress
            // 
            this.btnSetStartAddress.Location = new System.Drawing.Point(506, 15);
            this.btnSetStartAddress.Name = "btnSetStartAddress";
            this.btnSetStartAddress.Size = new System.Drawing.Size(58, 23);
            this.btnSetStartAddress.TabIndex = 4;
            this.btnSetStartAddress.Text = "Set";
            this.btnSetStartAddress.UseVisualStyleBackColor = true;
            this.btnSetStartAddress.Click += new System.EventHandler(this.btnSetStartAddress_Click);
            // 
            // tbStartAddress
            // 
            this.tbStartAddress.Location = new System.Drawing.Point(295, 16);
            this.tbStartAddress.MaxLength = 10;
            this.tbStartAddress.Name = "tbStartAddress";
            this.tbStartAddress.Size = new System.Drawing.Size(100, 23);
            this.tbStartAddress.TabIndex = 3;
            this.tbStartAddress.Text = "0000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start Address:";
            // 
            // cbDevice
            // 
            this.cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevice.FormattingEnabled = true;
            this.cbDevice.Items.AddRange(new object[] {
            "D0000",
            "W0000"});
            this.cbDevice.Location = new System.Drawing.Point(95, 16);
            this.cbDevice.Name = "cbDevice";
            this.cbDevice.Size = new System.Drawing.Size(93, 23);
            this.cbDevice.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Device:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tbConnectionLog);
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Size = new System.Drawing.Size(650, 761);
            this.splitContainer2.SplitterDistance = 299;
            this.splitContainer2.SplitterIncrement = 8;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnReadDouble);
            this.groupBox1.Controls.Add(this.btnReadFloat);
            this.groupBox1.Controls.Add(this.btnWriteDouble);
            this.groupBox1.Controls.Add(this.tbValueDouble);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.btnWriteFloat);
            this.groupBox1.Controls.Add(this.tbValueFloat);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.btnWriteString);
            this.groupBox1.Controls.Add(this.btnWriteDecimal);
            this.groupBox1.Controls.Add(this.tbValueString);
            this.groupBox1.Controls.Add(this.tbValueDecimal);
            this.groupBox1.Controls.Add(this.btnWriteHex);
            this.groupBox1.Controls.Add(this.tbValueHex);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbTargetAddress);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(646, 295);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Write Data";
            // 
            // btnWriteString
            // 
            this.btnWriteString.Location = new System.Drawing.Point(384, 147);
            this.btnWriteString.Name = "btnWriteString";
            this.btnWriteString.Size = new System.Drawing.Size(96, 29);
            this.btnWriteString.TabIndex = 14;
            this.btnWriteString.Text = "Write";
            this.btnWriteString.UseVisualStyleBackColor = true;
            this.btnWriteString.Click += new System.EventHandler(this.btnWriteString_Click);
            // 
            // btnWriteDecimal
            // 
            this.btnWriteDecimal.Location = new System.Drawing.Point(384, 116);
            this.btnWriteDecimal.Name = "btnWriteDecimal";
            this.btnWriteDecimal.Size = new System.Drawing.Size(96, 29);
            this.btnWriteDecimal.TabIndex = 13;
            this.btnWriteDecimal.Text = "Write";
            this.btnWriteDecimal.UseVisualStyleBackColor = true;
            this.btnWriteDecimal.Click += new System.EventHandler(this.btnWriteDecimal_Click);
            // 
            // tbValueString
            // 
            this.tbValueString.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValueString.Location = new System.Drawing.Point(203, 147);
            this.tbValueString.MaxLength = 32;
            this.tbValueString.Name = "tbValueString";
            this.tbValueString.Size = new System.Drawing.Size(175, 29);
            this.tbValueString.TabIndex = 12;
            // 
            // tbValueDecimal
            // 
            this.tbValueDecimal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValueDecimal.Location = new System.Drawing.Point(203, 116);
            this.tbValueDecimal.MaxLength = 32;
            this.tbValueDecimal.Name = "tbValueDecimal";
            this.tbValueDecimal.Size = new System.Drawing.Size(175, 29);
            this.tbValueDecimal.TabIndex = 11;
            // 
            // btnWriteHex
            // 
            this.btnWriteHex.Location = new System.Drawing.Point(384, 85);
            this.btnWriteHex.Name = "btnWriteHex";
            this.btnWriteHex.Size = new System.Drawing.Size(96, 29);
            this.btnWriteHex.TabIndex = 6;
            this.btnWriteHex.Text = "Write";
            this.btnWriteHex.UseVisualStyleBackColor = true;
            this.btnWriteHex.Click += new System.EventHandler(this.btnWriteHex_Click);
            // 
            // tbValueHex
            // 
            this.tbValueHex.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValueHex.Location = new System.Drawing.Point(203, 85);
            this.tbValueHex.MaxLength = 32;
            this.tbValueHex.Name = "tbValueHex";
            this.tbValueHex.Size = new System.Drawing.Size(175, 29);
            this.tbValueHex.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(70, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 30);
            this.label6.TabIndex = 9;
            this.label6.Text = "Value String:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(48, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 30);
            this.label5.TabIndex = 8;
            this.label5.Text = "Value Decimal:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(87, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 30);
            this.label4.TabIndex = 7;
            this.label4.Text = "Value Hex:";
            // 
            // tbTargetAddress
            // 
            this.tbTargetAddress.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTargetAddress.Location = new System.Drawing.Point(203, 38);
            this.tbTargetAddress.Name = "tbTargetAddress";
            this.tbTargetAddress.ReadOnly = true;
            this.tbTargetAddress.Size = new System.Drawing.Size(175, 29);
            this.tbTargetAddress.TabIndex = 6;
            this.tbTargetAddress.TextChanged += new System.EventHandler(this.tbTargetAddress_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 30);
            this.label3.TabIndex = 6;
            this.label3.Text = "Target Address:";
            // 
            // tbConnectionLog
            // 
            this.tbConnectionLog.BackColor = System.Drawing.Color.Black;
            this.tbConnectionLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbConnectionLog.ForeColor = System.Drawing.Color.LightGray;
            this.tbConnectionLog.Location = new System.Drawing.Point(0, 102);
            this.tbConnectionLog.MaxLength = 2048;
            this.tbConnectionLog.Multiline = true;
            this.tbConnectionLog.Name = "tbConnectionLog";
            this.tbConnectionLog.Size = new System.Drawing.Size(646, 352);
            this.tbConnectionLog.TabIndex = 7;
            this.tbConnectionLog.Text = "Communication Logs:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnLaunchTcpServer);
            this.panel2.Controls.Add(this.tbServerPort);
            this.panel2.Controls.Add(this.tbServerAddress);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.tbClientConnectionStatus);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(646, 102);
            this.panel2.TabIndex = 0;
            // 
            // btnLaunchTcpServer
            // 
            this.btnLaunchTcpServer.Location = new System.Drawing.Point(299, 13);
            this.btnLaunchTcpServer.Name = "btnLaunchTcpServer";
            this.btnLaunchTcpServer.Size = new System.Drawing.Size(79, 24);
            this.btnLaunchTcpServer.TabIndex = 6;
            this.btnLaunchTcpServer.Text = "Launch";
            this.btnLaunchTcpServer.UseVisualStyleBackColor = true;
            this.btnLaunchTcpServer.Click += new System.EventHandler(this.btnLaunchTcpServer_Click);
            // 
            // tbServerPort
            // 
            this.tbServerPort.Location = new System.Drawing.Point(247, 14);
            this.tbServerPort.MaxLength = 6;
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(46, 23);
            this.tbServerPort.TabIndex = 9;
            this.tbServerPort.Text = "8000";
            // 
            // tbServerAddress
            // 
            this.tbServerAddress.Location = new System.Drawing.Point(125, 14);
            this.tbServerAddress.MaxLength = 2048;
            this.tbServerAddress.Name = "tbServerAddress";
            this.tbServerAddress.Size = new System.Drawing.Size(116, 23);
            this.tbServerAddress.TabIndex = 8;
            this.tbServerAddress.Text = "127.0.0.1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(37, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 15);
            this.label8.TabIndex = 7;
            this.label8.Text = "Server Setting:";
            // 
            // tbClientConnectionStatus
            // 
            this.tbClientConnectionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbClientConnectionStatus.BackColor = System.Drawing.Color.DarkGray;
            this.tbClientConnectionStatus.Location = new System.Drawing.Point(125, 61);
            this.tbClientConnectionStatus.MaxLength = 2048;
            this.tbClientConnectionStatus.Name = "tbClientConnectionStatus";
            this.tbClientConnectionStatus.Size = new System.Drawing.Size(511, 23);
            this.tbClientConnectionStatus.TabIndex = 6;
            this.tbClientConnectionStatus.Text = "No Clients";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 15);
            this.label7.TabIndex = 6;
            this.label7.Text = "Client Connection:";
            // 
            // btnWriteFloat
            // 
            this.btnWriteFloat.Location = new System.Drawing.Point(384, 178);
            this.btnWriteFloat.Name = "btnWriteFloat";
            this.btnWriteFloat.Size = new System.Drawing.Size(96, 29);
            this.btnWriteFloat.TabIndex = 17;
            this.btnWriteFloat.Text = "Write";
            this.btnWriteFloat.UseVisualStyleBackColor = true;
            this.btnWriteFloat.Click += new System.EventHandler(this.btnWriteFloat_Click);
            // 
            // tbValueFloat
            // 
            this.tbValueFloat.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValueFloat.Location = new System.Drawing.Point(203, 178);
            this.tbValueFloat.MaxLength = 32;
            this.tbValueFloat.Name = "tbValueFloat";
            this.tbValueFloat.Size = new System.Drawing.Size(175, 29);
            this.tbValueFloat.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(55, 178);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(142, 30);
            this.label10.TabIndex = 15;
            this.label10.Text = "Float (32 Bits):";
            // 
            // btnWriteDouble
            // 
            this.btnWriteDouble.Location = new System.Drawing.Point(384, 208);
            this.btnWriteDouble.Name = "btnWriteDouble";
            this.btnWriteDouble.Size = new System.Drawing.Size(96, 29);
            this.btnWriteDouble.TabIndex = 20;
            this.btnWriteDouble.Text = "Write";
            this.btnWriteDouble.UseVisualStyleBackColor = true;
            this.btnWriteDouble.Click += new System.EventHandler(this.btnWriteDouble_Click);
            // 
            // tbValueDouble
            // 
            this.tbValueDouble.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValueDouble.Location = new System.Drawing.Point(203, 208);
            this.tbValueDouble.MaxLength = 32;
            this.tbValueDouble.Name = "tbValueDouble";
            this.tbValueDouble.Size = new System.Drawing.Size(175, 29);
            this.tbValueDouble.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(33, 208);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(164, 30);
            this.label11.TabIndex = 18;
            this.label11.Text = "Double (64 Bits):";
            // 
            // btnReadFloat
            // 
            this.btnReadFloat.Location = new System.Drawing.Point(486, 179);
            this.btnReadFloat.Name = "btnReadFloat";
            this.btnReadFloat.Size = new System.Drawing.Size(96, 29);
            this.btnReadFloat.TabIndex = 21;
            this.btnReadFloat.Text = "Read";
            this.btnReadFloat.UseVisualStyleBackColor = true;
            this.btnReadFloat.Click += new System.EventHandler(this.btnReadFloat_Click);
            // 
            // btnReadDouble
            // 
            this.btnReadDouble.Location = new System.Drawing.Point(486, 209);
            this.btnReadDouble.Name = "btnReadDouble";
            this.btnReadDouble.Size = new System.Drawing.Size(96, 29);
            this.btnReadDouble.TabIndex = 22;
            this.btnReadDouble.Text = "Read";
            this.btnReadDouble.UseVisualStyleBackColor = true;
            this.btnReadDouble.Click += new System.EventHandler(this.btnReadDouble_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Mainform";
            this.Text = "PLC Emulator For MC Protocol MC3E";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wordListGrid1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private WordListGrid wordListGrid1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSetStartAddress;
        private System.Windows.Forms.TextBox tbStartAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnWriteHex;
        private System.Windows.Forms.TextBox tbValueHex;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbTargetAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnWriteString;
        private System.Windows.Forms.Button btnWriteDecimal;
        private System.Windows.Forms.TextBox tbValueString;
        private System.Windows.Forms.TextBox tbValueDecimal;
        private System.Windows.Forms.TextBox tbConnectionLog;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbClientConnectionStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbServerAddress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnLaunchTcpServer;
        private System.Windows.Forms.TextBox tbServerPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbWordCount;
        private System.Windows.Forms.Button btnReadDouble;
        private System.Windows.Forms.Button btnReadFloat;
        private System.Windows.Forms.Button btnWriteDouble;
        private System.Windows.Forms.TextBox tbValueDouble;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnWriteFloat;
        private System.Windows.Forms.TextBox tbValueFloat;
        private System.Windows.Forms.Label label10;
    }
}

