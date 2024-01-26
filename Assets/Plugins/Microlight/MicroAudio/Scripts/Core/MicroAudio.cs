using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // Manager for MicroAudio, handles settings and intermediate between modules
    // Should be on first scene
    // ****************************************************************************************************
    public class MicroAudio : MonoBehaviour {
        // Mixer
        [SerializeField] AudioMixer _mixer;
        [SerializeField] AudioMixerGroup _masterMixerGroup;
        [SerializeField] AudioMixerGroup _musicMixerGroup;
        [SerializeField] AudioMixerGroup _sfxMixerGroup;
        [SerializeField] AudioMixerGroup _uiMixerGroup;
        public static AudioMixer Mixer => _instance._mixer;
        public static AudioMixerGroup MasterMixerGroup => _instance._masterMixerGroup;
        public static AudioMixerGroup MusicMixerGroup => _instance._musicMixerGroup;
        public static AudioMixerGroup SFXMixerGroup => _instance._sfxMixerGroup;
        public static AudioMixerGroup UIMixerGroup => _instance._uiMixerGroup;

        // Music
        [SerializeField] AudioSource _musicAudioSource1;
        [SerializeField] AudioSource _musicAudioSource2;   // Used for crossfade of songs
        [SerializeField] float _crossfadeTime;

        // Sounds
        [SerializeField] int _maxSoundsSources = 40;
        [SerializeField] int _maxInstancesOfSameClip = 8;   // Maximum number of same sounds playing at the same time
        [SerializeField] Transform _soundsContainer;

        // Debug
        [SerializeField] bool _debugMode;
        internal static bool DebugMode {
            get {
                if(_instance == null) return false;
                return _instance._debugMode;
            }
        }
        [SerializeField] bool _managerDebug;
        internal static bool ManagerDebug => _instance._managerDebug;
        [SerializeField] bool _musicDebug;
        internal static bool MusicDebug => _instance._musicDebug;
        [SerializeField] bool _musicTracksDebug;
        internal static bool MusicTracksDebug => _instance._musicTracksDebug;
        [SerializeField] bool _crossfadeDebug;
        internal static bool CrossfadeDebug => _instance._crossfadeDebug;
        [SerializeField] bool _soundsDebug;
        internal static bool SoundsDebug => _instance._soundsDebug;
        [SerializeField] bool _infinityDebug;
        internal static bool InfinityDebug => _instance._infinityDebug;
        [SerializeField] bool _delayDebug;
        internal static bool DelayDebug => _instance._delayDebug;
        [SerializeField] bool _fadeDebug;
        internal static bool FadeDebug => _instance._fadeDebug;        

        // Strings for referencing mixer group
        const string MIXER_MASTER = "MasterVolume";
        const string MIXER_MUSIC = "MusicVolume";
        const string MIXER_SFX = "SFXVolume";
        const string MIXER_UI = "UIVolume";

        // Keys for PlayerPref for saving and loading of volume
        const string MASTER_KEY = "microAudioMasterVolume";
        const string MUSIC_KEY = "microAudioMusicVolume";
        const string SFX_KEY = "microAudioSFXVolume";
        const string UI_KEY = "microAudioUIVolume";

        // Modules
        MicroMusic _microMusic;
        MicroSounds _microSounds;
        MicroInfinitySounds _microInfinitySounds;

        // Volume
        static float _masterVolume; 
        public static float MasterVolume {
            get => _masterVolume;
            set {
                if(_instance == null) {
                    MicroAudioDebugger.NotInitialized();
                    return;
                }
                value = Mathf.Clamp(value, 0.0001f, 1f);
                _masterVolume = value;
                _instance._mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
                MicroAudioDebugger.MasterVolumeChanged(_masterVolume);
            } 
        }
        static float _musicVolume;
        public static float MusicVolume {
            get => _musicVolume;
            set {
                if(_instance == null) {
                    MicroAudioDebugger.NotInitialized();
                    return;
                }
                value = Mathf.Clamp(value, 0.0001f, 1f);
                _musicVolume = value;
                _instance._mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
                MicroAudioDebugger.MasterVolumeChanged(_musicVolume);
            }
        }
        static float _sfxVolume;
        public static float SFXVolume {
            get => _sfxVolume;
            set {
                if(_instance == null) {
                    MicroAudioDebugger.NotInitialized();
                    return;
                }
                value = Mathf.Clamp(value, 0.0001f, 1f);
                _sfxVolume = value;
                _instance._mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
                MicroAudioDebugger.MasterVolumeChanged(_sfxVolume);
            }
        }
        static float _uiVolume;
        public static float UIVolume {
            get => _uiVolume;
            set {
                if(_instance == null) {
                    MicroAudioDebugger.NotInitialized();
                    return;
                }
                value = Mathf.Clamp(value, 0.0001f, 1f);
                _uiVolume = value;
                _instance._mixer.SetFloat(MIXER_UI, Mathf.Log10(value) * 20);
                MicroAudioDebugger.MasterVolumeChanged(_uiVolume);
            }
        }        

        // Events for systems
        internal static event Action UpdateEvent;

        #region Initialization
        // Singleton
        internal static MicroAudio _instance;
        private void Awake() {
            if(_instance == null) {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
                return;
            }

            _microMusic = new MicroMusic(_musicAudioSource1, _musicAudioSource2, _crossfadeTime);
            _microSounds = new MicroSounds(_soundsContainer, _maxSoundsSources, _maxInstancesOfSameClip);
            _microInfinitySounds = new MicroInfinitySounds(_microSounds);

            CheckReferences();
            LoadSettings();
            OnMusicEnd += NextTrack;

            MicroAudioDebugger.Initialized();
        }
        protected void CheckReferences() {
            if(_mixer == null) {
                Debug.LogWarning("MicroAudio: Mixer is not referenced.");
            }
            if(_masterMixerGroup == null) {
                Debug.LogWarning("MicroAudio: Master mixer group is not referenced.");
            }
            if(_musicMixerGroup == null) {
                Debug.LogWarning("MicroAudio: Music mixer group is not referenced.");
            }
            if(_sfxMixerGroup == null) {
                Debug.LogWarning("MicroAudio: SFX mixer group is not referenced.");
            }
            if(_uiMixerGroup == null) {
                Debug.LogWarning("MicroAudio: UI mixer group is not referenced.");
            }
            if(_soundsContainer == null) {
                Debug.LogWarning("MicroAudio: Sounds container is not referenced.");
            }
            if(_musicAudioSource1 == null) {
                Debug.LogWarning("MicroAudio: Music audio source 1 is not referenced.");
            }
            if(_musicAudioSource2 == null) {
                Debug.LogWarning("MicroAudio: Music audio source 2 is not referenced.");
            }
        }
        #endregion

        private void Update() {
            UpdateEvent?.Invoke();
        }
        private void OnDestroy() {
            UpdateEvent = null;
        }

        #region SaveLoad
        public static void SaveSettings() {
            PlayerPrefs.SetFloat(MASTER_KEY, _masterVolume);
            PlayerPrefs.SetFloat(MUSIC_KEY, _musicVolume);
            PlayerPrefs.SetFloat(SFX_KEY, _sfxVolume);
            PlayerPrefs.SetFloat(UI_KEY, _uiVolume);

            MicroAudioDebugger.SavedSettings();
        }
        public static void LoadSettings() {
            MasterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
            MusicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            SFXVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
            UIVolume = PlayerPrefs.GetFloat(UI_KEY, 1f);

            MicroAudioDebugger.LoadedSettings();
        }
        #endregion

        #region Music
        // Data
        public static float CurrentTrackProgress {
            get {
                if(_instance == null || _instance._microMusic == null) {
                    MicroAudioDebugger.NotInitialized();
                    return -1f;
                }
                return _instance._microMusic.CurrentTrackProgress();
            }
        }
        public static MicroSoundGroup ActiveMusicGroup {
            get {
                if(_instance == null || _instance._microMusic == null) {
                    MicroAudioDebugger.NotInitialized();
                    return null;
                }
                return _instance._microMusic.ActiveGroup;
            }
        }
        public static List<int> MusicPlaylist {
            get {
                if(_instance == null || _instance._microMusic == null) {
                    MicroAudioDebugger.NotInitialized();
                    return null;
                }
                return _instance._microMusic.Playlist;
            }
        }
        public static int MusicCurrentPlaylistIndex {
            get {
                if(_instance == null || _instance._microMusic == null) {
                    MicroAudioDebugger.NotInitialized();
                    return -1;
                }
                return _instance._microMusic.PlaylistCurrentIndex;
            }
        }
        public static AudioSource MainMusicAudioSource {
            get {
                if(_instance == null || _instance._microMusic == null) {
                    MicroAudioDebugger.NotInitialized();
                    return null;
                }
                return _instance._microMusic.MainAudioSource;
            }
        }
        public static AudioSource CrossfadeMusicAudioSource {
            get {
                if(_instance == null || _instance._microMusic == null) {
                    MicroAudioDebugger.NotInitialized();
                    return null;
                }
                return _instance._microMusic.CrossfadeAudioSource;
            }
        }

        // Events
        // When music clip came to end and stopped
        // If crossfade is turned on, it triggers when another song starts playing
        public static event Action OnMusicStart;
        internal static void MusicStarted() => OnMusicStart?.Invoke();
        public static event Action OnMusicEnd;
        internal static void MusicEnded() => OnMusicEnd?.Invoke();
        public static event Action<SoundFade> OnCrossfadeStart;
        internal static void CrossfadeStarted(SoundFade fade) => OnCrossfadeStart?.Invoke(fade);
        public static event Action<SoundFade> OnCrossfadeEnd;
        internal static void CrossfadeEnded(SoundFade fade) => OnCrossfadeEnd?.Invoke(fade);
        public static event Action<List<int>, MicroSoundGroup> OnNewPlaylist;
        internal static void NewPlaylistCreated(List<int> playlist, MicroSoundGroup playlistClips) => OnNewPlaylist?.Invoke(playlist, playlistClips);

        // Controls
        public static void PlayOneTrack(AudioClip clip, bool loop = true, float crossfade = 0f) {
            if(_instance == null || _instance._microMusic == null) {
                MicroAudioDebugger.NotInitialized();
                return;
            }
            _instance._microMusic.PlayOneTrack(clip, loop, crossfade);
        }
        /// <summary>
        /// Allows to turn on or off crossfade, time of 0 or less turns crossfade off
        /// </summary>
        public static void PlayMusicGroup(MicroSoundGroup group, float crossfadeTime, bool shuffle = false, bool bypassCrossfade = false) {
            if(_instance == null || _instance._microMusic == null) {
                MicroAudioDebugger.NotInitialized();
                return;
            }
            _instance._microMusic.SetCrossfadeDuration(crossfadeTime);
            PlayMusicGroup(group, shuffle, bypassCrossfade);
        }
        public static void PlayMusicGroup(MicroSoundGroup group, bool shuffle = false, bool bypassCrossfade = false) {
            if(_instance == null || _instance._microMusic == null) {
                MicroAudioDebugger.NotInitialized();
                return;
            }
            _instance._microMusic.PlayMusicGroup(group, shuffle, bypassCrossfade);            
        }
        public static void NextTrack() {
            if(_instance == null || _instance._microMusic == null) {
                MicroAudioDebugger.NotInitialized();
                return;
            }
            _instance._microMusic.NextTrack();
        }
        public static void PreviousTrack() {
            if(_instance == null || _instance._microMusic == null) {
                MicroAudioDebugger.NotInitialized();
                return;
            }
            _instance._microMusic.PreviousTrack();
        }
        public static void SelectTrack(int index) {
            if(_instance == null || _instance._microMusic == null) {
                MicroAudioDebugger.NotInitialized();
                return;
            }
            _instance._microMusic.SelectTrack(index);
        }
        public static void SetCrossfadeDuration(float crossfadeDuration) {
            if(_instance == null || _instance._microMusic == null) {
                MicroAudioDebugger.NotInitialized();
                return;
            }
            _instance._microMusic.SetCrossfadeDuration(crossfadeDuration);
        }
        #endregion

        #region Sounds
        public static AudioSource PlayEffectSound(AudioClip clip, AudioSource src) {
            if(_instance == null || _instance._microSounds == null) {
                MicroAudioDebugger.NotInitialized();
                return null;
            }
            return _instance._microSounds.PlaySound(clip, _instance._sfxMixerGroup, src, 0f, 1f, 1f);
        }
        public static AudioSource PlayEffectSound(AudioClip clip, float delay = 0f, float volume = 1f, float pitch = 1f, AudioSource src = null) {
            if(_instance == null || _instance._microSounds == null) {
                MicroAudioDebugger.NotInitialized();
                return null;
            }
            return _instance._microSounds.PlaySound(clip, _instance._sfxMixerGroup, src, delay, volume, pitch);
        }
        public static AudioSource PlayUISound(AudioClip clip, AudioSource src) {
            if(_instance == null || _instance._microSounds == null) {
                MicroAudioDebugger.NotInitialized();
                return null;
            }
            return _instance._microSounds.PlaySound(clip, _instance._uiMixerGroup, src, 0f, 1f, 1f);
        }
        public static AudioSource PlayUISound(AudioClip clip, float delay = 0f, float volume = 1f, float pitch = 1f, AudioSource src = null) {
            if(_instance == null || _instance._microSounds == null) {
                MicroAudioDebugger.NotInitialized();
                return null;
            }
            return _instance._microSounds.PlaySound(clip, _instance._uiMixerGroup, src, delay, volume, pitch);
        }
        public static DelayedSound GetDelayStatusOfSound(AudioSource src) {
            if(_instance == null || _instance._microSounds == null) {
                MicroAudioDebugger.NotInitialized();
                return null;
            }
            return _instance._microSounds.GetDelayStatusOfSound(src);
        }
        #endregion

        #region Infinity Sounds
        public static MicroInfinityInstance PlayInfinityEffectSound(MicroInfinitySoundGroup group, float volume = 1f) {
            if(_instance == null || _instance._microSounds == null || _instance._microInfinitySounds == null) {
                MicroAudioDebugger.NotInitialized();
                return null;
            }
            return _instance._microInfinitySounds.PlayInfinitySound(group, _instance._sfxMixerGroup, volume);
        }
        public static MicroInfinityInstance PlayInfinityUISound(MicroInfinitySoundGroup group, float volume = 1f) {
            if(_instance == null || _instance._microSounds == null || _instance._microInfinitySounds == null) {
                MicroAudioDebugger.NotInitialized();
                return null;
            }
            return _instance._microInfinitySounds.PlayInfinitySound(group, _instance._uiMixerGroup, volume);
        }
        #endregion
    }
}