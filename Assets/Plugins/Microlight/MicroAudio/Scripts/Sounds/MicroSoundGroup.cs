using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // ScriptableObject used for storing sound group
    // ****************************************************************************************************
    [CreateAssetMenu(fileName = "MicroSoundGroup", menuName = "Microlight/Micro Audio/Sound Group")]
    public class MicroSoundGroup : ScriptableObject {
        [SerializeField] List<AudioClip> _clipList;
        public List<AudioClip> ClipList => _clipList;
        public AudioClip GetRandomClip {
            get {
                if(_clipList.Count < 1) {
                    MicroAudioDebugger.RandomClipListEmpty();
                    return null;
                }
                return _clipList[Random.Range(0, _clipList.Count - 1)];
            }
        }
    }
}