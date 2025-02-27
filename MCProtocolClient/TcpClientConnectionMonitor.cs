using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MCProtocolClient
{
    public class TcpClientConnectionMonitor
    {
        private TcpClient _client;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _lastConnectionState;

        public event Action<bool> ConnectedChanged; // true: 연결됨, false: 끊어짐

        public TcpClientConnectionMonitor(TcpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _lastConnectionState = IsConnected();
        }

        public void StartMonitoring(int intervalMs = 1000)
        {
            if (_cancellationTokenSource != null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => MonitorConnection(intervalMs, _cancellationTokenSource.Token));
        }

        public void StopMonitoring()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
        }

        private async Task MonitorConnection(int intervalMs, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //
                Check();

                // await Task.Delay(intervalMs, cancellationToken) --> throrws an exeption
                await Task.Delay(intervalMs, cancellationToken).ContinueWith(tsk =>
                {
                    Check();
                }); // --> this will save you from writing code for try...catch
            }
        }

        private void Check()
        {
            bool isConnected = IsConnected();
            if (_lastConnectionState != isConnected)
            {
                _lastConnectionState = isConnected;
                ConnectedChanged?.Invoke(isConnected);
            }
        }


        private byte[] _alive_check_buffer = new byte[1] { 0 };
        public bool IsConnected()
        {
            try
            {
                if (_client == null || _client.Client == null) return false;

                // 실제로 전송은 이루어지지 않고, 다만, 연결이 끊어져 있다면, Exception 발생함!
                _client.Client.Send(_alive_check_buffer, 0, 0);

                //
                return true;
            }
            catch
            {
                // 서버가 닫힘
                return false; 
            }
        }
    }
}
