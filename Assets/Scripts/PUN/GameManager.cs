﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

//#pragma warning disable 649

public class GameManager : MonoBehaviourPunCallbacks
{

	#region Public Fields

	static public GameManager Instance;

	#endregion

	#region Private Fields

	private GameObject instance;

    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
	{
		Instance = this;

		// in case we started this demo with the wrong scene being active, simply load the menu scene
		if (!PhotonNetwork.IsConnected)
		{
			PhotonNetwork.LoadLevel("Menu");

			return;
		}
		if (playerPrefab == null) { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

			//Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
		} else {

			if (PlayerChar.LocalPlayerInstance==null)
			{
				//Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity, 0);
			}else{

				//Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
			}


		}

	}

    #endregion

    #region Photon Callbacks

    /// <summary>
    /// Called when a Photon Player got connected. We need to then load a bigger scene.
    /// </summary>
    /// <param name="other">Other.</param>
    public override void OnPlayerEnteredRoom( Player other  )
	{
		//Debug.Log( "OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

		if ( PhotonNetwork.IsMasterClient )
		{
			//Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

			LoadArena();
		}
	}

	/// <summary>
	/// Called when a Photon Player got disconnected. We need to load a smaller scene.
	/// </summary>
	/// <param name="other">Other.</param>
	public override void OnPlayerLeftRoom( Player other  )
	{
		//Debug.Log( "OnPlayerLeftRoom() " + other.NickName ); // seen when other disconnects

		if ( PhotonNetwork.IsMasterClient )
		{
			//Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

			LoadArena(); 
		}
	}

	/// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom()
	{
		PhotonNetwork.LoadLevel("Menu");
	}

	#endregion

	#region Public Methods

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

	#endregion

	#region Private Methods

	void LoadArena()
	{
		if ( ! PhotonNetwork.IsMasterClient )
		{
			//Debug.LogError( "PhotonNetwork : Trying to Load a level but we are not the master Client" );
		}

	}

	#endregion

}
