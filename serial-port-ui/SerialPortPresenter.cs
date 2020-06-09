using System;
using System.IO.Ports;

namespace SerialPortChat
{
    public class SerialPortPresenter
    {
        private readonly IClientUserInterface _view;
        private System.IO.Ports.SerialPort serialPort;

        public SerialPortPresenter(IClientUserInterface mainView)
        {
            _view = mainView;

            //Register UI Events
            mainView.ConnectSerialPort += ConnectSerialPort;
            mainView.DisconnectSerialPort += DisconnectSerialPort;
            mainView.SendMessage += OnDataSent;
        }

        public void AddLogMessage(string message)
        {
            string dateTimeNow = DateTime.Now.ToString();
            _view.UpdateLog($"[{dateTimeNow}]: {message}");
        }

        private void ConnectSerialPort(object sender, ConnectEventArgs e)
        {
            serialPort = new SerialPort()
            {
                PortName = e.Port,
                BaudRate = e.BaudRate,
                Parity = e.Parity,
                DataBits = e.Databits,
                StopBits = e.Stopbits
            };

            try
            {
                serialPort.Open();
                serialPort.DataReceived += OnDataReceived;
                AddLogMessage("Connected...");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DisconnectSerialPort(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                    AddLogMessage("Disconnected..");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void OnDataReceived(object sender, EventArgs e)
        {
            AddLogMessage($"Received: {serialPort.ReadExisting()}");
        }

        private void OnDataSent(object sender, string e)
        {
            try
            {
                serialPort.Write(e);
                AddLogMessage($"Sent: {e}");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}