using UnityEngine;
using JetBrains.Annotations;
using TMPro;

public class JoinGameScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	public delegate void JoinGameActionDelegate(string ipAddres, string password);
	private MenuActionDelegate _onBackButton;
	private JoinGameActionDelegate _onJoinGameButton;

	[SerializeField] private TMP_Text _ipText;
	[SerializeField] private TMP_Text _passwordText;

	public void SetBackButtonAction(MenuActionDelegate action)
	{
		_onBackButton = action;
	}

	public void SetJoinGameButtonAction(JoinGameActionDelegate action)
	{
		_onJoinGameButton = action;
	}

	[UsedImplicitly]
	public void OnBackButton()
	{
		_onBackButton();
	}

	[UsedImplicitly]
	public void OnJoinGame()
	{
		_onJoinGameButton(_ipText.text, _passwordText.text);
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