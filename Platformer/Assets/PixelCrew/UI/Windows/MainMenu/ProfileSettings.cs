using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.UI.Windows.MainMenu
{
    public class ProfileSettings : MonoBehaviour
    {
        [SerializeField] private NicknameWidget _nicknameWidget;

        private void Start()
        {
            _nicknameWidget.SetModel(GameSettings.Instance.Nickname);
        }

        public void SaveNickname()
        {
            GameSettings.Instance.Nickname.Value = _nicknameWidget.Text;
        }
    }
}