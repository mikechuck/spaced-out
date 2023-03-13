using UnityEngine;
using JetBrains.Annotations;
using TMPro;

public class ToastMessage : MonoBehaviour
{
	[SerializeField] private TMP_Text _toastText;
	[SerializeField] private GameObject _background;
	[SerializeField] private float _xMarginSize;
	[SerializeField] private float _toastHeight;

	public string ToastText
	{
		get
		{
			return _toastText.text;
		}
		set
		{
			_toastText.text = value;
		}
	}
}