using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // Manager for playing MicroInfitnity sound effects
    // ****************************************************************************************************
    internal class MicroInfinitySounds {
        readonly MicroSounds _microSounds;
        readonly List<MicroInfinityInstance> _instanceList;

        internal MicroInfinitySounds(MicroSounds microSounds) { 
            _microSounds = microSounds;
            _instanceList = new List<MicroInfinityInstance>();

            MicroAudio.UpdateEvent += Update;
        }

        void Update() {
            foreach (MicroInfinityInstance instance in _instanceList) {
                instance.Update();
            }
        }

        internal MicroInfinityInstance PlayInfinitySound(MicroInfinitySoundGroup infinityGroup, AudioMixerGroup mixerGroup, float volume) {
            MicroInfinityInstance newInstance = new MicroInfinityInstance(infinityGroup, mixerGroup);
            newInstance.RequestPlaySound += PlaySound;
            newInstance.RequestUnreserveSound += UnreserveSound;
            newInstance.OnCancel += FinishGroup;
            newInstance.OnFinish += FinishGroup;
            newInstance.Play();
            _instanceList.Add(newInstance);
            return newInstance;
        }
        AudioSource PlaySound(AudioClip clip, AudioMixerGroup mixerGroup, float volume, float pitch) {
            AudioSource src = _microSounds.PlaySound(clip, mixerGroup, null, 0f, volume, pitch);
            if(src != null) _microSounds.ReserveSoundSource(src);
            return src;
        }
        void UnreserveSound(AudioSource src) {
            _microSounds.UnreserveSoundSource(src);
        }
        void FinishGroup(MicroInfinityInstance instance) {
            _instanceList.Remove(instance);
        }
    }
}