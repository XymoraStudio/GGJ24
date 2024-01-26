using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Controller for notification system
    // ****************************************************************************************************
    public class MicroNotificationController : MicroUIController {
        MicroNotificationWindow _prefab;
        RectTransform _container;
        float _defaultDuration;

        Queue<MicroNotificationSpawnInfo> _notificationSpawnQueue;
        MicroNotificationWindow _activeNotificationWindow;

        internal MicroNotificationController(MicroNotificationWindow prefab, RectTransform container, float defaultDuration) {
            _prefab = prefab;
            _container = container;
            _defaultDuration = defaultDuration;

            _notificationSpawnQueue = new Queue<MicroNotificationSpawnInfo>();
            _activeNotificationWindow = null;

            CheckReferences();
        }
        protected void CheckReferences() {
            if(_prefab == null) {
                //Debug.LogWarning("MicroNotification: Prefab is not referenced.");
            }
            if(_container == null) {
                //Debug.LogWarning("MicroNotification: Container is not referenced.");
            }
        }

        public MicroNotificationWindow QueueNotification(string text, float duration = 0, MicroNotificationWindow customPrefab = null) {
            if(customPrefab == null && _prefab == null) return null;
            if(_container == null) return null;
            if(duration == 0) duration = _defaultDuration;

            MicroNotificationWindow usedPrefab = customPrefab != null ? customPrefab : _prefab;
            MicroNotificationWindow notification = Object.Instantiate(usedPrefab, _container);
            _notificationSpawnQueue.Enqueue(new MicroNotificationSpawnInfo(notification, text, duration));
            notification.OnDespawn += NotificationWindowDespawned;
            TrySpawnNotification();
            return notification;
        }
        void TrySpawnNotification() {
            if(_activeNotificationWindow != null) return;
            if(_notificationSpawnQueue.Count < 1) return;

            MicroNotificationSpawnInfo spawnInfo = _notificationSpawnQueue.Dequeue();
            _activeNotificationWindow = spawnInfo.Window;
            _activeNotificationWindow.ShowWindow(spawnInfo.Text, spawnInfo.Duration);
        }
        void NotificationWindowDespawned(MicroNotificationWindow window) {
            window.OnDespawn -= NotificationWindowDespawned;
            if(_activeNotificationWindow == window) _activeNotificationWindow = null;
            TrySpawnNotification();
        }

        #region MicroUI methods
        override internal void Update() { }
        #endregion
    }
}