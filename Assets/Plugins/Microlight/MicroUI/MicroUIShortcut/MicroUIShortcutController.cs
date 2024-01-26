using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Enables shortcuts feature on screens, for example pressing escape to active close button on popup
    // ****************************************************************************************************
    public class MicroUIShortcutController : MonoBehaviour {
        readonly List<MicroUIShortcutGroup> _groups = new List<MicroUIShortcutGroup>();   // Stores shortcut groups
        MicroUIShortcutGroup _highestPriorityGroup;

        // Default priority settings
        public static int DEFAULT_PRIORITY = 0;
        public static int SETTINGS_PRIORITY = 100;
        public static int POPUP_PRIORITY = 200;
        public static int MODAL_PRIORITY = 400;

        // Singleton
        static MicroUIShortcutController _instance;
        public static MicroUIShortcutController Instance {
            get => _instance;
        }
        private void Awake() {
            if(_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }
            else _instance = this;

            SceneManager.sceneLoaded += OnSceneChanged;
            DontDestroyOnLoad(gameObject);
        }
        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneChanged;
        }

        private void Update() {
            if(_highestPriorityGroup == null) return;

            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)) {
                if(_highestPriorityGroup != null && _highestPriorityGroup.Decline != null) _highestPriorityGroup.Decline.Click();
            }
            else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                if(_highestPriorityGroup != null && _highestPriorityGroup.Confirm != null) _highestPriorityGroup.Confirm.Click();
            }
        }
        /// <summary>
        /// Returns group of highest priority
        /// </summary>
        MicroUIShortcutGroup HighestPriorityGroup() {
            MicroUIShortcutGroup highestPriorityGroup = null;
            foreach(MicroUIShortcutGroup group in _groups) {
                if(highestPriorityGroup == null) highestPriorityGroup = group;
                if(group.Priority > highestPriorityGroup.Priority) highestPriorityGroup = group;
            }
            return highestPriorityGroup;
        }
        
        public void ResetGroups() {
            _groups.Clear();
            _highestPriorityGroup = null;
        }
        public void AddGroup(MicroUIShortcutGroup group) {
            if(group == null) return;
            if(_groups.Contains(group)) return;

            _groups.Add(group);
            _highestPriorityGroup = HighestPriorityGroup();
        }
        public void RemoveGroup(MicroUIShortcutGroup group) {
            if(group == null) return;
            if(!_groups.Contains(group)) return;

            _groups.Remove(group);
            _highestPriorityGroup = HighestPriorityGroup();
        }
        // When scene changes, we want to remove any active shortcuts
        void OnSceneChanged(Scene scene, LoadSceneMode sceneMode) {
            ResetGroups();
        }
    }
}