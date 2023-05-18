using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class TimerComponent : MonoBehaviour
    {
        [SerializeField] private TimerData[] _timers;
        [SerializeField] private bool _callAtStartFromFirst;

        private void Start()
        {
            if (_callAtStartFromFirst)
            {
                if (_timers != null)
                {
                    var timer = _timers[0];
                    StartCoroutine(StartTimer(timer));
                }
            }

        }
        public void SetTimer(int index)
        {
            var timer = _timers[index];

            StartCoroutine(StartTimer(timer));
        }

        private IEnumerator StartTimer(TimerData timer)
        {
            yield return new WaitForSeconds(timer.Delay);

            timer.OnTimesUp?.Invoke();
        }

        [Serializable]
        public class TimerData
        {
            public float Delay;
            public UnityEvent OnTimesUp;
        }
    }
}


