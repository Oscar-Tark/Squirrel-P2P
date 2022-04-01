/*  <Scorpion P2P>
    Copyright (C) <2022+>  <Oscar Arjun Singh Tark>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security;

namespace ScorpionP2P
{
    //Peers that are always online
    public struct scorpion_P2P_static_peers
    {
        private string[] ip_1ist;
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

        public scorpion_P2P_object(long id, string name, string peer_IP_address, short peer_port, string RSA_priv_path, string RSA_pub_path, protocol_type protocol)
        {
            this.id = id;
            this.name = name;
            this.str_ip = peer_IP_address;
            this.port = (peer_port = null ? 8002 : peer_port);
            this.RSA_priv_path = RSA_priv_path;
            this.RSA_pub_path = RSA_pub_path;
            ip_addr = IPAddress.Parse(this.str_ip == null ? "127.0.0.1" : IP_address);
            proto = protocol;
            server = server_or_null;
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
                return null;
            }
            catch(Exception e)
            {
                return e;
            }
        }

        private Exception startClient()
        {
            SimpleTCP.SimpleTcpClient sctl = new SimpleTCP.SimpleTcpClient();
            sctl.Connect(ip, port);
            sctl.DataReceived += Sctl_clientDataReceived;
        }

        public void sendAsPeer()
        {

        }

        public void retrieveAsClient()
        {

        }

        long id;
        string name;
        string str_ip;
        IPAddress ip_addr;
        short port = 0;
        long last_active_ticks = 0;
        DateTime last_active_datetime = DateTime.MinValue;
        int ping = -1;
        protocol_type proto;
        string RSA_pub_path;
        private string RSA_priv_path;
        SimpleTCP.SimpleTcpServer peer;
        SimpleTCP.SimpleTcpClient[] clients;

        /*TCP server : Events*/
        void Sctl_ClientConnected(object sender, TcpClient e)
        {
            Console.WriteLine("TCP >> Client " + (IPEndPoint)e.Client.RemoteEndPoint + " connected");
            return;
        }

        void Sctl_ClientDisconnected(object sender, TcpClient e)
        {
            Console.WriteLine("TCP >> Client " + (IPEndPoint)e.Client.RemoteEndPoint + " disconnected");
            return;
        }

        public void Sctl_DataReceived(object sender, SimpleTCP.Message e)
        {
            return;
        }
    };

    public struct protocol_type
    {
        //What protocol to use
        const short tcp = 0x00;
        const short udp = 0x01;
    };

    //Starts a p2p session
    public class ScorpionP2P
    {
        Dictionary<string, scorpion_P2P_object> peers_and_clients;

        //Server
        public void newP2P(string name, string host_ip_or_null, short host_port_or_null, string RSA_pub_path, string RSA_priv_path)
        {
            if(RSA_public_path == null || RSA_private_path == null)
                HANDLE.write_warning("Scorpion server started. No RSA keys have been assigned to this server. Non RSA servers can be read by MITM attacks and other sniffing techniques");
        
            scorpion_P2P_object p2p = new scorpion_P2P_object(++peer_servers.Count, name, host_ip_or_null, host_port_or_null = null ? 8002 : host_port_or_null);

            var started;
            if((started = p2p.startServer()) == null)
                peers_and_clients.Add(name, p2p);
            return;
        }

        public async Task<bool> requestData()
        {
            return false;
        }
    }
}