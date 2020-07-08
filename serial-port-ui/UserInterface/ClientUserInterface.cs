using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace SerialPortChat
{
    public class ClientUserInterface : Form, IClientUserInterface
    {
        public ClientUserInterface()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }

        public event EventHandler<ConnectEventArgs> ConnectSerialPort;

        public event EventHandler DisconnectSerialPort;

        public event EventHandler<string> SendMessage;

        #region Form Components

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox logRichTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button sendMessageButton;
        private System.Windows.Forms.CheckBox connectToggleButton;
        private TableLayoutPanel tableLayoutPanel2;
        private ComboBox stopbitsComboBox;
        private ComboBox databitsComboBox;
        private ComboBox parityComboBox;
        private ComboBox baudComboBox;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComboBox portComboBox;
        private System.Windows.Forms.RichTextBox messageRichTextBox;

        #endregion Form Components

        private void InitializeComboBoxes()
        {
            List<string> baudRateList = new List<string>() { "9600" };
            List<string> databitsList = new List<string>() { "4", "5", "6", "7", "8" };
            List<string> stopbitsList = new List<string>() { "None", "One", "OnePointFive", "Two" };
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                portComboBox.Items.Add(s);
            }
            foreach (string s in baudRateList)
            {
                baudComboBox.Items.Add(s);
            }
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                parityComboBox.Items.Add(s);
            }
            foreach (string s in databitsList)
            {
                databitsComboBox.Items.Add(s);
            }
            foreach (string s in stopbitsList)
            {
                stopbitsComboBox.Items.Add(s);
            }
        }

        private void InvokeUI(Action action)
        {
            this.Invoke(action);
        }

        public void UpdateLog(string message)
        {
            InvokeUI(() =>
            {
                logRichTextBox.Text += $"{ message } \n";
                logRichTextBox.Refresh();
            });
        }

        private void connectToggleButton_CheckedChanged(object sender, EventArgs e)
        {
            if (connectToggleButton.Checked)
            {
                try
                {
                    ConnectSerialPort?.Invoke(sender, new ConnectEventArgs
                    {
                        Port = this.portComboBox.Text,
                        BaudRate = Convert.ToInt32(baudComboBox.Text),
                        Parity = (Parity)Enum.Parse(typeof(Parity), parityComboBox.Text),
                        Databits = Convert.ToInt32(databitsComboBox.Text),
                        Stopbits = (StopBits)Enum.Parse(typeof(StopBits), stopbitsComboBox.Text)
                    });

                    InvokeUI(() =>
                    {
                        connectToggleButton.Text = "Stop";
                        messageRichTextBox.Enabled = true;
                        sendMessageButton.Enabled = true;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    DisconnectSerialPort?.Invoke(sender, e);
                    InvokeUI(() =>
                    {
                        connectToggleButton.Text = "Connect";
                        messageRichTextBox.Enabled = false;
                        sendMessageButton.Enabled = false;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            try
            {
                SendMessage?.Invoke(sender, messageRichTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                messageRichTextBox.Clear();
            }
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.logRichTextBox = new RichTextBox();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.sendMessageButton = new Button();
            this.connectToggleButton = new CheckBox();
            this.messageRichTextBox = new RichTextBox();
            this.portComboBox = new ComboBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.baudComboBox = new ComboBox();
            this.parityComboBox = new ComboBox();
            this.databitsComboBox = new ComboBox();
            this.stopbitsComboBox = new ComboBox();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.logRichTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new Padding(3, 3, 20, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 18.85246F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 81.14754F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(558, 386);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // logRichTextBox
            //
            this.logRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.logRichTextBox.Dock = DockStyle.Fill;
            this.logRichTextBox.Location = new System.Drawing.Point(30, 67);
            this.logRichTextBox.Margin = new Padding(30, 3, 30, 3);
            this.logRichTextBox.Name = "logRichTextBox";
            this.logRichTextBox.ReadOnly = true;
            this.logRichTextBox.Size = new System.Drawing.Size(498, 269);
            this.logRichTextBox.TabIndex = 1;
            this.logRichTextBox.Text = "";
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.sendMessageButton, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.connectToggleButton, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.messageRichTextBox, 1, 0);
            this.tableLayoutPanel3.Dock = DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(30, 342);
            this.tableLayoutPanel3.Margin = new Padding(30, 3, 30, 10);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(498, 34);
            this.tableLayoutPanel3.TabIndex = 2;
            //
            // sendMessageButton
            //
            this.sendMessageButton.Dock = DockStyle.Fill;
            this.sendMessageButton.Enabled = false;
            this.sendMessageButton.Location = new System.Drawing.Point(400, 3);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(95, 28);
            this.sendMessageButton.TabIndex = 2;
            this.sendMessageButton.Text = "Send";
            this.sendMessageButton.UseVisualStyleBackColor = true;
            this.sendMessageButton.Click += sendMessageButton_Click;
            //
            // connectToggleButton
            //
            this.connectToggleButton.Appearance = Appearance.Button;
            this.connectToggleButton.AutoSize = true;
            this.connectToggleButton.Dock = DockStyle.Fill;
            this.connectToggleButton.Location = new System.Drawing.Point(3, 3);
            this.connectToggleButton.Name = "connectToggleButton";
            this.connectToggleButton.Size = new System.Drawing.Size(93, 28);
            this.connectToggleButton.TabIndex = 3;
            this.connectToggleButton.Text = "Connect";
            this.connectToggleButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.connectToggleButton.UseVisualStyleBackColor = true;
            this.connectToggleButton.CheckedChanged += connectToggleButton_CheckedChanged;
            //
            // messageRichTextBox
            //
            this.messageRichTextBox.Dock = DockStyle.Fill;
            this.messageRichTextBox.Enabled = false;
            this.messageRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageRichTextBox.Location = new System.Drawing.Point(102, 3);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.Size = new System.Drawing.Size(292, 28);
            this.messageRichTextBox.TabIndex = 4;
            this.messageRichTextBox.Text = "";
            //
            // portComboBox
            //
            this.portComboBox.Dock = DockStyle.Fill;
            this.portComboBox.FormattingEnabled = true;
            this.portComboBox.Location = new System.Drawing.Point(3, 20);
            this.portComboBox.Margin = new Padding(3, 3, 20, 3);
            this.portComboBox.Name = "portComboBox";
            this.portComboBox.Size = new System.Drawing.Size(76, 21);
            this.portComboBox.TabIndex = 2;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(102, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Baud Rate";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(201, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Parity";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(300, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Data Bits";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(399, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Stop Bits";
            //
            // baudComboBox
            //
            this.baudComboBox.Dock = DockStyle.Fill;
            this.baudComboBox.FormattingEnabled = true;
            this.baudComboBox.Location = new System.Drawing.Point(102, 20);
            this.baudComboBox.Margin = new Padding(3, 3, 20, 3);
            this.baudComboBox.Name = "baudComboBox";
            this.baudComboBox.Size = new System.Drawing.Size(76, 21);
            this.baudComboBox.TabIndex = 7;
            //
            // parityComboBox
            //
            this.parityComboBox.Dock = DockStyle.Fill;
            this.parityComboBox.FormattingEnabled = true;
            this.parityComboBox.Location = new System.Drawing.Point(201, 20);
            this.parityComboBox.Margin = new Padding(3, 3, 20, 3);
            this.parityComboBox.Name = "parityComboBox";
            this.parityComboBox.Size = new System.Drawing.Size(76, 21);
            this.parityComboBox.TabIndex = 8;
            //
            // databitsComboBox
            //
            this.databitsComboBox.Dock = DockStyle.Fill;
            this.databitsComboBox.FormattingEnabled = true;
            this.databitsComboBox.Location = new System.Drawing.Point(300, 20);
            this.databitsComboBox.Margin = new Padding(3, 3, 20, 3);
            this.databitsComboBox.Name = "databitsComboBox";
            this.databitsComboBox.Size = new System.Drawing.Size(76, 21);
            this.databitsComboBox.TabIndex = 9;
            //
            // stopbitsComboBox
            //
            this.stopbitsComboBox.Dock = DockStyle.Fill;
            this.stopbitsComboBox.FormattingEnabled = true;
            this.stopbitsComboBox.Location = new System.Drawing.Point(399, 20);
            this.stopbitsComboBox.Margin = new Padding(3, 3, 20, 3);
            this.stopbitsComboBox.Name = "stopbitsComboBox";
            this.stopbitsComboBox.Size = new System.Drawing.Size(79, 21);
            this.stopbitsComboBox.TabIndex = 10;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.stopbitsComboBox, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.databitsComboBox, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.parityComboBox, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.baudComboBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label5, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.portComboBox, 0, 1);
            this.tableLayoutPanel2.Dock = DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(30, 15);
            this.tableLayoutPanel2.Margin = new Padding(30, 15, 30, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(498, 44);
            this.tableLayoutPanel2.TabIndex = 0;
            //
            // ClientUserInterface
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 386);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ClientUserInterface";
            this.Text = "Serial Port Chat";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    public class ConnectEventArgs : EventArgs
    {
        public String Port { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int Databits { get; set; }
        public StopBits Stopbits { get; set; }
    }
}