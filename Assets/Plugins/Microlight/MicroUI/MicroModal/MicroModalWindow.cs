using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Data for the modal window, binds with modal window that is created
    // ****************************************************************************************************
    public class MicroModalWindow : MonoBehaviour {
        [Header("UI")]
        [SerializeField] RectTransform _root;
        public RectTransform Root => _root;
        [SerializeField] Image _windowImage;
        public Image WindowImage => _windowImage;
        [SerializeField] TMP_Text _messageText;
        public TMP_Text MessageText => _messageText;

        [Header("Buttons")]
        [SerializeField] MicroButton _confirmButton;
        public MicroButton ConfirmButton => _confirmButton;
        [SerializeField] MicroButton _declineButton;
        public MicroButton DeclineButton => _declineButton;

        // Delegates
        public delegate void ConfirmAction();
        ConfirmAction _confirmAction;
        public delegate void DeclineAction();
        DeclineAction _declineAction;

        // Events
        public event Action<MicroModalWindow> OnShow;
        public event Action<MicroModalWindow> OnClose;

        // Other
        MicroUIShortcutGroup _uiGroup;

        #region Initialization
        private void Awake() {
            CheckReferences();
            if(Root != null) Root.gameObject.SetActive(false);
        }
        protected void CheckReferences() {
            if(Root == null) {
                Debug.LogWarning("MicroModal: Root is not referenced.");
            }
            if(MessageText == null) {
                Debug.LogWarning("MicroModal: MessageText is not referenced.");
            }
            if(WindowImage == null) {
                Debug.LogWarning("MicroModal: WindowImage is not referenced.");
            }
            if(ConfirmButton == null) {
                Debug.LogWarning("MicroModal: ConfirmButton is not referenced.");
            }
            if(DeclineButton == null) {
                Debug.LogWarning("MicroModal: DeclineButton is not referenced.");
            }
        }
        private void OnEnable() {
            if(ConfirmButton != null) ConfirmButton.OnClick += ConfirmClick;
            if(DeclineButton != null) DeclineButton.OnClick += DeclineClick;
        }
        private void OnDisable() {
            if(ConfirmButton != null) ConfirmButton.OnClick -= ConfirmClick;
            if(DeclineButton != null) DeclineButton.OnClick -= DeclineClick;
        }
        private void OnDestroy() {
            if(MicroUIShortcutController.Instance != null && _uiGroup != null) MicroUIShortcutController.Instance.RemoveGroup(_uiGroup);
        }
        #endregion

        #region Window managment
        internal void ShowWindow(string message, ConfirmAction confirm, DeclineAction decline) {
            if(Root == null) return;

            Root.gameObject.SetActive(true);
            MessageText.text = message;

            _confirmAction = confirm;
            _declineAction = decline;

            _uiGroup = new MicroUIShortcutGroup(ConfirmButton, DeclineButton, MicroUIShortcutController.MODAL_PRIORITY);
            if(MicroUIShortcutController.Instance != null) MicroUIShortcutController.Instance.AddGroup(_uiGroup);
            OnShow?.Invoke(this);
        }
        void CloseWindow() {
            if(MicroUIShortcutController.Instance != null && _uiGroup != null) {
                MicroUIShortcutController.Instance.RemoveGroup(_uiGroup);
                _uiGroup = null;
            }
            OnClose?.Invoke(this);
            Destroy(Root.gameObject);
        }
        #endregion

        #region User interaction
        void ConfirmClick(MicroInteractable interactable) {
            _confirmAction?.Invoke();
            CloseWindow();
        }
        void DeclineClick(MicroInteractable interactable) {
            _declineAction?.Invoke();
            CloseWindow();
        }
        #endregion
    }
}