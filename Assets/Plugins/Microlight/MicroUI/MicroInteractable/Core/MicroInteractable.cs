using System;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Base class for interactable MicroUI elements
    // ****************************************************************************************************
    public abstract class MicroInteractable :
        MonoBehaviour, IMicroClickable,
        IBeginDragHandler, IEndDragHandler, IDragHandler,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

        // Properties
        [SerializeField] protected bool _interactable = true;
        public bool Interactable {
            get {
                if(!gameObject.activeInHierarchy) return false;
                else return _interactable;
            }
            set {
                if(_interactable == value) return;
                _interactable = value;
                if(value) ResetInteraction();   // If turning interactable on, make sure to reset its interaction state and check where the mouse is
                else if(!value) {
                    IsClicked = false;
                    IsHovered = false;
                    IsDragged = false;
                }

                OnInteractableChange?.Invoke(this);
            }
        }
        [SerializeField] protected bool _draggable;
        public bool Draggable {
            get => _draggable;
            set {
                if(_draggable == value) return;
                _draggable = value;
                OnDraggableChange?.Invoke(this);
            }
        }
        protected bool _isClicked;
        public bool IsClicked { 
            get => _isClicked;
            protected set {
                if(_isClicked == value) return;
                _isClicked = value;
                OnClickedChange?.Invoke(this);
            }
        }
        protected bool _isHovered;
        public bool IsHovered {
            get => _isHovered;
            protected set {
                if(_isHovered == value) return;
                _isHovered = value;
                OnHoverChange?.Invoke(this);
            }
        }
        protected bool _isDragged;
        public bool IsDragged {
            get => _isDragged;
            protected set {
                if(_isDragged == value) return;
                _isDragged = value;
                OnIsDraggedChange?.Invoke(this);
            }
        }
        [SerializeField] protected MicroCursorClient _microCursorClient;

        // Pointer data for dragging object
        protected Vector2 _pointerDragStartPosition;
        public Vector2 PointerDragStartPosition {
            get => _pointerDragStartPosition;
            protected set => _pointerDragStartPosition = value;
        }
        protected Vector2 _pointerDragPreviousFramePosition;
        public Vector2 PointerDragPreviousFramePosition {
            get => _pointerDragPreviousFramePosition;
            protected set => _pointerDragPreviousFramePosition = value;
        }

        // Events
        public event Action<MicroInteractable> OnInteractableChange;
        public event Action<MicroInteractable> OnClickedChange;
        public event Action<MicroInteractable> OnHoverChange;
        public event Action<MicroInteractable> OnDraggableChange;
        public event Action<MicroInteractable> OnIsDraggedChange;
        public event Action<MicroInteractable> OnClick;
        public event Action<MicroInteractable, Vector2, Vector2> OnDragged;

        // Update upon enabling
        virtual protected void OnEnable() {
            if(_microCursorClient == null) _microCursorClient = GetComponent<MicroCursorClient>();
            ResetInteraction();
        }
        virtual protected void OnDestroy() {
            OnInteractableChange = null;
            OnClickedChange = null;
            OnHoverChange = null;
            OnDraggableChange = null;
            OnIsDraggedChange = null;
            OnClick = null;
            OnDragged = null;
        }

        #region Public
        /// <summary>
        /// Resets state of the interactable element, usually used after enabling button again
        /// </summary>
        public void ResetInteraction() {
            IsDragged = false;
            IsClicked = false;
            IsHovered = MicroUIUtilities.IsMouseOverElement(gameObject);
        }
        #endregion

        #region Pointer events
        // Drag
        virtual public void OnBeginDrag(PointerEventData eventData) {
            if(!Interactable || !Draggable) return;   // If not interactable or not draggable

            PointerDragStartPosition = Input.mousePosition;
            PointerDragPreviousFramePosition = PointerDragStartPosition;
            IsDragged = true;
            if(MicroUI.CursorController != null) MicroUI.CursorController.Dragging = true;
            UpdateCursorClient();
        }
        virtual public void OnEndDrag(PointerEventData eventData) {
            if(!Interactable || !Draggable) return;

            IsClicked = false;
            IsDragged = false;
            if(MicroUI.CursorController != null) MicroUI.CursorController.Dragging = false;
            UpdateCursorClient();
        }
        virtual public void OnDrag(PointerEventData eventData) {
            if(!Interactable || !Draggable) {
                IsDragged = false;
                return;
            }
            if(!IsDragged) return;

            OnDragged?.Invoke(this, PointerDragPreviousFramePosition, Input.mousePosition);
            PointerDragPreviousFramePosition = Input.mousePosition;
        }

        // Hover
        virtual public void OnPointerEnter(PointerEventData eventData) {
            if(!Interactable) return;   // If not interactable
            IsHovered = true;
            UpdateCursorClient();
        }
        virtual public void OnPointerExit(PointerEventData eventData) {
            if(!Interactable) return;   // If not interactable
            IsHovered = false;
        }

        // Click
        virtual public void OnPointerUp(PointerEventData eventData) {
            if(!Interactable) return;   // If not interactable
            IsClicked = false;
            UpdateCursorClient();
        }
        virtual public void OnPointerDown(PointerEventData eventData) {
            if(!Interactable) return;   // If not interactable
            IsClicked = true;
            UpdateCursorClient();
        }
        virtual public void OnPointerClick(PointerEventData eventData) {
            if(!Interactable) return;   // If not interactable
            OnClick?.Invoke(this);
        }
        #endregion

        #region IMicroClickable
        /// <summary>
        /// Simulates click
        /// </summary>
        virtual public void Click() => OnPointerClick(null);
        /// <summary>
        /// Forcefully triggers on click event
        /// </summary>
        virtual public void ForceClick() => OnClick.Invoke(this);
        /// <summary>
        /// Returns game object of interactable, required by interface
        /// </summary>
        virtual public GameObject GetGameObject() => gameObject;
        virtual protected void UpdateCursorClient() {
            if(_microCursorClient == null) return;

            if(!Interactable) _microCursorClient.State = MicroCursorStates.NonInteractable;
            else if(IsClicked) _microCursorClient.State = MicroCursorStates.Clicked;
            else _microCursorClient.State = MicroCursorStates.Interactable;

            if(IsHovered) _microCursorClient.OnPointerEnter(null);
        }
        #endregion
    }

    #region Custom Editor
#if UNITY_EDITOR
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    [CustomEditor(typeof(MicroInteractable))]
    public class MicroInteractable_Editor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // Store serialized properties
            SerializedProperty interactable = serializedObject.FindProperty("_interactable");
            SerializedProperty draggable = serializedObject.FindProperty("_draggable");
            SerializedProperty microCursorClient = serializedObject.FindProperty("_microCursorClient");

            // Settings
            EditorGUILayout.LabelField("Interactable Core", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(interactable, new GUIContent("Interactable", "Can interactable be interacted with."));
            EditorGUILayout.PropertyField(draggable, new GUIContent("Draggable", "Will interactable report back the drag callbacks."));
            EditorGUILayout.PropertyField(microCursorClient, new GUIContent("Cursor Client", "Allows integration of the MicroCursor."));

            serializedObject.ApplyModifiedProperties();   // Apply changes
        }
    }
#endif
    #endregion
}