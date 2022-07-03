using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Photon.Pun.Demo.PunBasics
{

	[RequireComponent(typeof(TMP_InputField))]
	public class PlayerNameInputField : MonoBehaviour
	{

		const string _playerNamePrefKey = "PlayerName";

		void Start()
		{

			string defaultName = string.Empty;
			InputField _inputField = this.GetComponent<InputField>();

			if (_inputField != null)
			{
				if (PlayerPrefs.HasKey(_playerNamePrefKey))
				{
					defaultName = PlayerPrefs.GetString(_playerNamePrefKey);
					_inputField.text = defaultName;
				}
			}

			PhotonNetwork.NickName = defaultName;
		}

		public void SetPlayerName(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				value = "brawlker";
			}
			PhotonNetwork.NickName = value;

			PlayerPrefs.SetString(_playerNamePrefKey, value);
		}
	}
}
