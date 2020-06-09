using System;

namespace SerialPortChat
{
    public interface IClientUserInterface
    {
        event EventHandler<ConnectEventArgs> ConnectSerialPort;

        event EventHandler DisconnectSerialPort;

        event EventHandler<string> SendMessage;

        void UpdateLog(string message);
    }
}