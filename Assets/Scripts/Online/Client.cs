using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public enum Command
{
    handshake,
    connectToRoom,
    disconnectFromRoom,
    addPlayer,
    removePlayer,
    synchPosPlayer,
    synchStatsPlayer,
    disconnect,
    hit,
    ping,
    start
}

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip;
    public int port;

    public int id = -1;
    public int roomId = -1;
    public int roomCount = 0;

    public TCP tcp;

    private bool isConnected = false;

    public delegate void Execute(int id, Packet data);
    public Dictionary<Command, Execute> packetHandlers;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        packetHandlers = new Dictionary<Command, Execute>()
        {
            { Command.handshake, HandshakeCallback },
            { Command.connectToRoom, OnConnectToRoom },
            { Command.disconnectFromRoom, OnDisconnectFromRoom },
            { Command.addPlayer, OnAddPlayer },
            { Command.synchPosPlayer, OnSynchPlayerPos },
            { Command.synchStatsPlayer, OnSynchPlayerStats },
            { Command.removePlayer, OnRemovePlayer },
            { Command.start, OnStartGame }
        };

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ConnectToServer();
    }

    private void FixedUpdate()
    {
        ThreadManager.UpdateMain();
    }

    private void OnApplicationQuit()
    {
        Disconnect(); // Disconnect when the game is closed
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        tcp = new TCP();

        isConnected = true;
        tcp.Connect(); // Connect tcp, udp gets connected once tcp is done
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        /// <summary>Attempts to connect to the server via TCP.</summary>
        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        /// <summary>Initializes the newly connected client's TCP-related info.</summary>
        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            Packet packet = new Packet(Command.handshake);
            packet.Write(-1);
            SendData(packet);
        }

        /// <summary>Sends data to the client via TCP.</summary>
        /// <param name="_packet">The packet to send.</param>
        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    _packet.WriteLength();
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null); // Send data to server
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        /// <summary>Reads incoming data from the stream.</summary>
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data)); // Reset receivedData if all data was handled
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();
            }
        }

        /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
        /// <param name="_data">The recieved data.</param>
        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                // If client's received data contains a packet
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    // If packet contains no data
                    return true; // Reset receivedData instance to allow it to be reused
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                // While packet contains data AND packet data length doesn't exceed the length of the packet we're reading
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        Command _packetId = (Command)_packet.ReadInt();
                        if (_packetId != Command.ping)
                        {
                            int id = _packet.ReadInt();
                            instance.packetHandlers[_packetId](id, _packet); // Call appropriate method to handle the packet
                        }
                    }
                });

                _packetLength = 0; // Reset packet length
                if (receivedData.UnreadLength() >= 4)
                {
                    // If client's received data contains another packet
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        // If packet contains no data
                        return true; // Reset receivedData instance to allow it to be reused
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true; // Reset receivedData instance to allow it to be reused
            }

            return false;
        }

        /// <summary>Disconnects from the server and cleans up the TCP connection.</summary>
        private void Disconnect()
        {
            instance.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    /// <summary>Disconnects from the server and stops all network traffic.</summary>
    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();

            Debug.Log("Disconnected from server.");
        }
    }

    void HandshakeCallback(int id, Packet data)
    {
        Debug.Log("Handshake callback");
        this.id = id;
    }

    void OnConnectToRoom(int id, Packet data)
    {
        Debug.Log("Connected to room");
        if (id != this.id)
            Debug.LogError("Wrong params");

        roomId = data.ReadInt();
        int playersCount = data.ReadInt();
        roomCount = playersCount;
        for (int i = 0; i < playersCount; i++)
        {
            int playerId = data.ReadInt();
            ThreadManager.ExecuteOnMainThread(() => GameController.instance.AddPlayer(playerId));
        }
    }

    void OnDisconnectFromRoom(int id, Packet data)
    {
        if (id == this.id)
        {
            roomId = -1;
            roomCount = 0;
            ThreadManager.ExecuteOnMainThread(() => GameController.instance.Clear(id));
        }

        roomCount = data.ReadInt();
        ThreadManager.ExecuteOnMainThread(() => GameController.instance.RemovePlayer(id));

    }

    void OnAddPlayer(int id, Packet data)
    {
        if (id != this.id)
            Debug.LogError("Wrong params");

        int playerId = data.ReadInt();
        roomCount++;
        ThreadManager.ExecuteOnMainThread(() => GameController.instance.AddPlayer(playerId));
    }

    void OnSynchPlayerPos(int id, Packet data)
    {
        if (id != this.id)
            Debug.LogError("Wrong params");

        int playerId = data.ReadInt();
        Vector3 pos = data.ReadVector3();
        Vector3 velocity = data.ReadVector3();
        ThreadManager.ExecuteOnMainThread(() => GameController.instance.SynchPlayerPos(playerId, pos, velocity));
    }
    void OnSynchPlayerStats(int id, Packet data)
    {
        if (id != this.id)
            Debug.LogError("Wrong params");

        int playerId = data.ReadInt();
        bool isUFS = data.ReadBool();
        bool stunned = data.ReadBool();
        bool killed = data.ReadBool();
        bool isHost = data.ReadBool();
        ThreadManager.ExecuteOnMainThread(() => GameController.instance.SynchPlayerStats(playerId, isUFS, stunned, killed, isHost));
    }

    void OnRemovePlayer(int id, Packet data)
    {
        if (id != this.id)
            Debug.LogError("Wrong params");

        int playerId = data.ReadInt();
        ThreadManager.ExecuteOnMainThread(() => GameController.instance.RemovePlayer(playerId));
    }

    void OnStartGame(int id, Packet data)
    {
        if (id != this.id)
            Debug.LogError("Wrong params");

        ThreadManager.ExecuteOnMainThread(() => GameController.instance.LoadGameLevel());
    }

    public void SynchPlayerPos(Vector3 pos, Vector3 v)
    {
        Packet packet = new Packet(Command.synchPosPlayer);
        packet.Write(id);
        packet.Write(pos);
        packet.Write(v);
        tcp.SendData(packet);
    }

    public void HitTargets(List<int> players)
    {
        if (players.Count == 0)
            return;

        Packet packet = new Packet(Command.hit);
        packet.Write(id);
        packet.Write(players.Count);

        foreach (int player in players)
            packet.Write(player);

        tcp.SendData(packet);
    }

    public void ConnectToRoom()
    {
        Packet packet = new Packet(Command.connectToRoom);
        packet.Write(id);
        packet.Write(1); // room id
        tcp.SendData(packet);
    }

    public void DisconnectToRoom()
    {
        Packet packet = new Packet(Command.disconnectFromRoom);
        packet.Write(id);
        packet.Write(roomId);
        tcp.SendData(packet);
    }

    public void StartGame()
    {

    }
}