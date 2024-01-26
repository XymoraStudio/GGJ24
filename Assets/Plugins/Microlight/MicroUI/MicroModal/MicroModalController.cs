using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Controller for modal windows
    // ****************************************************************************************************
    public class MicroModalController : MicroUIController {
        MicroModalWindow _prefab;
        RectTransform _container;

        internal MicroModalController(MicroModalWindow prefab, RectTransform container) {
            _prefab = prefab;
            _container = container;

            CheckReferences();
        }
        protected void CheckReferences() {
            if(_prefab == null) {
                Debug.LogWarning("MicroModal: Prefab is not referenced.");
            }
            if(_container == null) {
                Debug.LogWarning("MicroModal: Container is not referenced.");
            }
        }

        public MicroModalWindow SpawnModalWindow(string modalText, MicroModalWindow.ConfirmAction confirm, MicroModalWindow.DeclineAction decline, MicroModalWindow customPrefab = null) {
            if(customPrefab == null && _prefab == null) return null;
            if(_container == null) return null;

            MicroModalWindow usedPrefab = customPrefab != null ? customPrefab : _prefab;
            MicroModalWindow window = Object.Instantiate(usedPrefab, _container);
            window.ShowWindow(modalText, confirm, decline);
            return window;
        }

        #region MicroUI methods
        override internal void Update() { }
        #endregion
    }
}