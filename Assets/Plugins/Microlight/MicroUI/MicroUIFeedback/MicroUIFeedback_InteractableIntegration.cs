using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // MicroUI feedbacks integration for the MicroUI interactable element
    // ****************************************************************************************************
    public class MicroUIFeedback_InteractableIntegration : MonoBehaviour {
        MicroInteractable _interactable;
        bool _subscribedToEvents;

        [SerializeField] MicroUIFeedbackAnimations _hoverAnimation;
        [SerializeField] MicroUIFeedbackAnimations _unhoverAnimation;

        void Start() {
            _interactable = GetComponent<MicroInteractable>();
            if(!_subscribedToEvents && _interactable != null) {
                _subscribedToEvents = true;
                _interactable.OnHoverChange += OnHoverChange;
            }
        }
        private void OnDestroy() {
            _subscribedToEvents = false;
            _interactable.OnHoverChange -= null;
        }

        void OnHoverChange(MicroInteractable interactable) {
            if(interactable.IsHovered) MicroUIFeedbacks.PlayAnimation(_interactable.transform, _hoverAnimation);
            else MicroUIFeedbacks.PlayAnimation(_interactable.transform, _unhoverAnimation);
        }
    }
}