using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPortChat
{
    public partial class ClientUserInterface : Form, IClientUserInterface
    {
        public ClientUserInterface()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }

        public event EventHandler<ConnectEventArgs> ConnectSerialPort;

        public event EventHandler DisconnectSerialPort;

        public event EventHandler<string> SendMessage;

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