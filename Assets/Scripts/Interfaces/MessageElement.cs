using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Netologia.Quest.Interfaces
{
	/// <summary>
	/// Контроллер окошка с текстом рядом с персонажем
	/// </summary>
	public class MessageElement : MonoBehaviour
	{
		private RectTransform _transform;
		
		[SerializeField]
		private Image _backgroundDialog;
		[SerializeField] private Image _imageThoughts;
		[SerializeField]
		private TextMeshProUGUI _text;
		[SerializeField]
		private TextMeshProUGUI _textThoughts;

        [Space][SerializeField, Tooltip("Цвет фона для рабочего сообщения")]
		private Color _workBackgroundColor = Color.white;
		[SerializeField, Tooltip("Цвет фона для сообщения персонажа")]
		private Color _characterBackgroundColor = Color.yellow;

		public RectTransform Transform => _transform;

		public void WorkPush((Color Font, string Text) pair)
		{
            _backgroundDialog.gameObject.SetActive(false);
            _imageThoughts.gameObject.SetActive(true);

            _text.enabled = false;
			_textThoughts.enabled = true;
            _textThoughts.text = pair.Text;
		}

		public void CharacterPush((Color Font, string Text) pair)
		{
            _imageThoughts.gameObject.SetActive(false);
            _backgroundDialog.gameObject.SetActive(true);

            //_background.color = _characterBackgroundColor;
            _textThoughts.enabled = false;
            _text.enabled = true;
            _text.text = pair.Text;
			_text.color = pair.Font;
		}

		public void Enable() => gameObject.SetActive(true);
		public void Disable() => gameObject.SetActive(false);
		
		private void Awake()
		{
			_transform = transform as RectTransform;
		}
	}
}