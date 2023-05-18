using System;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.Windows.GameMenu
{
    [RequireComponent(typeof(ReloadLevelComponent))]
    public class GameMenuWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        private Action _closeAction;

        protected override void Start()
        {
            base.Start();
            
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnRestartGame()
        {
            var reload = GetComponent<ReloadLevelComponent>();
            _closeAction = () => { reload.Reload(); };

            Close();
        }

        public void OnExit()
        {
            _closeAction = () => { SceneManager.LoadScene("MainMenu"); };
            var session = FindObjectOfType<GameSession>();
            Destroy(session);
            Close();
        }

        protected override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}