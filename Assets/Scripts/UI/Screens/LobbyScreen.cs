using UnityEngine;
using JetBrains.Annotations;

public class LobbyScreen : MonoBehaviour
{
	public delegate void MenuActionDelegate();
	private MenuActionDelegate _onBackButton;

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