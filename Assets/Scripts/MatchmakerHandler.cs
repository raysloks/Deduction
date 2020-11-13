using System;
using System.Net;

public class MatchmakerHandler
{
    public GameController game;

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
        game.connectionState = GameController.ConnectionState.ConnectingToMatchmaker;
        game.timeout = game.time + 2000000000;
    }

    internal void LobbyIdentityHandler(IPEndPoint endpoint, LobbyIdentity message)
    {
        if (message.address.Length == 0)
        {
            game.connectionState = GameController.ConnectionState.None;
            game.timeout = 0;
            if (message.lobby.Length == 0)
                game.CreateInfoPopup("No vacant lobbies.");
            else
                game.CreateInfoPopup("Lobby not found.");
        }
        else
        {
            if (lobby.Length == 0)
                lobby = message.lobby;
            if (lobby != message.lobby)
                game.CreateInfoPopup("Lobby mismatch.");
            game.handler.link.Connect(new IPEndPoint(IPAddress.Parse(message.address), message.port));
            game.connectionState = GameController.ConnectionState.ConnectingToLobby;
            game.timeout = game.time + 2000000000;
        }
    }

    internal void LobbyRequestHandler(IPEndPoint endpoint, LobbyRequest message)
    {
    }

    internal void ConnectionHandler(IPEndPoint endpoint)
    {
        link.Send(endpoint, new LobbyRequest { lobby = lobby });
        game.connectionState = GameController.ConnectionState.RequestingLobby;
        game.timeout = game.time + 2000000000;
    }
}
