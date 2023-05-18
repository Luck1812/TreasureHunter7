using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Components.Audio
{
    public class AudioSettingComponent : MonoBehaviour
    {
        [SerializeField] private SoundSettings _mode;
        private AudioSource _source;
        private FloatPersistentProperty _model;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private void Start()
        {
            _source = GetComponent<AudioSource>();

            _model = FindProperty();
            _trash.Retain(_model.Subscribe(OnSoundSettingChanged));
            OnSoundSettingChanged(_model.Value, _model.Value);
        }

        private void OnSoundSettingChanged(float newValue, float oldValue)
        {
            _source.volume = newValue;
        }

        private FloatPersistentProperty FindProperty()
        {
            switch (_mode)
            {
                case SoundSettings.Music:
                    return GameSettings.Instance.Music;
                case SoundSettings.Sfx:
                    return GameSettings.Instance.Sfx;
            }

            throw new ArgumentException("Undefined mode");
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}