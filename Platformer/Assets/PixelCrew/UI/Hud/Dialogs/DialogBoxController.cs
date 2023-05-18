using System;
using System.Collections;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [Space][SerializeField] protected DialogContent _content;
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;
        
        [Space] [SerializeField] private float _textSpeed = 0.09f;

        [Header("Sounds")] [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private AudioSource _sfxSource;
        private DialogData _data;
        private Coroutine _typingRoutine;
        private UnityEvent _onComplete;
        private int _currentSentences;
    
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    
        protected virtual DialogContent CurrentContent => _content;

        protected Sentence CurrentSentence => _data.Sentences[_currentSentences];
        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
        }
        public void OnSkip()
        {
            if (_typingRoutine == null) return;

            StopTypeAnimation();
            CurrentContent.Text.text = LocalizationManager.Instance.Localize(_data.Sentences[_currentSentences].Value);
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentences++;

            var isDialogCompleted = _currentSentences >= _data.Sentences.Length;
            if (isDialogCompleted)
            {
                HideDialogBox();
                _onComplete?.Invoke();
            }
            else
            {
                OnStartDialogAnimation();
            }
        
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
        }

        protected virtual void OnStartDialogAnimation()
        {
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        private IEnumerator TypeDialogText()
        {
            CurrentContent.Text.text = String.Empty;
            var sentences = _data.Sentences[_currentSentences];
            CurrentContent.TrySetIcon(sentences.Icon);
            var localizeText = LocalizationManager.Instance.Localize(sentences.Value);
            foreach (var symbol in localizeText)
            {
                CurrentContent.Text.text += symbol;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }
        }
        private void OnCloseDialogAnimation()
        {
            _container.SetActive(false);
        }
    
        public void ShowDialog(DialogData data, UnityEvent onComplete)
        {
            _data = data;
            _onComplete = onComplete;
            _currentSentences = 0;
            CurrentContent.Text.text = string.Empty;
        
            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }
    
    }
}
