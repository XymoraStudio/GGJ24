using System;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Radio button element of MicroUI library
    // ****************************************************************************************************
    public class MicroRadioButton : MicroButton {
        // Properties
        [SerializeField] protected bool _selected;
        public bool Selected {
            get => _selected;
            set {
                if(_selected == value) return;
                _selected = value;
                OnSelectedChange?.Invoke(this);              
            }
        }

        // Events
        public event Action<MicroRadioButton> OnSelectedChange;

        // Event handling
        override protected void OnEnable() {
            OnSelectedChange += SelectedChange;

            base.OnEnable();
        }
        override protected void OnDisable() {
            base.OnDisable();
            OnSelectedChange -= SelectedChange;
        }
        override protected void OnDestroy() {
            base.OnDestroy();
            OnSelectedChange = null;
        }

        #region Pointer Events
        virtual protected void SelectedChange(MicroRadioButton toggledButton) {
            // If its selected, button can't be interacted with
            if(Selected) {
                IsClicked = false;
                IsHovered = false;
                IsDragged = false;
            }
            UpdateVisuals();
        }
        // Drag
        override public void OnBeginDrag(PointerEventData eventData) {
            if(Selected) return;
            base.OnBeginDrag(eventData);
        }
        override public void OnEndDrag(PointerEventData eventData) {
            if(Selected) return;
            base.OnEndDrag(eventData);
        }
        override public void OnDrag(PointerEventData eventData) {
            if(Selected) return;
            base.OnDrag(eventData);
        }

        // Hover
        override public void OnPointerEnter(PointerEventData eventData) {
            if(Selected) return;
            base.OnPointerEnter(eventData);
        }
        override public void OnPointerExit(PointerEventData eventData) {
            if(Selected) return;
            base.OnPointerExit(eventData);
        }

        // Click
        override public void OnPointerUp(PointerEventData eventData) {
            if(Selected) return;
            base.OnPointerUp(eventData);
        }
        override public void OnPointerDown(PointerEventData eventData) {
            if(Selected) return;
            base.OnPointerDown(eventData);
        }
        override public void OnPointerClick(PointerEventData eventData) {
            if(Selected) return;
            base.OnPointerClick(eventData);
        }
        #endregion

        /// <summary>
        /// Updates visual appearance of the button based on settings
        /// </summary>
        override public void UpdateVisuals() {
            if(Image == null) return;   // Can't update visuals if image is not referenced
            if(IntegratedSprites && SpriteSet != null) UpdateSprite();
            if(IntegratedColors && ColorSet != null) UpdateColor();

            void UpdateSprite() {
                if(Selected) Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Selected);
                else if(!Interactable) Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Disabled);
                else if(IsClicked) Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Pressed);
                else if(IsHovered && !IsDragged) Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Highlighted);
                else Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Normal);
            }
            void UpdateColor() {
                if(Selected) Image.color = ColorSet.ColorByState(MicroUIStates.Selected);
                else if(!Interactable) Image.color = ColorSet.ColorByState(MicroUIStates.Disabled);
                else if(IsClicked) Image.color = ColorSet.ColorByState(MicroUIStates.Pressed);
                else if(IsHovered && !IsDragged) Image.color = ColorSet.ColorByState(MicroUIStates.Highlighted);
                else Image.color = ColorSet.ColorByState(MicroUIStates.Normal);
            }
        }
        protected override void UpdateCursorClient() {
            if(_microCursorClient == null) return;

            if(!Interactable) _microCursorClient.State = MicroCursorStates.NonInteractable;
            else if(Selected) _microCursorClient.State = MicroCursorStates.Normal;
            else {
                base.UpdateCursorClient();
            }

            if(IsHovered) _microCursorClient.OnPointerEnter(null);
        }
    }

    #region Custom Editor
#if UNITY_EDITOR
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    [CustomEditor(typeof(MicroRadioButton))]
    public class MicroRadioButton_Editor : MicroButton_Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
#endif
    #endregion
}