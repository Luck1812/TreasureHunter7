using System;
using PixelCrew.UI.LevelsLoader;

namespace PixelCrew.UI.Windows.Level
{
    public class LevelSelectionWindow : AnimatedWindow
    {
        private Action _closeAction;

        public void OnStartLevel(string level)
        {
            _closeAction = () =>
            {
                var levelLoader = FindObjectOfType<LevelLoader>();
                levelLoader.LevelLoad(level);
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