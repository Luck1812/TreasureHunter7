using System;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizeText : AbstractLocalizeComponent
    {
        [SerializeField] private string _key;
        [SerializeField] private TextRegime _textRegime;

        public string Key => _key;
        private Text _text;
        protected override void Awake()
        {
            _text = GetComponent<Text>();
            base.Awake();
        }
        protected override void Localize()
        {
            var localized = LocalizationManager.Instance.Localize(_key);
            GetRegime(localized);
        }
        public void GetRegime(string localized)
        {
            switch (_textRegime)
                {
                    case TextRegime.Capitalize:
                        _text.text = localized.ToUpper();
                        break;
                    case TextRegime.UpFirstLetter:
                        _text.text = ToUpperFirstLetter(localized);
                        break;
                    case TextRegime.PlainText:
                        _text.text = localized;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }
        public string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            
            char[] letters = source.ToCharArray();
            
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }

        public enum TextRegime
        {
            Capitalize,
            UpFirstLetter,
            PlainText
        }
    }
}