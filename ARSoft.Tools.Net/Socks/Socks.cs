using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ARSoft.Tools.Net.Socks
{
    /// <summary>
    /// 
    /// </summary>
    public static class Socks
    {
        private struct Socks4Response
        {
            public readonly byte Version;
            public readonly byte Command;
            // rest is ignored

            public Socks4Response(System.Net.Sockets.Socket socket)
            {
                var receiveBuffer = new byte[4];

                // Get the version
                socket.Receive(receiveBuffer, 1, SocketFlags.None);
                Version = receiveBuffer[0];

                // Get the command
                socket.Receive(receiveBuffer, 1, SocketFlags.None);
                Command = receiveBuffer[0];

                // Get the destPort (ignored)
                socket.Receive(receiveBuffer, 2, SocketFlags.None);

                // Get the destAddress (ignored)
                socket.Receive(receiveBuffer, 4, SocketFlags.None);
            }
            public Socks4Response(TcpClient tcpClient)
            {
                var receiveBuffer = new byte[4];

                // Get the version
                tcpClient.GetStream().Read(receiveBuffer, 0, 1);
                Version = receiveBuffer[0];

                // Get the command
                tcpClient.GetStream().Read(receiveBuffer, 0, 1);
                Command = receiveBuffer[0];

                // Get the destPort (ignored)
                tcpClient.GetStream().Read(receiveBuffer, 0, 2);

                // Get the destAddress (ignored)
                tcpClient.GetStream().Read(receiveBuffer, 0, 4);
            }
        }

        private struct Socks4Request
        {
            private readonly byte _command;
            private readonly short _targetPort;
            private readonly IPAddress _target;
            private readonly string _userId;

            public Socks4Request(byte command, short targetPort, IPAddress target, string userId)
            {
                _command = command;
                _targetPort = targetPort;
                _target = target;
                _userId = userId;
            }

            /// <summary>
            /// Serialize the request and return the binary representation
            /// </summary>
            /// <returns>Binary representation of the Socks4Request request.</returns>
            public byte[] GetBytes()
            {
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        // Version, 1 byte
                        bw.Write((byte)4);

                        // Command, 1 byte
                        bw.Write(_command);

                        // Target port, 2 bytes
                        bw.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(_targetPort)));

                        // Target address, 4 bytes
                        bw.Write(_target.GetAddressBytes());

                        // User id, n bytes
                        bw.Write(Encoding.ASCII.GetBytes(_userId));

                        // End-of-packet NULL, 1 byte
                        bw.Write((byte)0);
                    }

                    return ms.ToArray();
                }
            }
        }

        public static System.Net.Sockets.Socket ConnectSocketTo(IPAddress proxyAddress, short proxyPort, IPAddress destAddress, short destPort, string asciiUserId)
        {
            // create the socket
            var socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // connect to the proxy
            socket.Connect(new IPEndPoint(proxyAddress, proxyPort));

            // create the request
            var requestPacket = new Socks4Request(1, destPort, destAddress, asciiUserId);

            // Send the request
            socket.Send(requestPacket.GetBytes());

            // Get the response from teh socket
            var response = new Socks4Response(socket);

            if (response.Version != 0)
            {
                // Unexpected version (must be 0)
                throw new Socks4Exception(string.Format("Invalid version. Expected 0, got {0}.", response.Version));
            }

            if (response.Command < (byte)Socks4ResponseCommand.MinValue || response.Command > (byte)Socks4ResponseCommand.MaxValue)
            {
                // Invalid response command
                throw new Socks4Exception(string.Format("Invalid reply command. Got {0}.", response.Command));
            }

            if (response.Command != (byte)Socks4ResponseCommand.Granted)
            {
                // Request failed
                throw new Socks4Exception((Socks4ResponseCommand)response.Command);
            }

            // Connection made! Return the socket.
            return socket;
        }

        public static TcpClient ConnectTcpClientTo(IPAddress proxyAddress, short proxyPort, IPAddress destAddress, short destPort, string asciiUserId)
        {
            // create the socket
            var socket = new TcpClient(AddressFamily.InterNetwork);

            // connect to the proxy
            socket.Connect(new IPEndPoint(proxyAddress, proxyPort));

            // create the request
            var requestPacket = new Socks4Request(1, destPort, destAddress, asciiUserId);

            // Get the request
            var requestData = requestPacket.GetBytes();

            // Send the request
            socket.GetStream().Write(requestData, 0, requestData.Length);

            // Get the response from teh socket
            var response = new Socks4Response(socket);

            if (response.Version != 0)
            {
                // Unexpected version (must be 0)
                throw new Socks4Exception(string.Format("Invalid version. Expected 0, got {0}.", response.Version));
            }

            if (response.Command < (byte)Socks4ResponseCommand.MinValue || response.Command > (byte)Socks4ResponseCommand.MaxValue)
            {
                // Invalid response command
                throw new Socks4Exception(string.Format("Invalid reply command. Got {0}.", response.Command));
            }

            if (response.Command != (byte)Socks4ResponseCommand.Granted)
            {
                // Request failed
                throw new Socks4Exception((Socks4ResponseCommand)response.Command);
            }

            // Connection made! Return the socket.
            return socket;
        }

        public static TcpClient ConnectTcpClientTo(IPAddress proxyAddress, short proxyPort, IPAddress destAddress, short destPort, string asciiUserId, int timeout)
        {
            // create the socket
            var socket = new TcpClient(AddressFamily.InterNetwork);
            socket.ReceiveTimeout = timeout;
            socket.SendTimeout = timeout;

            // connect to the proxy
            socket.Connect(new IPEndPoint(proxyAddress, proxyPort));

            // create the request
            var requestPacket = new Socks4Request(1, destPort, destAddress, asciiUserId);

            // Get the request
            var requestData = requestPacket.GetBytes();

            // Send the request
            socket.GetStream().Write(requestData, 0, requestData.Length);

            // Get the response from teh socket
            var response = new Socks4Response(socket);

            if (response.Version != 0)
            {
                // Unexpected version (must be 0)
                throw new Socks4Exception(string.Format("Invalid version. Expected 0, got {0}.", response.Version));
            }

            if (response.Command < (byte)Socks4ResponseCommand.MinValue || response.Command > (byte)Socks4ResponseCommand.MaxValue)
            {
                // Invalid response command
                throw new Socks4Exception(string.Format("Invalid reply command. Got {0}.", response.Command));
            }

            if (response.Command != (byte)Socks4ResponseCommand.Granted)
            {
                // Request failed
                throw new Socks4Exception((Socks4ResponseCommand)response.Command);
            }

            // Connection made! Return the socket.
            return socket;
        }
    }
}
