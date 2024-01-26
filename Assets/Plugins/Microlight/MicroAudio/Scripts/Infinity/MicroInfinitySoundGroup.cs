using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // ScriptableObject used for storing data about infinite sound group
    // ****************************************************************************************************
    [CreateAssetMenu(fileName = "MicroInfinitySoundGroup", menuName = "Microlight/Micro Audio/Infinity Sound Group")]
    public class MicroInfinitySoundGroup : ScriptableObject {
        [SerializeField] AudioClip _loopClip;
        [SerializeField][Range(0f, 1f)] float _loopVolume = 1f;
        [SerializeField][Range(-3f, 3f)] float _loopPitch = 1f;
        [SerializeField] AudioClip _startClip;
        [SerializeField][Range(0f, 1f)] float _startVolume = 1f;
        [SerializeField][Range(-3f, 3f)] float _startPitch = 1f;
        [SerializeField] AudioClip _endClip;
        [SerializeField][Range(0f, 1f)] float _endVolume = 1f;
        [SerializeField][Range(-3f, 3f)] float _endPitch = 1f;
        [SerializeField] List<AudioClip> _randomClips;
        [SerializeField][Range(0f, 1f)] float _randomVolume = 1f;
        [SerializeField][Range(-3f, 3f)] float _randomPitch = 1f;

        [Header("Settings")]
        [SerializeField] float _minTimeBetweenRandomClips;
        [SerializeField] float _maxTimeBetweenRandomClips;
        [SerializeField] bool _delayFirstRandomClip;

        public AudioClip LoopClip => _loopClip;
        public float LoopVolume => _loopVolume;
        public float LoopPitch => _loopPitch;
        public AudioClip StartClip => _startClip;
        public float StartVolume => _startVolume;
        public float StartPitch => _startPitch;
        public AudioClip EndClip => _endClip;
        public float EndVolume => _endVolume;
        public float EndPitch => _endPitch;
        public float RandomVolume => _randomVolume;
        public float RandomPitch => _randomPitch;
        public float[] TimeBetweenClips => new float[2] { _minTimeBetweenRandomClips, _maxTimeBetweenRandomClips };
        public int AmountOfRandomClips => _randomClips.Count;
        public bool DelayFirstRandomClip => _delayFirstRandomClip;
        public AudioClip GetRandomClip {
            get {
                if(_randomClips.Count < 1) {
                    MicroAudioDebugger.RandomClipListEmpty();
                    return null;
                }
                return _randomClips[Random.Range(0, _randomClips.Count - 1)];
            }
        }
    }
}