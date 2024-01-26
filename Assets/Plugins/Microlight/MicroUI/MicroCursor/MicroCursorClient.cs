using UnityEngine;
using UnityEngine.EventSystems;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Goes onto any element that wants to control how cursor looks when interacted
    // For UI element it needs image that has raycast target
    // For GameObjects requires collider as well as PhysicsRaycaster on camera
    // ****************************************************************************************************
    public class MicroCursorClient : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] MicroCursorStates _state;
        public MicroCursorStates State {
            get => _state;
            set {
                if(_state == value) return;
                _state = value;
                if(MicroUIUtilities.IsMouseOverElement(gameObject)) MicroUI.CursorController.CurrentState = _state;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => MicroUI.CursorController.CurrentState = _state;
        public void OnPointerExit(PointerEventData eventData) => MicroUI.CursorController.CurrentState = MicroCursorStates.Normal;
    }
}