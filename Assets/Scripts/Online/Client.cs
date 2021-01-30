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
	spawnPlayer,
	removePlayer,
	synchPosPlayer,
	disconnect
}

public class Client : MonoBehaviour
{
	public static Client instance;

	public int id;
	public int roomId;

	private TcpClient socketConnection;
	private Thread clientReceiveThread;

	private delegate void Execute(int id, Packet data);
	private Dictionary<Command, Execute> packetHandlers;

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
			{ Command.connectToRoom, ConnectToRoomCallback },
			{ Command.spawnPlayer, SpawnPlayer },
            { Command.synchPosPlayer, SynchPlayerPosCallback }
        };

		socketConnection = new TcpClient("100.83.45.67", 8052);
		ConnectToTcpServer();
	}

    private void Start()
	{
		Packet packet = new Packet(Command.handshake);
		packet.Write(-1);
		SendMessage(packet);
	}

    private void Update()
    {
		ThreadManager.UpdateMain();
    }

    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            //clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}

	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData()
	{
		try
		{
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);

						using (Packet packet = new Packet(bytes))
						{
							Command command = (Command)packet.ReadInt();
							int uid = packet.ReadInt();
							packetHandlers[command](uid, packet);
						}
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public void SendMessage(Packet data)
	{
		if (socketConnection == null)
		{
			return;
		}

		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				byte[] serverMessageAsByteArray = data.ToArray();
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
			}
		}
		catch (SocketException socketException)
		{
			Console.WriteLine("Socket exception: " + socketException);
		}
	}

	void HandshakeCallback(int id, Packet data)
	{
		this.id = id;

		Packet packet = new Packet(Command.connectToRoom);
		packet.Write(this.id);
		packet.Write(1); // room id
		SendMessage(packet);
	}

	void ConnectToRoomCallback(int id, Packet data)
    {
		if (id != this.id)
			Debug.LogError("Wrong params");

		roomId = data.ReadInt();
		int playersCount = data.ReadInt();
		for(int i = 0; i < playersCount; i++)
        {
			int playerId = data.ReadInt();
			ThreadManager.ExecuteOnMainThread(()=> GameController.instance.SpawnPlayer(playerId, "", Vector3.zero));
        }

	}

	void SpawnPlayer(int id, Packet data)
	{
		if (id != this.id)
			Debug.LogError("Wrong params");

		int playerId = data.ReadInt();
			ThreadManager.ExecuteOnMainThread(() => GameController.instance.SpawnPlayer(playerId, "", Vector3.zero));
	}

	void SynchPlayerPosCallback(int id, Packet data)
	{
		if (id != this.id)
			Debug.LogError("Wrong params");

		int playerId = data.ReadInt();
		Vector3 pos = data.ReadVector3();
		Vector3 velocity = data.ReadVector3();
		ThreadManager.ExecuteOnMainThread(() => GameController.instance.SynchPlayerPos(playerId, pos, velocity));
	}
}