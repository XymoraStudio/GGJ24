using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // Sounds module for MicroAudio
    // ****************************************************************************************************
    internal class MicroSounds {
        // Sources
        Transform _soundsContainer;
        readonly List<AudioSource> _soundAudioSources;
        readonly List<AudioSource> _reservedSoundAudioSources;   // List of audio sources which might not be playing right now BUT are needed for other uses
        readonly List<DelayedSound> _delayedSounds;   // List of delayed sounds

        readonly int MAX_SOUND_SOURCES;
        readonly int MAX_INSTANCES_OF_SAME_CLIP;

        internal MicroSounds(Transform soundsContainer, int maxSoundSources, int maxInstancesOfSameClip) {
            // Sources
            _soundsContainer = soundsContainer;
            _soundAudioSources = new List<AudioSource>();
            _reservedSoundAudioSources = new List<AudioSource>();
            _delayedSounds = new List<DelayedSound>();

            MAX_SOUND_SOURCES = maxSoundSources;
            MAX_INSTANCES_OF_SAME_CLIP = maxInstancesOfSameClip;
        }

        // Internal methods
        internal AudioSource PlaySound(AudioClip clip, AudioMixerGroup group, AudioSource src, float delay, float volume, float pitch) {
            if(clip == null) {
                MicroAudioDebugger.SoundClipEmpty();
                return null;
            }
            if(group == null) {
                MicroAudioDebugger.SoundClipEmpty();
                return null;
            }
            if(src == null) src = GetFreeSoundSource(clip);
            if(src == null) return null;
            src.clip = clip;
            src.volume = Mathf.Clamp01(volume);
            src.outputAudioMixerGroup = group;
            src.loop = false;
            src.pitch = pitch;

            // Play
            if(delay > 0f) {
                DelaySoundEffect(src, delay);
            }
            else {
                src.Play();
                MicroAudioDebugger.PlaySound(src);
            }

            return src;
        }
        internal DelayedSound GetDelayStatusOfSound(AudioSource src) {
            foreach(DelayedSound x in _delayedSounds) {
                if(x.Source == src) return x;
            }
            return null;
        }

        // Private methods       
        AudioSource GetFreeSoundSource(AudioClip clipToPlay) {
            // Check for too many of same clip
            if(MAX_INSTANCES_OF_SAME_CLIP > 0) {
                int sameClipCount = 0;
                foreach(AudioSource x in _soundAudioSources) {
                    if(x.clip == clipToPlay && x.isPlaying) sameClipCount++;
                    if(sameClipCount >= MAX_INSTANCES_OF_SAME_CLIP) {
                        MicroAudioDebugger.TooManySameClips();
                        return null;
                    }
                }
            }

            // Free source
            foreach(AudioSource x in _soundAudioSources) {
                if(!x.isPlaying && !IsAudioSourceReserved(x)) return x;   // We found free sound source
            }

            // If there is no available sources, try to create new
            if(_soundAudioSources.Count >= MAX_SOUND_SOURCES && MAX_SOUND_SOURCES > 0) {
                MicroAudioDebugger.CantCreateNewSources();
                return null;
            }
            return CreateNewSoundSource();
        }
        AudioSource CreateNewSoundSource() {
            GameObject obj = new GameObject($"SoundPlayer {_soundAudioSources.Count}", typeof(AudioSource));
            obj.transform.SetParent(_soundsContainer);
            AudioSource src = obj.GetComponent<AudioSource>();
            _soundAudioSources.Add(src);

            MicroAudioDebugger.SoundSourceCreated(src);
            return src;
        }
        DelayedSound DelaySoundEffect(AudioSource src, float delay) {
            DelayedSound delayedSound = new DelayedSound(src, delay);
            _delayedSounds.Add(delayedSound);
            _reservedSoundAudioSources.Add(src);
            delayedSound.Destroy += RemoveDelayedSound;
            return delayedSound;
        }
        void RemoveDelayedSound(DelayedSound delayedSound) {
            _reservedSoundAudioSources.Remove(delayedSound.Source);
            _delayedSounds.Remove(delayedSound);
        }
        internal void ReserveSoundSource(AudioSource src) {
            if(src != null) _reservedSoundAudioSources.Add(src);
        }
        internal void UnreserveSoundSource(AudioSource src) {
            if(src != null) _reservedSoundAudioSources.Remove(src);
        }
        bool IsAudioSourceReserved(AudioSource src) {
            foreach(AudioSource x in _reservedSoundAudioSources) {
                if(src == x) return true;
            }
            return false;
        }
    }
}