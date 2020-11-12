using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public class NetLink
{
	public NetworkHandler handler;

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

	public void Send(IPEndPoint endpoint, in AbilityUsed message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)1);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in DoorUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)2);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in GameOver message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)3);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in GamePhaseUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)4);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in GameSettings message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)5);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in GameStartRequested message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)6);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in GivenTasks message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)7);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in Heartbeat message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)8);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in KillAttempted message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)9);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in LightUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)10);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in MeetingRequested message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)11);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in MobEjected message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)12);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in MobRemoved message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)13);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in MobRoleUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)14);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in MobStateUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)15);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in MobTeleport message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)16);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in MobUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)17);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in PickupCooldown message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)18);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in PlayerUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)19);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in PlayerVoted message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)20);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in ReportAttempted message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)21);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in ResetGameSettings message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)22);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in RestartRequested message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)23);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in SabotageTaskUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)24);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in SendEvidence message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)25);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in TaskListUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)26);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in TaskUpdate message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)27);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(IPEndPoint endpoint, in VoiceFrame message)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)28);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in AbilityUsed message)
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

	public void Send(in DoorUpdate message)
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

	public void Send(in GameOver message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)3);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in GamePhaseUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)4);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in GameSettings message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)5);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in GameStartRequested message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)6);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in GivenTasks message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)7);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in Heartbeat message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)8);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in KillAttempted message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)9);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in LightUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)10);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in MeetingRequested message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)11);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in MobEjected message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)12);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in MobRemoved message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)13);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in MobRoleUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)14);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in MobStateUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)15);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in MobTeleport message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)16);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in MobUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)17);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in PickupCooldown message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)18);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in PlayerUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)19);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in PlayerVoted message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)20);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in ReportAttempted message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)21);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in ResetGameSettings message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)22);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in RestartRequested message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)23);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in SabotageTaskUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)24);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in SendEvidence message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)25);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in TaskListUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)26);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in TaskUpdate message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)27);
		message.Serialize(writer);
		byte[] bytes = stream.ToArray();
		client.SendAsync(bytes, bytes.Length, endpoint);
	}

	public void Send(in VoiceFrame message)
	{
		if (endpoint == null)
			return;
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write((byte)28);
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
			AbilityUsed message = AbilityUsed.Deserialize(reader);
			message_queue.Enqueue(() => handler.AbilityUsedHandler(endpoint, message));
			break;
		}
		case 2:
		{
			DoorUpdate message = DoorUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.DoorUpdateHandler(endpoint, message));
			break;
		}
		case 3:
		{
			GameOver message = GameOver.Deserialize(reader);
			message_queue.Enqueue(() => handler.GameOverHandler(endpoint, message));
			break;
		}
		case 4:
		{
			GamePhaseUpdate message = GamePhaseUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.GamePhaseUpdateHandler(endpoint, message));
			break;
		}
		case 5:
		{
			GameSettings message = GameSettings.Deserialize(reader);
			message_queue.Enqueue(() => handler.GameSettingsHandler(endpoint, message));
			break;
		}
		case 6:
		{
			GameStartRequested message = GameStartRequested.Deserialize(reader);
			message_queue.Enqueue(() => handler.GameStartRequestedHandler(endpoint, message));
			break;
		}
		case 7:
		{
			GivenTasks message = GivenTasks.Deserialize(reader);
			message_queue.Enqueue(() => handler.GivenTasksHandler(endpoint, message));
			break;
		}
		case 8:
		{
			Heartbeat message = Heartbeat.Deserialize(reader);
			message_queue.Enqueue(() => handler.HeartbeatHandler(endpoint, message));
			break;
		}
		case 9:
		{
			KillAttempted message = KillAttempted.Deserialize(reader);
			message_queue.Enqueue(() => handler.KillAttemptedHandler(endpoint, message));
			break;
		}
		case 10:
		{
			LightUpdate message = LightUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.LightUpdateHandler(endpoint, message));
			break;
		}
		case 11:
		{
			MeetingRequested message = MeetingRequested.Deserialize(reader);
			message_queue.Enqueue(() => handler.MeetingRequestedHandler(endpoint, message));
			break;
		}
		case 12:
		{
			MobEjected message = MobEjected.Deserialize(reader);
			message_queue.Enqueue(() => handler.MobEjectedHandler(endpoint, message));
			break;
		}
		case 13:
		{
			MobRemoved message = MobRemoved.Deserialize(reader);
			message_queue.Enqueue(() => handler.MobRemovedHandler(endpoint, message));
			break;
		}
		case 14:
		{
			MobRoleUpdate message = MobRoleUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.MobRoleUpdateHandler(endpoint, message));
			break;
		}
		case 15:
		{
			MobStateUpdate message = MobStateUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.MobStateUpdateHandler(endpoint, message));
			break;
		}
		case 16:
		{
			MobTeleport message = MobTeleport.Deserialize(reader);
			message_queue.Enqueue(() => handler.MobTeleportHandler(endpoint, message));
			break;
		}
		case 17:
		{
			MobUpdate message = MobUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.MobUpdateHandler(endpoint, message));
			break;
		}
		case 18:
		{
			PickupCooldown message = PickupCooldown.Deserialize(reader);
			message_queue.Enqueue(() => handler.PickupCooldownHandler(endpoint, message));
			break;
		}
		case 19:
		{
			PlayerUpdate message = PlayerUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.PlayerUpdateHandler(endpoint, message));
			break;
		}
		case 20:
		{
			PlayerVoted message = PlayerVoted.Deserialize(reader);
			message_queue.Enqueue(() => handler.PlayerVotedHandler(endpoint, message));
			break;
		}
		case 21:
		{
			ReportAttempted message = ReportAttempted.Deserialize(reader);
			message_queue.Enqueue(() => handler.ReportAttemptedHandler(endpoint, message));
			break;
		}
		case 22:
		{
			ResetGameSettings message = ResetGameSettings.Deserialize(reader);
			message_queue.Enqueue(() => handler.ResetGameSettingsHandler(endpoint, message));
			break;
		}
		case 23:
		{
			RestartRequested message = RestartRequested.Deserialize(reader);
			message_queue.Enqueue(() => handler.RestartRequestedHandler(endpoint, message));
			break;
		}
		case 24:
		{
			SabotageTaskUpdate message = SabotageTaskUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.SabotageTaskUpdateHandler(endpoint, message));
			break;
		}
		case 25:
		{
			SendEvidence message = SendEvidence.Deserialize(reader);
			message_queue.Enqueue(() => handler.SendEvidenceHandler(endpoint, message));
			break;
		}
		case 26:
		{
			TaskListUpdate message = TaskListUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.TaskListUpdateHandler(endpoint, message));
			break;
		}
		case 27:
		{
			TaskUpdate message = TaskUpdate.Deserialize(reader);
			message_queue.Enqueue(() => handler.TaskUpdateHandler(endpoint, message));
			break;
		}
		case 28:
		{
			VoiceFrame message = VoiceFrame.Deserialize(reader);
			message_queue.Enqueue(() => handler.VoiceFrameHandler(endpoint, message));
			break;
		}
		default:
			break;
		}
	}

	public const uint crc = 0x1b1d05f6;
	private UdpClient client;
}
