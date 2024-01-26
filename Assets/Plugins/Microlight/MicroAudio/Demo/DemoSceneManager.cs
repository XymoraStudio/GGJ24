using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Microlight.MicroAudio {
    public class DemoSceneManager : MonoBehaviour {
        [Header("UI")]
        [SerializeField] AudioClip _uiClip1;
        [SerializeField] AudioClip _uiClip2;
        [SerializeField] AudioClip _uiClipDelayed;
        [SerializeField] Slider _uiDelaySlider;

        [Header("SFX")]
        [SerializeField] AudioClip _sfxClip1;
        [SerializeField] AudioClip _sfxClip2;
        [SerializeField] AudioClip _sfxClipDelayed;
        [SerializeField] Slider _sfxDelaySlider;

        [Header("Music")]
        [SerializeField] AudioClip _musicLoopTrack;
        [SerializeField] MicroSoundGroup _musicGroup;
        [SerializeField] Toggle _shuffleToggle;
        [SerializeField] Slider _crossfadeSlider;

        [Header("Volume")]
        [SerializeField] Slider _masterVolumeSlider;
        [SerializeField] Slider _musicVolumeSlider;
        [SerializeField] Slider _sfxVolumeSlider;
        [SerializeField] Slider _uiVolumeSlider;
        [Space]
        [SerializeField] Text _masterVolumeText;
        [SerializeField] Text _musicVolumeText;
        [SerializeField] Text _sfxVolumeText;
        [SerializeField] Text _uiVolumeText;

        [Header("Music Status")]
        [SerializeField] Text _track1Text;
        [SerializeField] Text _track2Text;
        [SerializeField] Text _track3Text;
        [SerializeField] Slider _trackProgressSlider;
        [SerializeField] Slider _crossfadeProgressSlider;
        [SerializeField] Text _crossfadeFromTrack;
        [SerializeField] Text _crossfadeToTrack;
        SoundFade _crossfade;

        [Header("Misc")]
        [SerializeField] AudioClip _checkVolumeClip;
        DelayedSound _volumePingTest;   // Tests volume of sound effects when changing value
        const float _pingTestDelay = 0.25f;

        private void Start() {
            _masterVolumeSlider.value = MicroAudio.MasterVolume;
            _musicVolumeSlider.value = MicroAudio.MusicVolume;
            _sfxVolumeSlider.value = MicroAudio.SFXVolume;
            _uiVolumeSlider.value = MicroAudio.UIVolume;

            MicroAudio.OnNewPlaylist += UpdateTrackPlaylist;
            MicroAudio.OnMusicEnd += UpdatePlaylistStatus;
            MicroAudio.OnCrossfadeStart += CrossfadeStarted;
            MicroAudio.OnCrossfadeEnd += CrossfadeEnded;

            _crossfadeProgressSlider.gameObject.SetActive(false);
            _crossfadeFromTrack.gameObject.SetActive(false);
            _crossfadeToTrack.gameObject.SetActive(false);
        }

        private void Update() {
            _trackProgressSlider.value = MicroAudio.CurrentTrackProgress;

            if(_crossfade != null && _crossfade.IsPlaying) _crossfadeProgressSlider.value = _crossfade.Progress;
            else if(_crossfade != null && !_crossfade.IsPlaying) CrossfadeEnded(_crossfade);
        }

        #region UI Sounds Controls
        public void UIButton1() {
            MicroAudio.PlayUISound(_uiClip1);
        }
        public void UIButton2() {
            MicroAudio.PlayUISound(_uiClip2);
        }
        public void UIButtonDelay() {
            MicroAudio.PlayUISound(_uiClipDelayed, _uiDelaySlider.value);
        }
        #endregion

        #region SFX Sounds Controls
        public void SFXButton1() {
            MicroAudio.PlayUISound(_sfxClip1);
        }
        public void SFXButton2() {
            MicroAudio.PlayUISound(_sfxClip2);
        }
        public void SFXButtonDelay() {
            MicroAudio.PlayUISound(_sfxClipDelayed, _sfxDelaySlider.value);
        }
        #endregion

        #region Volume Controls
        public void OnMasterVolumeChange() {
            MicroAudio.MasterVolume = _masterVolumeSlider.value;
            MicroAudio.SaveSettings();
            _masterVolumeText.text = ((int)(_masterVolumeSlider.value * 100)).ToString();
        }
        public void OnMusicVolumeChange() {
            MicroAudio.MusicVolume = _musicVolumeSlider.value;
            MicroAudio.SaveSettings();
            _musicVolumeText.text = ((int)(_musicVolumeSlider.value * 100)).ToString();
        }
        public void OnSFXVolumeChange() {
            MicroAudio.SFXVolume = _sfxVolumeSlider.value;
            MicroAudio.SaveSettings();
            _sfxVolumeText.text = ((int)(_sfxVolumeSlider.value * 100)).ToString();
            StartVolumeTestPing(1);
        }
        public void OnUIVolumeChange() {
            MicroAudio.UIVolume = _uiVolumeSlider.value;
            MicroAudio.SaveSettings();
            _uiVolumeText.text = ((int)(_uiVolumeSlider.value * 100)).ToString();
            StartVolumeTestPing(0);
        }
        void StartVolumeTestPing(int layer) {
            // If already runing, reset
            if(_volumePingTest != null && _volumePingTest.Progress < 1f) {
                _volumePingTest.ResetTimer();
                return;
            }

            if(layer == 0) {
                AudioSource src = MicroAudio.PlayUISound(_checkVolumeClip, _pingTestDelay);
                _volumePingTest = MicroAudio.GetDelayStatusOfSound(src);
            }
            else {
                AudioSource src = MicroAudio.PlayEffectSound(_checkVolumeClip, _pingTestDelay);
                _volumePingTest = MicroAudio.GetDelayStatusOfSound(src);
            }
        }
        #endregion

        #region Music
        public void LoopOne() {
            MicroAudio.PlayOneTrack(_musicLoopTrack);
        }
        public void PlayMusicGroup() {
            MicroAudio.PlayMusicGroup(_musicGroup, _crossfadeSlider.value, _shuffleToggle.isOn);
        }
        public void NextTrack() {
            MicroAudio.NextTrack();
        }
        public void PreviousTrack() {
            MicroAudio.PreviousTrack();
        }
        #endregion

        #region MusicStatus
        void UpdateTrackPlaylist(List<int> playlist, MicroSoundGroup group) {
            _track1Text.text = group.ClipList[playlist[0]].name;
            _track2Text.text = group.ClipList[playlist[1]].name;
            _track3Text.text = group.ClipList[playlist[2]].name;
            UpdatePlaylistStatus();
        }
        void UpdatePlaylistStatus() {
            Debug.Log("Updating playlist status");
            _track1Text.color = _masterVolumeText.color;
            _track2Text.color = _masterVolumeText.color;
            _track3Text.color = _masterVolumeText.color;

            if(MicroAudio.ActiveMusicGroup == null) return;
            else if(MicroAudio.MusicCurrentPlaylistIndex == 0) _track1Text.color = Color.cyan;
            else if(MicroAudio.MusicCurrentPlaylistIndex == 1) _track2Text.color = Color.cyan;
            else if(MicroAudio.MusicCurrentPlaylistIndex == 2) _track3Text.color = Color.cyan;
        }
        void CrossfadeStarted(SoundFade fade) {
            // Display crossfade status
            _crossfadeProgressSlider.gameObject.SetActive(true);
            _crossfadeFromTrack.gameObject.SetActive(true);
            _crossfadeToTrack.gameObject.SetActive(true);

            _crossfade = fade;

            if(MicroAudio.CrossfadeMusicAudioSource.clip != null) _crossfadeFromTrack.text = MicroAudio.CrossfadeMusicAudioSource.clip.name;
            if(MicroAudio.MainMusicAudioSource.clip != null) _crossfadeToTrack.text = MicroAudio.MainMusicAudioSource.clip.name;
        }
        void CrossfadeEnded(SoundFade fade) {
            // Hide crossfade status
            _crossfadeProgressSlider.gameObject.SetActive(false);
            _crossfadeFromTrack.gameObject.SetActive(false);
            _crossfadeToTrack.gameObject.SetActive(false);

            _crossfade = null;
        }
        #endregion
    }
}