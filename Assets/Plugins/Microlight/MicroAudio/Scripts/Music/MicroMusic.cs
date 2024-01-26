using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // Music module for MicroAudio
    // ****************************************************************************************************
    internal class MicroMusic {
        AudioSource _mainAudioSource;
        AudioSource _crossfadeAudioSource;

        bool _musicStarted;
        bool _invokenMusicEnd;
        bool _shufflePlaylist;
        bool _forcedChangeOfMusic;
        float _crossfadeTime;

        MicroSoundGroup _activeGroup;
        List<int> _playlist = new List<int>();
        int _playlistCurrentIndex;
        List<SoundFade> _soundFades;

        internal MicroMusic(AudioSource source1, AudioSource source2, float crossfadeTime) {
            _mainAudioSource = source1;
            _crossfadeAudioSource = source2;

            _musicStarted = false;
            _invokenMusicEnd = false;
            _shufflePlaylist = false;
            _forcedChangeOfMusic = false;
            _crossfadeTime = crossfadeTime;

            _activeGroup = null;
            _playlist = new List<int>();
            _playlistCurrentIndex = 0;
            _soundFades = new List<SoundFade>();

            MicroAudio.UpdateEvent += Update;
        }

        void Update() {
            if(!_musicStarted && _mainAudioSource.time > 0) {
                _musicStarted = true;
                _invokenMusicEnd = false;
                MicroAudioDebugger.TrackStarted(_mainAudioSource);
                MicroAudio.MusicStarted();
            }
            else if(_musicStarted &&
                    _activeGroup != null &&
                    _crossfadeTime > 0f &&
                    _mainAudioSource.time >= _mainAudioSource.clip.length - _crossfadeTime &&
                    !_invokenMusicEnd) {
                EndMusicTrack();
            }
            else if(_musicStarted && _mainAudioSource.time == 0 && !_invokenMusicEnd) {
                EndMusicTrack();
            }

            if(_forcedChangeOfMusic) _forcedChangeOfMusic = false;
        }

        #region Internal methods
        internal MicroSoundGroup ActiveGroup => _activeGroup;
        internal List<int> Playlist => _playlist;
        internal int PlaylistCurrentIndex => _playlistCurrentIndex;
        internal AudioSource MainAudioSource => _mainAudioSource;
        internal AudioSource CrossfadeAudioSource => _crossfadeAudioSource;
        internal void PlayOneTrack(AudioClip clip, bool loop, float crossfade) {
            if(clip == null) {
                MicroAudioDebugger.MusicTrackEmpty();
            }
            ClearMusicGroup();
            ClearSoundFades();

            if(crossfade > 0f) {
                SwapAudioSources();
                PlayMusicClip(clip, _mainAudioSource, loop, 1f);
                FadeSound(_mainAudioSource, 0f, 1f, crossfade);
                SoundFade fade = FadeSound(_crossfadeAudioSource, 1f, 0f, crossfade);
                fade.OnFadeEnd += StopCrossfadeTrack;
                MicroAudioDebugger.CrossfadeStarted();
                MicroAudio.CrossfadeStarted(fade);
            }
            else {
                PlayMusicClip(clip, _mainAudioSource, loop, 1f);
            }
        }
        internal void PlayMusicGroup(MicroSoundGroup group, bool shuffle = false, bool bypassCrossfade = false) {
            if(group == null) {
                MicroAudioDebugger.MusicTrackEmpty();
                return;
            }
            if(group.ClipList.Count < 1) {
                _activeGroup = null;
                return;
            }
            ClearMusicGroup();
            ClearSoundFades();
            //StopAllMusic();
            _forcedChangeOfMusic = true;

            _activeGroup = group;
            _shufflePlaylist = shuffle;
            GeneratePlaylist();

            PrepareSources();
            ChangeTrack(0, bypassCrossfade);

            void PrepareSources() {
                _mainAudioSource.loop = false;
                _mainAudioSource.volume = 1f;
                _crossfadeAudioSource.loop = false;
                _crossfadeAudioSource.volume = 1f;
            }
        }
        internal void NextTrack() {
            if(_activeGroup == null) return;
            if(_forcedChangeOfMusic) return;
            ChangeTrack(true);
            _forcedChangeOfMusic = true;
        }
        internal void PreviousTrack() {
            if(_activeGroup == null) return;
            if(_forcedChangeOfMusic) return;
            ChangeTrack(false);
            _forcedChangeOfMusic = true;
        }
        internal void SelectTrack(int index) {
            if(_activeGroup == null) return;
            if(index < 0 || index >= _playlist.Count) return;
            if(_forcedChangeOfMusic) return;
            ChangeTrack(index);
            _forcedChangeOfMusic = true;
        }
        internal float CurrentTrackProgress() {
            if(_mainAudioSource == null) return 0;
            if(_crossfadeAudioSource == null) return 0;

            if(_crossfadeAudioSource.time > 0) {
                if(_crossfadeAudioSource.clip == null) return 0;
                return Mathf.Clamp01(_crossfadeAudioSource.time / _crossfadeAudioSource.clip.length);
            }
            else {
                if(_mainAudioSource.clip == null) return 0;
                return Mathf.Clamp01(_mainAudioSource.time / _mainAudioSource.clip.length);
            }
        }
        internal void SetCrossfadeDuration(float crossfadeTime) {
            _crossfadeTime = crossfadeTime;
        }
        #endregion

        #region Private methods              
        void PlayMusicClip(AudioClip clip, AudioSource source, bool loop, float volume) {
            if(source == null) return;

            _musicStarted = false;
            source.clip = clip;
            source.loop = loop;
            source.volume = volume;
            source.Play();
            MicroAudioDebugger.PlayMusicClip(source);
        }
        void ChangeTrack(int index, bool bypassFade = false) => ChangeTrack(true, index, bypassFade);
        void ChangeTrack(bool next, int index = -1, bool bypassFade = false) {
            if(_activeGroup == null) return;
            if(_playlist.Count < 1) return;
            if(_playlist.Count < 2) {
                _mainAudioSource.Play();
                return;
            }

            // Change index
            if(index >= 0 && index < _playlist.Count) {
                _playlistCurrentIndex = index;
                MicroAudioDebugger.SpecificTrack();
            }
            else if(next) {
                _playlistCurrentIndex++;
                MicroAudioDebugger.NextTrack();
            }
            else {
                _playlistCurrentIndex--;
                MicroAudioDebugger.PreviousTrack();
            }

            if(_playlistCurrentIndex >= _playlist.Count) {
                GeneratePlaylist();
            }
            else if(_playlistCurrentIndex < 0) {
                GeneratePlaylist();
                _playlistCurrentIndex = _playlist.Count - 1;
            }

            // Play the clip
            if(!bypassFade && _crossfadeTime > 0f) {
                ClearSoundFades();
                SwapAudioSources();
                PlayMusicClip(_activeGroup.ClipList[_playlist[_playlistCurrentIndex]], _mainAudioSource, false, 1f);
                FadeSound(_mainAudioSource, 0f, 1f, _crossfadeTime);
                SoundFade fade = FadeSound(_crossfadeAudioSource, 1f, 0f, _crossfadeTime);
                fade.OnFadeEnd += StopCrossfadeTrack;
                MicroAudioDebugger.CrossfadeStarted();
                MicroAudio.CrossfadeStarted(fade);
            }
            else {
                PlayMusicClip(_activeGroup.ClipList[_playlist[_playlistCurrentIndex]], _mainAudioSource, false, 1f);
            }
            MicroAudioDebugger.ChangedTrack(_mainAudioSource);
        }
        void GeneratePlaylist() {
            if(_activeGroup == null) return;
            _playlistCurrentIndex = 0;
            _playlist.Clear();

            for(int i = 0; i < _activeGroup.ClipList.Count; i++) {
                _playlist.Add(i);
            }

            if(_shufflePlaylist) Shuffle();
            MicroAudioDebugger.GeneratedPlaylist(_shufflePlaylist, _playlist);
            MicroAudio.NewPlaylistCreated(_playlist, _activeGroup);

            void Shuffle() {
                int n = _playlist.Count;
                int k;
                int swapTmp;
                while(n > 1) {
                    n--;
                    k = UnityEngine.Random.Range(0, n + 1);
                    swapTmp = _playlist[k];
                    _playlist[k] = _playlist[n];
                    _playlist[n] = swapTmp;
                }
            }
        }
        void EndMusicTrack() {
            if(_crossfadeAudioSource.time > 0) MicroAudioDebugger.TrackEnded(_crossfadeAudioSource);
            else MicroAudioDebugger.TrackEnded(_mainAudioSource);

            _musicStarted = false;
            MicroAudio.MusicEnded();           
            _invokenMusicEnd = true;
        }
        void StopAllMusic() {
            _mainAudioSource.Stop();
            _crossfadeAudioSource.Stop();
        }
        void ClearMusicGroup() {
            _activeGroup = null;
            _playlist.Clear();
            _playlistCurrentIndex = 0;
            MicroAudioDebugger.ClearedMusicGroup();
        }
        void SwapAudioSources() {
            AudioSource tmpSrc = _mainAudioSource;
            _mainAudioSource = _crossfadeAudioSource;
            _crossfadeAudioSource = tmpSrc;
            MicroAudioDebugger.SourcesSwapped();
        }
        #endregion        

        #region Music Fade
        SoundFade FadeSound(AudioSource src, float startVolume, float endVolume, float overSeconds) {
            SoundFade fade = new SoundFade(src, startVolume, endVolume, overSeconds);
            fade.Destroy += FinishFade;
            _soundFades.Add(fade);
            return fade;
        }
        void FinishFade(SoundFade fade) {
            _soundFades.Remove(fade);
        }
        void ClearSoundFades() {
            while(_soundFades.Count > 0) {
                _soundFades[0].Kill();
            }
            _soundFades.Clear();
        }
        void StopCrossfadeTrack(SoundFade fade) {
            fade.Source.Stop();
            MicroAudio.CrossfadeEnded(fade);
            MicroAudioDebugger.CrossfadeEnded();
        }
        #endregion
    }
}