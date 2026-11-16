using SuperSimpleTcp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using MCProtocolEnums;

namespace MCProtocolPLCEmulator
{
    public class MCProtocolServer
    {

        #region [props]

        SuperSimpleTcp.SimpleTcpServer _Server = null;
        public SuperSimpleTcp.SimpleTcpServer TCPServer
        {
            get { return _Server; }
        }

        private string _Server_ip = "127.0.0.1";
        private int _Server_port = 8_000;
        private int _IdleClientTimeoutMs = 600_000;
        private string _LastClientIpPort = null;

        #endregion

        #region [Logger Delegate]

        public delegate void LoggerMethod(object sender, string msg);
        public LoggerMethod Logger = null;

        #endregion

        public SimpleTcpServer CreateServerInstance(string server_ip, int server_port, List<PLCWordMemory> mem)
        {
            if (_Server != null)
            {
                _Server.Stop();
                _Server = null;
            }

            _Server_ip = server_ip;
            _Server_port = server_port;
            var server = new SimpleTcpServer(_Server_ip, _Server_port);
            var pending = new ConcurrentDictionary<string, List<byte>>();
            _Server = server;

            _Server.Events.ClientConnected += ClientConnected;
            _Server.Events.ClientDisconnected += (sender, args) =>
            {
                pending.TryRemove(args.IpPort, out _);
                ClientDisconnected(sender, args);
            };
            _Server.Events.DataReceived += (sender, args) => DataReceived(server, pending, mem, args);
            _Server.Events.DataSent += DataSent;
            _Server.Settings.UseAsyncDataReceivedEvents = false;
            _Server.Keepalive.EnableTcpKeepAlives = true;
            _Server.Settings.IdleClientTimeoutMs = _IdleClientTimeoutMs;
            _Server.Settings.MutuallyAuthenticate = false;
            _Server.Settings.AcceptInvalidCertificates = true;
            _Server.Settings.NoDelay = true;
            _Server.Logger = PrivateLogger;


            PrivateLogger($"Server Starts: {_Server_ip}:{_Server_port}");

            return _Server;
        }

        private void ClientConnected(object sender, ConnectionEventArgs e)
        {
            _LastClientIpPort = e.IpPort;
            PrivateLogger("[" + e.IpPort + "] client connected");
            ListClients();
        }

        private void ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            PrivateLogger("[" + e.IpPort + "] client disconnected: " + e.Reason.ToString());
            ListClients();
        }

        private void DataReceived(SimpleTcpServer server, ConcurrentDictionary<string, List<byte>> pending,
            List<PLCWordMemory> memories, DataReceivedEventArgs e)
        {
            var buffer = pending.GetOrAdd(e.IpPort, _ => new List<byte>());
            buffer.AddRange(e.Data);
            int consumed = 0;
            try
            {
                while (buffer.Count - consumed >= 2)
                {
                    int protocol = buffer[consumed] | (buffer[consumed + 1] << 8);
                    if (protocol != MCProtocolCommand.Request3E && protocol != MCProtocolCommand.Request4E)
                    {
                        throw new ArgumentException("Unsupported request subheader.");
                    }

                    int lengthOffset = (int)MC3ERequestFrameByteIndex.DataLength;
                    if (protocol == MCProtocolCommand.Request4E)
                    {
                        lengthOffset += MCProtocolCommand.SerialAndReservedLength;
                    }
                    int headerLength = lengthOffset + 2;
                    if (buffer.Count - consumed < headerLength)
                    {
                        break;
                    }

                    int lengthIndex = consumed + lengthOffset;
                    int dataLength = buffer[lengthIndex] | (buffer[lengthIndex + 1] << 8);
                    int frameLength = headerLength + dataLength;
                    if (buffer.Count - consumed < frameLength)
                    {
                        break;
                    }

                    var frame = new byte[frameLength];
                    buffer.CopyTo(consumed, frame, 0, frameLength);
                    ProcessRequest(server, memories, e.IpPort, frame);
                    consumed += frameLength;
                }
            }
            catch (ArgumentException ex)
            {
                PrivateLogger($"[{e.IpPort}] Invalid MC request: {ex.Message}");
                pending.TryRemove(e.IpPort, out _);
                server.DisconnectClient(e.IpPort);
                return;
            }
            buffer.RemoveRange(0, consumed);
        }

        private void ProcessRequest(SimpleTcpServer server, List<PLCWordMemory> memories, string client, byte[] frame)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in frame)
            {
                sb.Append(item.ToString("X2"));
            }
            PrivateLogger($"[{client}] recv : {sb}");


            var req = MCProtocolCommand.DecodeRequestCommand(frame);
            PrivateLogger("Request Command:");
            PrivateLogger(req.ToString());
            PrivateLogger("");

            PLCWordMemory target = memories.Find(x => x.DeviceLetter.ToString() == ((EnumPLCDeviceType)req.DeviceType).ToString());
            var end_code = MCProtocolCommand.ExecuteRequest(req, target);

            var res = MCProtocolCommand.SetResponseMessage(req, (ushort)end_code, req.Values);
            PrivateLogger("Response Message:");
            PrivateLogger(res.ToString());

            server.Send(client, MCProtocolCommand.EncodeResponseMessage(res));
        }

        private void DataSent(object sender, DataSentEventArgs e)
        {
            PrivateLogger("[" + e.IpPort + "] sent " + e.BytesSent + " bytes");
        }

        private void PrivateLogger(string msg)
        {
            Console.WriteLine(msg);
            Logger?.Invoke(this, msg);
        }

        private void ListClients()
        {
            PrivateLogger("Clinets List:");
            foreach (var client in _Server.GetClients())
            {
                PrivateLogger(client);
            }
        }

        private void Send(string client_ip_port_string, string data)
        {
            _Server.Send(client_ip_port_string, Encoding.UTF8.GetBytes(data));
        }

        private void SendAsync(string client_ip_port_string, string data)
        {
            _Server.SendAsync(client_ip_port_string, Encoding.UTF8.GetBytes(data)).Wait();
        }

        private void RemoveClient(string client_ip_port_string)
        {
            _Server.DisconnectClient(client_ip_port_string);
        }
    }
}
