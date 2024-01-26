using UnityEngine;

namespace Microlight.MicroUI {
    public class DemoScene : MonoBehaviour {
        [SerializeField] MicroButton _modalButton;
        [SerializeField] MicroButton _popupButton;
        [SerializeField] MicroTooltipClient _toggleButtonTooltipClient;

        [Header("Popup spawn positions")]
        [SerializeField] RectTransform _popupSpawnPos1;

        private void Start() {
            _modalButton.OnClick += OpenModal;
            _popupButton.OnClick += OpenPopup;

            _toggleButtonTooltipClient.StaticMessage = "Tooltip is now binded\nto modal button";
            _toggleButtonTooltipClient.BindInteractable(_modalButton);
        }

        void OpenModal(MicroInteractable interactable) {
            MicroUI.ModalController.SpawnModalWindow("Are you sure?", ModalConfirm, ModalDecline);
        }
        void ModalConfirm() {
            MicroNotificationWindow notificationWindow = MicroUI.NotificationController.QueueNotification("Modal said YES!");
            notificationWindow.MessageText.color = Color.green;
        }
        void ModalDecline() {
            MicroNotificationWindow notificationWindow = MicroUI.NotificationController.QueueNotification("Modal said No\n:(");
            notificationWindow.MessageText.color = Color.black;
        }

        void OpenPopup(MicroInteractable interactable) {
            MicroUI.PopupController.SpawnPopup("Are you sure?", _popupSpawnPos1, PopupConfirm);
        }
        void PopupConfirm(int id) {
            MicroUI.NotificationController.QueueNotification("Closed Popup!");
        }
    }
}