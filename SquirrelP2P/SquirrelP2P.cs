using System.Net;
using System.Net.Sockets;

namespace ScorpionP2P
{
    //Peers that are always online
    public struct scorpion_P2P_static_peers
    {
        private string[] ip_1ist = null;
        public scorpion_P2P_static_peers(string[] ip_list)
        {
            //Fills the default ip list defined by the administrator
            this.ip_1ist = ip_1ist;
            return;
        }
    };

    public struct scorpion_P2P_object
    {
        /*
        This object allows us to store peer-client objects. This allows us to send and recieve data to a list of preferred peers or recieve data from other clients
        We can have one server but multpile clients per object as we could have multiple clients connected to us but may want to connect to multiple peers to retrieve information.

        Peers: can only send data
        Clients: can only request data

        These destinctions gives peers and clients specific roles within a single object.
        */

        public scorpion_P2P_object(long id, string name, string peer_IP_address, int peer_port, string RSA_priv_path, string RSA_pub_path, protocol_type protocol)
        {
            this.id = id;
            this.name = name;
            this.str_ip = peer_IP_address;
            this.port = (peer_port == null ? 8002 : peer_port);
            this.RSA_priv_path = RSA_priv_path;
            this.RSA_pub_path = RSA_pub_path;
            ip_addr = IPAddress.Parse(this.str_ip == null ? "127.0.0.1" : peer_IP_address);
            proto = protocol;
            peer = null;
            clients = new SimpleTCP.SimpleTcpClient[5];
            return;
        }

        //Peer consists of a server and a client
        public Exception startPeer()
        {
            try
            {
                SimpleTCP.SimpleTcpServer sctl = new SimpleTCP.SimpleTcpServer();
                sctl.ClientConnected += Sctl_ClientConnected;
                sctl.ClientDisconnected += Sctl_ClientDisconnected;
                sctl.DataReceived += Sctl_DataReceived;
                sctl.Start(this.ip_addr, this.port);
                this.peer = sctl;
                return null;
            }
            catch(Exception e)
            {
                return e;
            }
        }

        internal Exception startClient(string ip, int port)
        {
            SimpleTCP.SimpleTcpClient sctl = new SimpleTCP.SimpleTcpClient();
            try
            {
                sctl.Connect(ip, port);
                sctl.DataReceived += Sctl_clientDataReceived;
            }
            catch(Exception e){ return e; }
            finally { sctl = null; }
            return null;
        }

        public void sendAsPeer(byte[] data)
        {

        }

        public void retrieveAsClient(string api_str)
        {

        }

        long id;
        string name;
        string str_ip;
        IPAddress ip_addr;
        int port = 0;
        long last_active_ticks = 0;
        DateTime last_active_datetime = DateTime.MinValue;
        int ping = -1;
        protocol_type proto;
        string RSA_pub_path;
        private string RSA_priv_path;
        SimpleTCP.SimpleTcpServer peer;
        SimpleTCP.SimpleTcpClient[] clients;

        /*TCP server : Events*/
        internal void Sctl_ClientConnected(object sender, TcpClient e)
        {
            Console.WriteLine("TCP >> Client " + (IPEndPoint)e.Client.RemoteEndPoint + " connected");
            return;
        }

        internal void Sctl_ClientDisconnected(object sender, TcpClient e)
        {
            Console.WriteLine("TCP >> Client " + (IPEndPoint)e.Client.RemoteEndPoint + " disconnected");
            return;
        }

        internal void Sctl_DataReceived(object sender, SimpleTCP.Message e)
        {
            return;
        }

        /*Client: Events*/
        internal void Sctl_clientDataReceived(object sender, SimpleTCP.Message e)
        {
            return;
        }
    };

    public enum protocol_type
    {
        //What protocol to use
        tcp = 0x00,
    };

    //Starts a p2p session
    public class ScorpionP2P
    {
        private Dictionary<string, scorpion_P2P_object> peers_and_clients;

        //Server
        public void newP2P(string name, string host_ip_or_null, short host_port_or_null, string RSA_pub_path, string RSA_priv_path)
        {
            if(RSA_pub_path == null || RSA_priv_path == null)
                Console.WriteLine("Scorpion server started. No RSA keys have been assigned to this server. Non RSA servers can be read by MITM attacks and other sniffing techniques");
        
            scorpion_P2P_object p2p = new scorpion_P2P_object((peers_and_clients.Count+1), name, host_ip_or_null, (host_port_or_null == null ? 8002 : host_port_or_null), RSA_priv_path, RSA_pub_path, protocol_type.tcp);

            Exception started;
            if((started = p2p.startPeer()) == null)
                peers_and_clients.Add(name, p2p);
            return;
        }

        public async Task<bool> requestData()
        {
            return false;
        }
    }
}