using UnityEngine;
using JetBrains.Annotations;
using TMPro;


public class CreateGameScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	public delegate void CreateGameActionDelegate(string password);
	private MenuActionDelegate _onBackButton;
	private CreateGameActionDelegate _onCreateGameButton;

	[SerializeField] private TMP_Text _ipText;
	[SerializeField] private TMP_Text _passwordText;

	public void SetBackButtonAction(MenuActionDelegate action)
	{
		_onBackButton = action;
	}

	public void SetCreateGameButtonAction(CreateGameActionDelegate action)
	{
		_onCreateGameButton = action;
	}

	public void SetIpAddress(string ipAddress)
	{
		_ipText.text = ipAddress;
	}

	[UsedImplicitly]
	public void OnBackButton()
	{
		_onBackButton();
	}

	[UsedImplicitly]
	public void OnCreateGame()
	{	
		_onCreateGameButton(_passwordText.text);
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