using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<PLCWordMemory> _plcWordMemories = null;

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
            _plcWordMemories = mem;



            _Server = new SuperSimpleTcp.SimpleTcpServer(_Server_ip, _Server_port);

            _Server.Events.ClientConnected += ClientConnected;
            _Server.Events.ClientDisconnected += ClientDisconnected;
            _Server.Events.DataReceived += DataReceived;
            _Server.Events.DataSent += DataSent;
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

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in e.Data)
            {
                sb.Append(item.ToString("X2"));
            }
            PrivateLogger($"[{e.IpPort}] recv : {sb}");


            var req = MCProtocolCommand.DecodeMC3ERequestCommand(e.Data.ToArray());
            PrivateLogger("Request Command:");
            PrivateLogger(req.ToString());
            PrivateLogger("");

            PLCWordMemory target = _plcWordMemories.Find(x => x.DeviceLetter.ToString() == ((EnumPLCDeviceType)req.DeviceType).ToString());
            var end_code = MCProtocolCommand.ExecuteRequest(req, target);

            var res = MCProtocolCommand.SetResponseMessage(req, (ushort)end_code, req.Values);
            PrivateLogger("Response Message:");
            PrivateLogger(res.ToString());

            _Server.Send(e.IpPort, MCProtocolCommand.EncodeMC3EResponseMessage(res));
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
