using UnityEngine;
using JetBrains.Annotations;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using TMPro;

public class CreateGameScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	private MenuActionDelegate _onBackButton;
	private string _userIpAddress;
	[SerializeField] private TMP_Text _ipText;

	private void Awake()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
         {
             if (ip.AddressFamily == AddressFamily.InterNetwork)
             {
                 _userIpAddress = ip.ToString();
				 _ipText.text = _userIpAddress;
				 break;
             }
         }
	}

	public void SetBackButtonAction(MenuActionDelegate action)
	{
		_onBackButton = action;
	}

	[UsedImplicitly]
	public void OnBackButton()
	{
		_onBackButton();
	}
	public void Show()
	{
		if (!this) return;
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		if (!this) return;
		gameObject.SetActive(false);
	}
}