using PixelCrew.Model.Data.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    [RequireComponent(typeof(InputField))]
    public class NicknameWidget : MonoBehaviour
    {
        [SerializeField] private InputField _inputField;
        public string Text => _inputField.text; 

        public void SetModel(StringPersistentProperty model)
        {
            _inputField.text = model.Value;
        }
    }
}