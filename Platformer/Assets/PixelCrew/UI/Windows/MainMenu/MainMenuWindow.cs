using System;
using PixelCrew.UI.LevelsLoader;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnShowShop()
        {
            WindowUtils.CreateWindow("UI/ClothingStoreWindow");
        }

        public void OnShowLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }

        public void OnShowLevelSelection()
        {
            WindowUtils.CreateWindow("UI/LevelSelectionWindow");
        }

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var levelLoader = FindObjectOfType<LevelLoader>();
                levelLoader.LevelLoad("Level1");
            };

            Close();
        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        protected override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}