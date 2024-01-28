using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // Instance of MicroInfinity sound effect
    // ****************************************************************************************************
    public class MicroInfinityInstance {
        // Variables
        MicroInfinitySoundGroup _infiniteGroup;
        AudioMixerGroup _mixerGroup;

        // Timer
        float _timer;
        float _nextRandomSound;

        // Play variables
        AudioSource _loopAudioSource;
        AudioSource _startAudioSource;
        AudioSource _endAudioSource;
        List<AudioSource> _randomsAudioSource;

        internal MicroInfinityInstance(MicroInfinitySoundGroup infiniteGroup, AudioMixerGroup mixerGroup) {
            _infiniteGroup = infiniteGroup;
            _mixerGroup = mixerGroup;

            if(_infiniteGroup.AmountOfRandomClips < 1) _timer = -1f;

            // Play variables
            _randomsAudioSource = new List<AudioSource>();
        }        

        // Events
        internal event Func<AudioClip, AudioMixerGroup, float, float, AudioSource> RequestPlaySound;
        internal event Action<AudioSource> RequestUnreserveSound;

        public event Action<AudioSource> OnPlaySound;
        public event Action<MicroInfinityInstance> OnCancel;
        public event Action<MicroInfinityInstance> OnFinish;

        void Destroy() {
            RequestPlaySound = null;
            RequestUnreserveSound = null;

            OnPlaySound = null;
            OnCancel = null;
            OnFinish = null;
        }

        #region Control
        internal void Play() {
            MicroAudioDebugger.StartInfinityGroup();
            
            PlayStartSound();
            
            _loopAudioSource = PlaySound(_infiniteGroup.LoopClip, _infiniteGroup.LoopVolume, _infiniteGroup.LoopPitch);
            if(_loopAudioSource != null) {
                _loopAudioSource.loop = true;
                _loopAudioSource.pitch = 0.6f;
                MicroAudioDebugger.StartInfinityLoop(_loopAudioSource);
            }            

            // Determine how will random sound play
            if(_startAudioSource == null) {
                if(_infiniteGroup.DelayFirstRandomClip) {
                    SetNewRandomClipTime(0f);
                }
                else {
                    _nextRandomSound = 0f;
                }
            }
        }
        public void Cancel() {
            StopAllSounds();
            OnCancel?.Invoke(this);
            MicroAudioDebugger.CancelInfinityGroup();
            Destroy();
        }
        public void Finish() {
            PlayEndSound();
            StopAllSounds();
            OnFinish?.Invoke(this);
            MicroAudioDebugger.FinishInfinityGroup();
            Destroy();
        }
        AudioSource PlaySound(AudioClip clip, float volume, float pitch) {
            AudioSource source = RequestPlaySound?.Invoke(clip, _mixerGroup, volume, pitch);
            if(source != null) OnPlaySound?.Invoke(source);
            return source;
        }
        void StopAllSounds() {
            if(_loopAudioSource != null) {
                _loopAudioSource.Stop();
                RequestUnreserveSound?.Invoke(_loopAudioSource);
            }
            if(_startAudioSource != null) {
                _startAudioSource.Stop();
                RequestUnreserveSound?.Invoke(_startAudioSource);
            }
            foreach(AudioSource x in _randomsAudioSource) {
                if(x != null) {
                    x.Stop();
                    RequestUnreserveSound?.Invoke(x);
                }
            }
            if(_endAudioSource != null) RequestUnreserveSound?.Invoke(_endAudioSource);
        }
        #endregion

        #region Sounds
        void PlayRandomSound() {
            AudioSource source = PlaySound(_infiniteGroup.GetRandomClip, _infiniteGroup.RandomVolume, _infiniteGroup.RandomPitch);
            if(source == null) return;

            _randomsAudioSource.Add(source);
            SetNewRandomClipTime(GetSourceLength(source));
            ClearEmptyAudioSources();
            MicroAudioDebugger.RandomInfinitySound(source);
        }
        void PlayStartSound() {
            if(_infiniteGroup.StartClip == null) return;
            _startAudioSource = PlaySound(_infiniteGroup.StartClip, _infiniteGroup.StartVolume, _infiniteGroup.StartPitch);
            SetNewRandomClipTime(GetSourceLength(_startAudioSource));
            MicroAudioDebugger.StartInfinitySound(_startAudioSource);
        }
        void PlayEndSound() {
            if(_infiniteGroup.EndClip == null) return;
            _endAudioSource = PlaySound(_infiniteGroup.EndClip, _infiniteGroup.EndVolume, _infiniteGroup.EndPitch);
            MicroAudioDebugger.EndInfinitySound(_endAudioSource);
        }
        #endregion

        #region Lifetime
        internal void Update() {
            if(_timer == -1) return;
            _timer += Time.deltaTime;

            if(_timer >= _nextRandomSound) {
                PlayRandomSound();
            }
        }
        void ClearEmptyAudioSources() {
            for(int i = 0; i < _randomsAudioSource.Count; i++) {
                if(!_randomsAudioSource[i].isPlaying) {
                    RequestUnreserveSound?.Invoke(_randomsAudioSource[i]);
                    _randomsAudioSource.RemoveAt(i);
                    i--;
                }
            }
        }
        void SetNewRandomClipTime(float previousClipLength) {
            _nextRandomSound = 
                _timer +
                previousClipLength +
                UnityEngine.Random.Range(_infiniteGroup.TimeBetweenClips[0], _infiniteGroup.TimeBetweenClips[1]);
        }
        float GetSourceLength(AudioSource src) {
            if(src == null || src.clip == null) return 0f;
            return src.clip.length / Mathf.Abs(src.pitch);
        }
        #endregion
    }
}