using System;
using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Client communicates to the controller when tooltip should be shown
    // ****************************************************************************************************
    public class MicroTooltipClient : MonoBehaviour {
        [SerializeField] bool _interactable = true;
        public bool Interactable {
            get => _interactable;
            set => _interactable = value;
        }
        [SerializeField] bool _staticTooltip;   // If turned on, it allows not referencing _tooltipContentSource and showing static message
        [SerializeField] string _staticMessage;
        public string StaticMessage {
            get => _staticMessage;
            set => _staticMessage = value;
        }
        [SerializeField] MicroTooltipWindow _targetWindow;

        // Variables
        MicroInteractable _bindedInteractable;   // Interactable that triggers this tooltip client
        IMicroTooltipContent _tooltipContentSource;
        public IMicroTooltipContent TooltipContentSource {
            get => _tooltipContentSource;
            set => _tooltipContentSource = value;
        }

        // Events
        public event Action<MicroTooltipClient> OnShow;
        public event Action<MicroTooltipClient> OnHide;

        private void Start() {
            if(_bindedInteractable == null) BindInteractable(gameObject.GetComponent<MicroInteractable>());
        }
        protected void CheckReferences() {
            if(_bindedInteractable == null) {
                Debug.LogWarning("MicroTooltip: Binded interactable is not referenceds.");
            }
            if(!_staticTooltip && _tooltipContentSource == null) {
                Debug.LogWarning("MicroTooltip: Content source is not referenced.");
            }            
        }
        private void OnDestroy() {
            if(_bindedInteractable != null) _bindedInteractable.OnHoverChange -= InteractableHoverChange;
        }

        public void BindInteractable(MicroInteractable interactable) {
            if(interactable == null) return;

            if(_bindedInteractable != null) _bindedInteractable.OnHoverChange -= InteractableHoverChange;
            _bindedInteractable = interactable;
            _bindedInteractable.OnHoverChange += InteractableHoverChange;
        }
        void InteractableHoverChange(MicroInteractable interactable) {
            if(!Interactable) return;
            if(!_staticTooltip && _tooltipContentSource == null) return;

            if(!interactable.IsHovered) {
                MicroUI.TooltipController.DisplayChangeWindow(false, _targetWindow);
                OnHide?.Invoke(this);
            }
            else {
                MicroUI.TooltipController.DisplayChangeWindow(true, _targetWindow);
                string message = _staticTooltip ? _staticMessage : _tooltipContentSource.GetMicroTooltipContent();
                MicroUI.TooltipController.UpdateWindowText(message);
                OnShow?.Invoke(this);
            }            
        }
    }
}