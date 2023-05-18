using System;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class OptionDialogController : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private Text _contentText;
        [SerializeField] private Transform _optionContainer;
        [SerializeField] private OptionItemWidget _prefab;

        private DataGroup<OptionData, OptionItemWidget> _dataGroup;
        private void Start()
        {
            _dataGroup =  new DataGroup<OptionData, OptionItemWidget>(_prefab, _optionContainer);
        }

        public void OnOptionsSelected(OptionData selectedOption)
        {
            selectedOption.OnSelect.Invoke();
            _content.SetActive(false);
            PlayerInputUtils.SetActive(true);

        }
        public void Show(OptionDialogData data) 
        {
            _content.SetActive(true);
            PlayerInputUtils.SetActive(false);
            _contentText.text = LocalizationManager.Instance.Localize(data.DialogText);
            
            _dataGroup.SetData(data.Options);
        }
    }
    
    
    [Serializable]
    public class OptionDialogData
    {
        public string DialogText;
        public OptionData[] Options;
    }
    [Serializable]
    public class OptionData
    {
        public string Text;
        public UnityEvent OnSelect;
    }
}