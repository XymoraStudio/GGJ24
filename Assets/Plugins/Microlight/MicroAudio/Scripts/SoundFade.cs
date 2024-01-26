using System;
using UnityEngine;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // Fades sound over time
    // ****************************************************************************************************
    public class SoundFade {
        readonly AudioSource _source;
        readonly float _startVolume;
        readonly float _endVolume;
        readonly float _overSeconds;
        float _timer;

        public SoundFade(AudioSource src, float startVolume, float endVolume, float overSeconds, bool isPlaying = true) {
            _source = src;
            _startVolume = Mathf.Clamp01(startVolume);
            _endVolume = Mathf.Clamp01(endVolume);
            _overSeconds = overSeconds;

            _timer = 0;
            _isPlaying = isPlaying;

            MicroAudio.UpdateEvent += Update;
            MicroAudioDebugger.FadeCreated(this);
        }

        // Properties
        bool _isPlaying;
        public bool IsPlaying {
            get => _isPlaying;
            set {
                if(_isPlaying == value) return;
                _isPlaying = value;
                OnPlayingChange?.Invoke(this);
                MicroAudioDebugger.FadePlayingChanged(this);
            }
        }        

        // Events
        public event Action<SoundFade> OnFadeEnd;   // Played when fade reaches end
        public event Action<SoundFade> OnPlayingChange;
        public event Action<SoundFade> Destroy;   // Played every time fade finishes (even if killed)

        // Public methods
        public AudioSource Source => _source;
        public float StartVolume => _startVolume;
        public float EndVolume => _endVolume;
        public float OverSeconds => _overSeconds;
        public float Progress => Mathf.Clamp01(_timer / _overSeconds);
        public void Kill() => FinishFade(false);
        public void Skip() => FinishFade(true);

        // Private methods
        void Update() {
            if(!_isPlaying) return;

            _timer += Time.deltaTime;
            if(_timer >= _overSeconds) {
                FinishFade(true);
            }
            else {
                _source.volume = Mathf.Clamp01(Mathf.Lerp(_startVolume, _endVolume, _timer/_overSeconds));
            }
        }        
        void FinishFade(bool setEndValue) {
            _isPlaying = false;
            if(setEndValue) {
                _source.volume = _endVolume;
                OnFadeEnd?.Invoke(this);
                MicroAudioDebugger.FadeEnded(this);
            }
            
            Destroy?.Invoke(this);
            MicroAudio.UpdateEvent -= Update;
            OnFadeEnd = null;
            OnPlayingChange = null;
            Destroy = null;
        }
    }
}