using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public class MatchmakerLink
{
	public MatchmakerHandler handler;

	public ConcurrentQueue<Action> message_queue = new ConcurrentQueue<Action>();

	public void Poll()
	{
		while (message_queue.TryDequeue(out Action action)) action();
	}

	public void Open(IPEndPoint endpoint)
	{
		client = new UdpClient(endpoint);
	}

	public void Receive()
	{
		client.ReceiveAsync().ContinueWith(task =>
		{
			Receive();
			Dispatch(task.Result.Buffer, task.Result.RemoteEndPoint);
		});
	}

	public IPEndPoint endpoint;

	public void Connect(IPEndPoint endpoint)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)0);
		writer.Write(crc);
		writer.Write((byte)0);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in LobbyIdentity message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)1);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in LobbyRequest message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)2);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in LobbyIdentity message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)1);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in LobbyRequest message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)2);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Dispatch(byte[] bytes, IPEndPoint endpoint)
	{
		BinaryReader reader = new BinaryReader(new MemoryStream(bytes));
		switch (reader.ReadByte())
		{
			case 0:
			{
				uint remote_crc = reader.ReadUInt32();
				if (remote_crc != crc)
					break;
				switch (reader.ReadByte())
				{
					case 0:
					{
						MemoryStream stream = new MemoryStream();
						BinaryWriter writer = new BinaryWriter(stream);
						writer.Write((byte)0);
						writer.Write(crc);
						writer.Write((byte)1);
						byte[] out_bytes = stream.ToArray();
						client.SendAsync(out_bytes, out_bytes.Length, endpoint);
						break;
					}
					case 1:
					{
						this.endpoint = endpoint;
						message_queue.Enqueue(() => handler.ConnectionHandler(endpoint));
						break;
					}
					default:
						break;
				}
				break;
			}
		case 1:
		{
			LobbyIdentity message = LobbyIdentity.Deserialize(reader);
			message_queue.Enqueue(() => handler.LobbyIdentityHandler(endpoint, message));
			break;
		}
		case 2:
		{
			LobbyRequest message = LobbyRequest.Deserialize(reader);
			message_queue.Enqueue(() => handler.LobbyRequestHandler(endpoint, message));
			break;
		}
		default:
			break;
		}
	}

	public const uint crc = 0xacd77b6f;
	private UdpClient client;
}
