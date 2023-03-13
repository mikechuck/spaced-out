using UnityEngine;
using JetBrains.Annotations;

public class MainMenuScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	private MenuActionDelegate _onJoinGameButton;
	private MenuActionDelegate _onCreateGameButton;

    public void SetJoinGameAction(MenuActionDelegate action)
	{
		_onJoinGameButton = action;
	}

    public void SetCreateGameAction(MenuActionDelegate action)
	{
		_onCreateGameButton = action;
	}

	[UsedImplicitly]
	public void OnJoinGameButton()
	{
		_onJoinGameButton();
	}
	
	[UsedImplicitly]
	public void OnCreateGameButton()
	{
		_onCreateGameButton();
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
