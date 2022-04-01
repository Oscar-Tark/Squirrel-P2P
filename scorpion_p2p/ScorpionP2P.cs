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
    public struct scorpion_P2P_object
    {
        public scorpion_P2P_object(SimpleTCP.SimpleTcpServer server_or_null, long id, string name, string IP_address, short port, string RSA_priv_path, string RSA_pub_path, bool is_server, protocol_type protocol)
        {
            this.id = id;
            this.name = name;
            this.str_ip = IP_address;
            this.port = port;
            this.RSA_priv_path = RSA_priv_path;
            this.RSA_pub_path = RSA_pub_path;
            ip_addr = IPAddress.Parse(this.str_ip == null ? "127.0.0.1" : IP_address);
            this.is_server = is_server;
            proto = protocol;
            server = server_or_null;
            return;
        }

        public Exception startServer()
        {
            try
            {
                sctl.Start(this.ip_addr, this.port);
                return null;
            }
            catch(Exception e)
            {
                return e;
            }
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
        bool is_server = false;
        string RSA_pub_path;
        private string RSA_priv_path;
        SimpleTCP.SimpleTcpServer server;
    };

    public struct protocol_type
    {
        //What protocol to use
        const short tcp = 0x00;
        const short udp = 0x01;
    };

    public class ScorpionP2PSlave
    {
        //Client
        public async Task<bool> broadcastData()
        {
            return false;
        }
    }

    public class ScorpionP2PMaster
    {
        Dictionary<string, scorpion_P2P_object> peer_servers;

        //Server
        public void newServer(string name, string host_ip_or_null, short host_port_or_null, string RSA_pub_path, string RSA_priv_path)
        {
            SimpleTCP.SimpleTcpServer sctl = new SimpleTCP.SimpleTcpServer();
            sctl.ClientConnected += Sctl_ClientConnected;
            sctl.ClientDisconnected += Sctl_ClientDisconnected;
            sctl.DataReceived += Sctl_DataReceived;
            
            if(RSA_public_path == null || RSA_private_path == null)
                HANDLE.write_warning("Scorpion server started. No RSA keys have been assigned to this server. Non RSA servers can be read by MITM attacks and other sniffing techniques");
        
            scorpion_P2P_object p2p = new scorpion_P2P_object(++peer_servers.Count, name, host_ip_or_null, host_port_or_null = null ? 8002 : host_port_or_null);

            var started;
            if((started = p2p.startServer()) == null)
                peer_servers.Add(name, p2p);
            
            sctl.Dispose();
            
            //sctl.Start(ipa, port);//(port, true);
            return;
        }

        public async Task<bool> broadcastData()
        {
            return false;
        }

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
    }
}