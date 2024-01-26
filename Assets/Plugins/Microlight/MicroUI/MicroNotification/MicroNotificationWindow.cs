using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Controls spawned notification window
    // ****************************************************************************************************
    public class MicroNotificationWindow : MonoBehaviour {
        // Properties
        [SerializeField] RectTransform _root;
        public RectTransform Root => _root;
        [SerializeField] Image _windowImage;
        public Image WindowImage => _windowImage;
        [SerializeField] TMP_Text _messageText;
        public TMP_Text MessageText => _messageText;
        RectTransform _container;

        // Variables
        Coroutine _waitAndDespawnCoroutine;

        // Events
        public event Action<MicroNotificationWindow> OnSpawn;
        public event Action<MicroNotificationWindow> OnDespawn;

        #region Initialization
        private void Awake() {
            CheckReferences();
            if(Root != null) Root.gameObject.SetActive(false);
        }        
        protected void CheckReferences() {
            if(Root == null) {
                Debug.LogWarning("MicroNotification: Root is not referenced.");
            }
            if(WindowImage == null) {
                Debug.LogWarning("MicroNotification: BackgroundImage is not referenced.");
            }
            if(MessageText == null) {
                Debug.LogWarning("MicroNotification: Text is not referenced.");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows to update contents of the notification and it resizes with it, in runtime
        /// </summary>
        public void UpdateContent(string text) {
            if(MessageText == null) return;
            MessageText.text = text;
            MessageText.ForceMeshUpdate();   // This is called so we can update the size of the tooltip
            UpdateWindowSize();
        }
        /// <summary>
        /// Updates background size to fit text and sprite
        /// </summary>
        void UpdateWindowSize() {
            if(Root == null) return;
            if(MessageText == null) return;

            Vector2 textSize = MessageText.GetRenderedValues(false);
            Vector2 paddingValue = new Vector2(Mathf.Abs(MessageText.GetComponent<RectTransform>().anchoredPosition.y), Mathf.Abs(MessageText.GetComponent<RectTransform>().anchoredPosition.y));
            Vector2 paddingSize = new Vector2(paddingValue.x, paddingValue.y) * 2;   // Multiply padding with 2 to pad the other side too
            Vector2 backgroundSize = textSize + paddingSize;

            Root.sizeDelta = backgroundSize;
        }
        #endregion

        #region Life cycle
        /// <summary>
        /// Displays notification and starts its lifetime
        /// </summary>
        internal void ShowWindow(string text, float duration) {
            if(Root == null) return;
            if(MessageText == null) return;
            _container = transform.parent.transform as RectTransform;
            if(_container == null) return;

            Root.gameObject.SetActive(true);
            UpdateContent(text);
            Spawn(duration);   // Spawn notification
        }
        
        void Spawn(float duration) {
            if(Root == null) return;

            Root.anchoredPosition = new Vector2(0f, -_container.rect.height * 0.1f);
            _waitAndDespawnCoroutine = StartCoroutine(WaitAndDespawn(duration));
            OnSpawn?.Invoke(this);
        }
        public void DeSpawn() {
            OnDespawn?.Invoke(this);
            Destroy(gameObject);
        }
        IEnumerator WaitAndDespawn(float seconds) {
            yield return new WaitForSeconds(seconds);
            DeSpawn();
        }
        #endregion
    }
}