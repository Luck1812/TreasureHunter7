using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Data/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private StringPersistentProperty _nickname;
        [SerializeField] private FloatPersistentProperty _music;
        [SerializeField] private FloatPersistentProperty _sfx;
        public StringPersistentProperty Nickname => _nickname;
        public FloatPersistentProperty Music => _music;
        public FloatPersistentProperty Sfx => _sfx;

        private static GameSettings _instance;
        public static GameSettings Instance => _instance == null ? LoadGameSettings() : _instance;
        private static GameSettings LoadGameSettings()
        {
            return _instance = Resources.Load<GameSettings>("GameSettings");
        }
        private void OnEnable()
        {
            _music = new FloatPersistentProperty(0f, SoundSettings.Music.ToString());
            _sfx = new FloatPersistentProperty(0.5f, SoundSettings.Sfx.ToString());
            _nickname = new StringPersistentProperty("Player", "Nickname");
        }

        private void OnValidate()
        {
            Music.Validate();
            Sfx.Validate();
            Nickname.Validate();
        }
        
    }
    public enum SoundSettings
    {
        Music,
        Sfx
    }
    
}