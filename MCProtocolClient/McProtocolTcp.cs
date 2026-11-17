using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace MCProtocol
{
    public class McProtocolTcp : McProtocolApp
    {
        // ====================================================================================
        // Constructor
        public McProtocolTcp() : this("", 0) { }

        public McProtocolTcp(string iHostName, int iPortNumber)
            : base(iHostName, iPortNumber)
        {
            _tcpClient = new TcpClient();
        }



        override protected void DoConnect()
        {
            if (IsConnected()) { return; }



            //
            StopConnectionCheckingTimer();



            if (_tcpClient == null) _tcpClient = new TcpClient();

            try
            {
                _tcpClient.Connect(HostName, PortNumber);
            }
            catch (SocketException e)
            {
                // connection failed
                Console.WriteLine(e.ToString());
                return;
            }



            //
            StartConnectionCheckingTimer();
        }

        override protected void DoDisconnect()
        {
            //
            _tcpClient.Close();

            //
            StopConnectionCheckingTimer();

            //
            ConnectionStatus = IsConnected();
        }

        override protected byte[] Execute(byte[] iCommand)
        {
            TcpClient connection = _tcpClient;
            NetworkStream stream = connection.GetStream();
            stream.ReadTimeout = 10_000;
            try
            {
                stream.Write(iCommand, 0, iCommand.Length);
                bool is4E = iCommand[0] == 0x54;
                var header = new byte[is4E ? 13 : 9];
                ReadExactly(stream, header, 0, 2);
                if (header[0] != (is4E ? 0xD4 : 0xD0) || header[1] != 0)
                {
                    throw new InvalidDataException("Unexpected MC response frame type.");
                }
                ReadExactly(stream, header, 2, header.Length - 2);
                int dataLength = BitConverter.ToUInt16(header, header.Length - 2);
                if (dataLength < 2)
                {
                    throw new InvalidDataException("The MC response is missing its end code.");
                }

                var response = new byte[header.Length + dataLength];
                Buffer.BlockCopy(header, 0, response, 0, header.Length);
                ReadExactly(stream, response, header.Length, dataLength);
                return response;
            }
            catch (Exception ex) when (ex is IOException || ex is InvalidDataException)
            {
                connection.Close();
                throw;
            }
        }

        private static void ReadExactly(NetworkStream stream, byte[] buffer, int offset, int count)
        {
            int end = offset + count;
            while (offset < end)
            {
                int received = stream.Read(buffer, offset, end - offset);
                if (received == 0)
                {
                    throw new EndOfStreamException("The connection closed before the MC response was complete.");
                }
                offset += received;
            }
        }

        private TcpClient _tcpClient { get; set; }

        public TcpClient TcpClient { get { return _tcpClient; } }


        #region [Check Socket Connection Status & Fire Events]

        private async Task<bool> TryReconnect()
        {
            //
            // 서버가 죽었어도, 클라이언트는 아직 연결을 가지고 있어서
            // 클라이언트에서 Close 를 하지 않으면, 아래 ConnectAsync 를 시도할 때, 소켓이 이미 연결되었다고 Exception 이 발생핸다.
            //
            _tcpClient.Close();
            _tcpClient.Dispose();
            _tcpClient = new TcpClient();

            //
            Task connectTask = _tcpClient.ConnectAsync(HostName, PortNumber);

            if (await Task.WhenAny(connectTask, Task.Delay(3_000)) == connectTask)
            {
                // 연결 성공
                Console.WriteLine("TryReconnect: 1111");
            }
            else
            {
                // 타임아웃 발생
                Console.WriteLine("TryReconnect: 2222");
            }

            return IsConnected();
        }


        private async void CheckConnectionStatus(object data)
        {
            // suspend timer until task finishes
            _connection_check_timer?.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            
            // do connection check
            ConnectionStatus = IsConnected();

            // configure timer for next calling
            if (ConnectionStatus)
            {
                // if currently connected : 1 sec
                _connection_check_timer?.Change(1_000, System.Threading.Timeout.Infinite);
            }
            else
            {
                if( await TryReconnect())
                {
                    // reconnected : 1 sec
                    _connection_check_timer?.Change(1_000, System.Threading.Timeout.Infinite);
                }
                else
                {
                    // if currently DIS-connected : 10 sec
                    _connection_check_timer?.Change(10_000, System.Threading.Timeout.Infinite);
                }
            }

            Console.WriteLine("CheckConnectionStatus");
        }

        private System.Threading.Timer _connection_check_timer = null;

        private void StartConnectionCheckingTimer()
        {
            _connection_check_timer = new System.Threading.Timer(CheckConnectionStatus, null, 1_000, System.Threading.Timeout.Infinite);
            Console.WriteLine("StartConnectionCheckingTimer...");
        }

        private void StopConnectionCheckingTimer()
        {
            if (_connection_check_timer != null)
            {
                _connection_check_timer.Dispose();
            }
            _connection_check_timer = null;
            Console.WriteLine("StopConnectionCheckingTimer...");
        }


        private bool _connection_status = false;

        public bool ConnectionStatus
        {
            get { return _connection_status; }
            
            private set
            {
                if (_connection_status != value)
                {
                    _connection_status = value;
                    OnConnectionChanged(_connection_status);
                }
            }
        }

        public event EventHandler<bool> ConnectionChanged;

        private void OnConnectionChanged(bool connected)
        {
            ConnectionChanged?.Invoke(this, connected);
        }

        private byte[] _alive_check_buffer = new byte[1] { 0x00 };

        public bool IsConnected()
        {
            try
            {
                if (_tcpClient == null || _tcpClient.Client == null) return false;

                // 실제로 전송은 이루어지지 않고, 다만, 연결이 끊어져 있다면, Exception 발생함!
                _tcpClient.Client.Send(_alive_check_buffer, 0, 0);

                //
                return true;
            }
            catch (System.Net.Sockets.SocketException e1)
            {
                // Server Shutdown
                Console.WriteLine($"{DateTime.Now:HH:mm:ss}, Connection Lost OR Server Down...{e1.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss}, Exception: {ex.Message}");
                return false;
            }
        }

        #endregion

    }

}
