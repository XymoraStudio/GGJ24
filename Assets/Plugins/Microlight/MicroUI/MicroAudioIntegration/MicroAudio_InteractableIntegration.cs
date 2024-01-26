using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // MicroAudio integration for the MicroUI interactable
    // ****************************************************************************************************
    public class MicroAudio_InteractableIntegration : MonoBehaviour {
        [SerializeField] MicroAudioSet _soundSet;
        MicroInteractable _interactable;

        private void Start() {
            _interactable = GetComponent<MicroInteractable>();
            OnEnable();
        }
        private void OnEnable() {
            if(_interactable == null) return;
            _interactable.OnHoverChange += HoverChange;
            _interactable.OnClick += Clicked;
        }
        private void OnDisable() {
            if(_interactable == null) return;
            _interactable.OnHoverChange -= HoverChange;
            _interactable.OnClick -= Clicked;
        }

        void HoverChange(MicroInteractable interactable) {
            if(interactable.IsHovered) {
                _soundSet.PlayHover();
            }
        }
        void Clicked(MicroInteractable interactable) {
            _soundSet.PlayClick();
        }
    }
}