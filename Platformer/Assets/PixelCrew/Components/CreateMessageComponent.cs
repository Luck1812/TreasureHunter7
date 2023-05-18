using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.Components
{
    public class CreateMessageComponent : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _messageObject;
        [SerializeField] private string _messageKey;
        [SerializeField] private int _maxCountMessage;

        private Text _localizeText;

        private void Start()
        {
            _localizeText = _messageObject.GetComponentInChildren<Text>();
            if (_localizeText != null)
                _localizeText.text = LocalizationExtensions.Localize(_messageKey);
        }

        public void CreateMessage()
        {
            if(_container.childCount >= _maxCountMessage)
                return;

            Instantiate(_messageObject, _container.position, Quaternion.identity, _container);
        }
    }
}
