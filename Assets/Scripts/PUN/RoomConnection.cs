using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomConnection : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _feedbackText;
    [SerializeField] private GameObject _controlPanel;
	[SerializeField] private byte _maxPlayersPerRoom = 4;

	private bool _isConnecting = false;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

	public void Connect()
	{
		_feedbackText.text = "";
		_isConnecting = true;
		_controlPanel.SetActive(false);
		
        if (PhotonNetwork.IsConnected)
		{
			LogFeedback("Joining Room...");
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{

			LogFeedback("Connecting...");

			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = Application.version;
		}
	}

    void LogFeedback(string message)
    {
        if (_feedbackText == null) return;

        _feedbackText.text += System.Environment.NewLine + message;
    }

	public override void OnConnectedToMaster()
	{
		if (_isConnecting)
		{
			LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
			Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
			PhotonNetwork.JoinRandomRoom();
		}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
		Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this._maxPlayersPerRoom });
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);
		Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");
		_isConnecting = false;
		_controlPanel.SetActive(true);

	}

	public override void OnJoinedRoom()
	{
		LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
		Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

		if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
		{
			Debug.Log("Joining Room");

			PhotonNetwork.LoadLevel("FirstMap");

		}
	}
}
