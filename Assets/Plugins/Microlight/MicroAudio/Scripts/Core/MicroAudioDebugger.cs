using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroAudio {
    // ****************************************************************************************************
    // Various options to show debug  messages
    // ****************************************************************************************************
    internal static class MicroAudioDebugger {
        #region Manager
        internal static void Initialized() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.ManagerDebug) return;
            //return;

            Debug.Log("MicroAudio: <color=green>Initialized</color>");
        }
        internal static void NotInitialized() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.ManagerDebug) return;
            //return;

            Debug.LogWarning("MicroAudio: <color=red>Manager not present on the scene</color>");
        }
        internal static void SavedSettings() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.ManagerDebug) return;
            //return;

            Debug.Log("MicroAudio: <color=green>Settings saved</color>");
        }
        internal static void LoadedSettings() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.ManagerDebug) return;
            //return;

            Debug.Log("MicroAudio: <color=green>Settings loaded</color>");
        }
        internal static void MasterVolumeChanged(float volume) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.ManagerDebug) return;
            //return;

            Debug.Log($"MicroAudio: Master volume changed to {volume}");
        }
        internal static void MusicVolumeChanged(float volume) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.ManagerDebug) return;
            //return;

            Debug.Log($"MicroAudio: Music volume changed to {volume}");
        }
        internal static void SFXVolumeChanged(float volume) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.ManagerDebug) return;
            //return;

            Debug.Log($"MicroAudio: SFX volume changed to {volume}");
        }
        internal static void UIVolumeChanged(float volume) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.ManagerDebug) return;
            //return;

            Debug.Log($"MicroAudio: UI volume changed to {volume}");
        }
        #endregion

        #region Music
        internal static void PlayMusicClip(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicDebug) return;
            //return;

            Debug.Log($"MicroAudio: Command to play music clip on {src.gameObject.name}; track {src.clip.name}");
        }                               
        internal static void GeneratedPlaylist(bool shuffle, List<int> playlist) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicDebug) return;
            //return;

            if(shuffle) Debug.Log("MicroAudio: Generated playlist. Shuffled.");
            else Debug.Log("MicroAudio: Generated playlist. Not shuffled.");
            string playlistInString = "";
            foreach(int x in playlist) {
                playlistInString += $"{x},";
            }
            playlistInString = playlistInString.Remove(playlistInString.Length - 1, 1);
            Debug.Log($"MicroAudio: Created playlist:{playlistInString}");
        }
        internal static void ClearedMusicGroup() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicDebug) return;
            //return;

            Debug.Log("MicroAudio: Cleared music group.");
        }
        internal static void SourcesSwapped() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicDebug) return;
            //return;

            Debug.Log("MicroAudio: Sources swapped.");
        }
        internal static void MusicTrackEmpty() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicDebug) return;
            //return;

            Debug.LogWarning($"MicroAudio: Music track reference is null");
        }
        internal static void NoActiveGroup() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicDebug) return;
            //return;

            Debug.LogWarning($"MicroAudio: There is no active group");
        }
        #endregion

        #region MusicTracks
        internal static void NextTrack() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicTracksDebug) return;
            //return;

            Debug.Log("MicroAudio: Next track.");
        }
        internal static void PreviousTrack() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicTracksDebug) return;
            //return;

            Debug.Log("MicroAudio: Previous track.");
        }
        internal static void SpecificTrack() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicTracksDebug) return;
            //return;

            Debug.Log("MicroAudio: Chosen specific track.");
        }
        internal static void TrackStarted(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicTracksDebug) return;
            //return;

            Debug.Log($"MicroAudio: Track started on {src.gameObject.name}; track {src.clip.name}");
        }
        internal static void TrackEnded(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicTracksDebug) return;
            //return;

            Debug.Log($"MicroAudio: Track ended on {src.gameObject.name}; track {src.clip.name}");
        }
        internal static void ChangedTrack(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.MusicTracksDebug) return;
            //return;

            Debug.Log($"MicroAudio: Changed track. Now playing {src.clip.name}");
        }        
        #endregion

        #region Crossfade
        internal static void CrossfadeStarted() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.CrossfadeDebug) return;
            //return;

            Debug.Log("MicroAudio: Crossfade started.");
        }
        internal static void CrossfadeEnded() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.CrossfadeDebug) return;
            //return;

            Debug.Log("MicroAudio: Crossfade ended.");
        }
        #endregion

        #region Sounds
        internal static void PlaySound(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.SoundsDebug) return;
            //return;

            Debug.Log($"MicroAudio: Playing sound; {src.gameObject.name}; clip:{src.clip.name}");
        }
        internal static void SoundSourceCreated(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.SoundsDebug) return;
            //return;

            Debug.Log($"MicroAudio: Sound source created; {src.gameObject.name}");
        }        
        internal static void CantCreateNewSources() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.SoundsDebug) return;
            //return;

            Debug.Log("MicroAudio: Can't create new sources, limit reached.");
        }
        internal static void TooManySameClips() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.SoundsDebug) return;
            //return;

            Debug.Log("MicroAudio: Can't play sound because there are already too many sources with the same clip.");
        }
        internal static void SoundClipEmpty() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.SoundsDebug) return;
            //return;

            Debug.LogWarning($"MicroAudio: Sound clip reference is null");
        }
        #endregion

        #region Infinity
        internal static void StartInfinityGroup() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.InfinityDebug) return;
            //return;

            Debug.Log($"MicroAudio: Started infinity group.");
        }
        internal static void FinishInfinityGroup() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.InfinityDebug) return;
            //return;

            Debug.Log($"MicroAudio: Finished infinity group.");
        }
        internal static void CancelInfinityGroup() {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.InfinityDebug) return;
            //return;

            Debug.Log($"MicroAudio: Canceled infinity group.");
        }
        internal static void StartInfinityLoop(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.InfinityDebug) return;
            //return;

            Debug.Log($"MicroAudio: Playing infinity looped sound; {src.gameObject.name}; clip:{src.clip.name}");
        }
        internal static void StartInfinitySound(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.InfinityDebug) return;
            //return;

            Debug.Log($"MicroAudio: Playing start sound for infinity group; {src.clip.name}");
        }
        internal static void EndInfinitySound(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.InfinityDebug) return;
            //return;

            Debug.Log($"MicroAudio: Playing end sound for infinity group; {src.clip.name}");
        }
        internal static void RandomInfinitySound(AudioSource src) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.InfinityDebug) return;
            //return;

            Debug.Log($"MicroAudio: Playing random sound for infinity group; {src.clip.name}");
        }
        #endregion  

        #region Delay
        internal static void StartDelayedSound(DelayedSound delay) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.DelayDebug) return;
            //return;

            Debug.Log($"MicroAudio: Started delay for sound; {delay.Source.gameObject.name}; clip:{delay.Source.clip.name}; delay:{delay.Delay}");
        }
        internal static void PlayDelayedSound(DelayedSound delay) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.DelayDebug) return;
            //return;

            Debug.Log($"MicroAudio: Delayed sound and now playing; {delay.Source.gameObject.name}; clip:{delay.Source.clip.name}.");
        }
        #endregion

        #region Fade
        internal static void FadeCreated(SoundFade fade) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.FadeDebug) return;
            //return;

            if(fade.IsPlaying) Debug.Log($"MicroAudio: Fade started on {fade.Source.gameObject.name}; {fade.StartVolume} -> {fade.EndVolume} over {fade.OverSeconds} seconds");
            else Debug.Log($"MicroAudio: Fade created but paused on {fade.Source.gameObject.name}; {fade.StartVolume} -> {fade.EndVolume} over {fade.OverSeconds} seconds");
        }
        internal static void FadeEnded(SoundFade fade) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.FadeDebug) return;
            //return;

            Debug.Log($"MicroAudio: Fade ended on {fade.Source.gameObject.name}");
        }
        internal static void FadePlayingChanged(SoundFade fade) {
            if(!MicroAudio.DebugMode) return;
            if(!MicroAudio.FadeDebug) return;
            //return;

            if(fade.IsPlaying) Debug.Log($"MicroAudio: Fade resumed on {fade.Source.gameObject.name}");
            else Debug.Log($"MicroAudio: Fade paused on {fade.Source.gameObject.name}");
        }
        #endregion

        #region Forced
        internal static void RandomClipListEmpty() {
            if(!MicroAudio.DebugMode) return;
            //return;

            Debug.LogWarning("MicroAudio: Sound group doesn't have any clips. Can't get random clip.");
        }
        #endregion
    }
}