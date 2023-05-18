using PixelCrew.Model;
using UnityEngine;
using PixelCrew.UI.LevelsLoader;

namespace PixelCrew.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();
            session.Save();
            
            var levelLoader = FindObjectOfType<LevelLoader>();
            levelLoader.LevelLoad(_sceneName);
        }
    }
}
