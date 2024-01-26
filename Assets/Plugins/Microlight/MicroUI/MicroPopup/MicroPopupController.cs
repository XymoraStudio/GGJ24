using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Controller for popup windows
    // ****************************************************************************************************
    public class MicroPopupController : MicroUIController {
        MicroPopupWindow _prefab;
        RectTransform _container;

        internal MicroPopupController(MicroPopupWindow prefab, RectTransform container) {
            _prefab = prefab;
            _container = container;

            CheckReferences();
        }
        protected void CheckReferences() {
            if(_prefab == null) {
                //Debug.LogWarning("MicroPopup: Prefab is not referenced.");
            }
            if(_container == null) {
                //Debug.LogWarning("MicroPopup: Container is not referenced.");
            }
        }
        public MicroPopupWindow SpawnPopup(string text) => SpawnPopup(text, null, 0, null);
        public MicroPopupWindow SpawnPopup(string text, MicroPopupWindow.CloseAction closeAction) => SpawnPopup(text, null, 0, closeAction);
        public MicroPopupWindow SpawnPopup(string text, int id) => SpawnPopup(text, null, id, null);
        public MicroPopupWindow SpawnPopup(string text, int id, MicroPopupWindow.CloseAction closeAction) => SpawnPopup(text, null, id, closeAction);
        public MicroPopupWindow SpawnPopup(string text, RectTransform spawnPosition, MicroPopupWindow.CloseAction closeAction) => SpawnPopup(text, spawnPosition, -1, closeAction);
        public MicroPopupWindow SpawnPopup(string text, RectTransform spawnPosition, int id = -1, MicroPopupWindow.CloseAction closeAction = null, MicroPopupWindow customPrefab = null) {
            if(customPrefab == null && _prefab == null) return null;
            if(_container == null) return null;

            MicroPopupWindow usedPrefab = customPrefab != null ? customPrefab : _prefab;
            MicroPopupWindow popup = Object.Instantiate(usedPrefab, _container);
            popup.ShowWindow(text, spawnPosition, id, closeAction);
            return popup;
        }

        #region MicroUI methods
        override internal void Update() { }
        #endregion
    }
}