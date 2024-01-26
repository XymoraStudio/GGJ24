using System;
using UnityEngine;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // Delays playing of sound
    // ****************************************************************************************************
    public class DelayedSound {
        readonly AudioSource _source;
        readonly float _delay;
        float _timer;

        public DelayedSound(AudioSource src, float delay) {
            _source = src;
            _delay = delay;

            _timer = 0;

            MicroAudio.UpdateEvent += Update;
            MicroAudioDebugger.StartDelayedSound(this);
        }

        // Events
        public event Action<DelayedSound> OnDelayEnd;   // Called only when it reaches end
        public event Action<DelayedSound> Destroy;   // Event which is always called when finishing delay

        // Public methods
        public AudioSource Source => _source;
        public float Delay => _delay;
        public float Progress => Mathf.Clamp01(_timer / _delay);
        public void Kill() => FinishDelay(false);
        public void Skip() => FinishDelay(true);
        public void ResetTimer() => _timer = 0;

        // Private methods
        void Update() {
            if(_timer >= _delay) return;

            _timer += Time.deltaTime;
            if(_timer >= _delay) {
                FinishDelay(true);
            }
        }
        void FinishDelay(bool playSound) {
            _timer = _delay;
            if(playSound) {
                _source.Play();
                OnDelayEnd?.Invoke(this);
                MicroAudioDebugger.PlayDelayedSound(this);
            }

            Destroy?.Invoke(this);
            MicroAudio.UpdateEvent -= Update;
            OnDelayEnd = null;
            Destroy = null;
        }

        
    }
}