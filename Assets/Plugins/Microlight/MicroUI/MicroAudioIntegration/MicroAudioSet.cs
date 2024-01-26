using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Stores audio clips used by UI elements
    // ****************************************************************************************************
    [CreateAssetMenu(fileName = "MicroAudioSet", menuName = "Microlight/Micro UI/Audio Set")]
    public class MicroAudioSet : ScriptableObject {
        [SerializeField] AudioClip _hover;
        [SerializeField][Range(0f, 1f)] float _hoverVolume = 1f;
        [SerializeField] AudioClip _click;
        [SerializeField][Range(0f, 1f)] float _clickVolume = 1f;

        public AudioSource PlayHover() => MicroAudio.MicroAudio.PlayUISound(_hover, 0f, _hoverVolume);
        public AudioSource PlayClick() => MicroAudio.MicroAudio.PlayUISound(_click, 0f, _clickVolume);
    }
}