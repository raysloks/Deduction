using System;
using System.Net;

public class MatchmakerHandler
{
    public GameController controller;

    public MatchmakerLink link;

    public string address = "172.105.79.194";
    public int port = 58712;
    public string lobby;

    public MatchmakerHandler()
    {
        link = new MatchmakerLink();
        link.handler = this;
        link.Open(new IPEndPoint(IPAddress.Any, 0));
        link.Receive();
    }

    public void ConnectToLobby(string lobby)
    {
        this.lobby = lobby;
        link.Connect(new IPEndPoint(IPAddress.Parse(address), port));
        controller.connectionState = GameController.ConnectionState.ConnectingToMatchmaker;
    }

    internal void LobbyIdentityHandler(IPEndPoint endpoint, LobbyIdentity message)
    {
        controller.handler.link.Connect(new IPEndPoint(IPAddress.Parse(message.address), message.port));
        controller.connectionState = GameController.ConnectionState.ConnectingToLobby;
    }

    internal void LobbyRequestHandler(IPEndPoint endpoint, LobbyRequest message)
    {
    }

    internal void ConnectionHandler(IPEndPoint endpoint)
    {
        link.Send(endpoint, new LobbyRequest { lobby = lobby });
        controller.connectionState = GameController.ConnectionState.RequestingLobby;
    }
}
