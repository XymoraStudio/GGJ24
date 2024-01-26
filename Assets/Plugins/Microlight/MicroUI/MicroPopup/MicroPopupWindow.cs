using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Popup window
    // ****************************************************************************************************
    public class MicroPopupWindow : MonoBehaviour {
        [SerializeField] RectTransform _root;
        public RectTransform Root => _root;
        [SerializeField] Image _windowImage;
        public Image WindowImage => _windowImage;
        [SerializeField] TMP_Text _messageText;
        public TMP_Text MessageText => _messageText;
        [SerializeField] MicroButton _closeButton;
        public MicroButton CloseButton => _closeButton;
        [SerializeField] bool _updateWindowSizeToTextSize;
        RectTransform _container;

        // Delegates
        public delegate void CloseAction(int ID);
        CloseAction _closeAction;

        // Events
        public event Action<MicroPopupWindow, int> OnClose;

        // Variables
        int _id;
        public int ID => _id;
        MicroUIShortcutGroup _uiGroup;

        #region Initialization
        private void Awake() {
            CheckReferences();
            if(Root != null) Root.gameObject.SetActive(false);
        }        
        protected void CheckReferences() {
            if(Root == null) {
                Debug.LogWarning("MicroPopup: Root is not referenced.");
            }
            if(WindowImage == null) {
                Debug.LogWarning("MicroPopup: WindowImage is not referenced.");
            }
            if(MessageText == null) {
                Debug.LogWarning("MicroPopup: MessageText is not referenced.");
            }
            if(CloseButton == null) {
                Debug.LogWarning("MicroPopup: CloseButton is not referenced.");
            }
        }
        private void OnEnable() {
            _closeButton.OnClick += ClosePopup;
        }
        private void OnDisable() {
            _closeButton.OnClick -= ClosePopup;
        }
        private void OnDestroy() {
            if(MicroUIShortcutController.Instance != null && _uiGroup != null) MicroUIShortcutController.Instance.RemoveGroup(_uiGroup);
        }
        #endregion

        #region Window mangement        
        internal void ShowWindow(string text, RectTransform spawnRectPosition, int id = -1, CloseAction closeAction = null) {
            if(Root == null) return;
            if(MessageText == null) return;
            _container = transform.parent.transform as RectTransform;
            if(_container == null) return;
            _id = id;

            Root.gameObject.SetActive(true);
            UpdateMessage(text);
            if(spawnRectPosition != null) PositionPopup(spawnRectPosition);
            MicroUIUtilities.PositionRectInsideContainer(Root, _container);

            _closeAction = closeAction;

            _uiGroup = new MicroUIShortcutGroup(_closeButton, _closeButton, MicroUIShortcutController.POPUP_PRIORITY);
            if(MicroUIShortcutController.Instance != null) MicroUIShortcutController.Instance.AddGroup(_uiGroup);
        }
        void UpdateMessage(string text) {
            if(MessageText == null) return;

            MessageText.text = text;            
            if(_updateWindowSizeToTextSize) {
                MessageText.ForceMeshUpdate();   // This is called so we can update the size of the popup
                UpdateWindowSize();
            }
        }
        void UpdateWindowSize() {
            if(MessageText == null) return;
            if(Root == null) return;

            Vector2 textSize = MessageText.GetRenderedValues(false);
            Vector2 paddingValue = new Vector2(Mathf.Abs(MessageText.GetComponent<RectTransform>().anchoredPosition.x), Mathf.Abs(MessageText.GetComponent<RectTransform>().anchoredPosition.y));   // Define padding size
            Vector2 paddingSize = new Vector2(paddingValue.x, paddingValue.y) * 2;   // We take the x position which is always the same and apply the same to the y, multiply padding with 2 to pad the other side too
            Vector2 backgroundSize = textSize + paddingSize;

            Root.sizeDelta = backgroundSize;
        }
        void PositionPopup(RectTransform rectPosition) {
            if(Root == null) return;
            if(_container == null) return;

            Root.anchorMin = rectPosition.anchorMin;
            Root.anchorMax = rectPosition.anchorMax;
            Root.pivot = rectPosition.pivot;
            //Root.anchoredPosition = rectPosition.anchoredPosition;
            Root.position = rectPosition.position;
        }
        void ClosePopup(MicroInteractable interactable) {
            if(_closeButton != null) _closeButton.gameObject.SetActive(false);   // This is here so mouse can return to normal
            if(_closeAction != null) _closeAction?.Invoke(ID);
            OnClose?.Invoke(this, ID);
            Destroy(gameObject);
        }
        #endregion
    }
}