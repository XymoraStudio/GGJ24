using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Toggleable button element of MicroUI library
    // ****************************************************************************************************
    public class MicroToggleButton : MicroButton {
        // Integration
        [SerializeField] protected InteractableSpriteSet _spriteOffSet;
        public InteractableSpriteSet SpriteOffSet {
            get => _spriteOffSet;
            set => _spriteOffSet = value;
        }
        [SerializeField] protected InteractableColorSet _colorOffSet;
        public InteractableColorSet ColorOffSet {
            get => _colorOffSet;
            set => _colorOffSet = value;
        }

        // Properties
        [SerializeField] protected bool _toggled;
        public bool Toggled {
            get => _toggled;
            set {
                if(_toggled == value) return;
                _toggled = value;
                OnToggledChange?.Invoke(this);
            }
        }
        protected bool _canBeToggled = true;
        public bool CanBeToggled {
            get => _canBeToggled;
            set => _canBeToggled = value;
        }

        // Events
        public event Action<MicroToggleButton> OnToggledChange;

        // Event handling
        override protected void OnEnable() {
            OnToggledChange += ToggledChange;
            OnClick += Toggle;
            
            base.OnEnable();
        }
        override protected void OnDisable() {
            base.OnDisable();
            OnToggledChange -= ToggledChange;
            OnClick -= Toggle;
        }
        override protected void OnDestroy() {
            base.OnDestroy();
            OnToggledChange = null;
        }

        /// <summary>
        /// Checks if references are done correctly
        /// </summary>
        //override protected void CheckReferences() {
        //    base.CheckReferences();
        //    // Checks
        //    if(_integratedSprites)
        //        if(_spriteOffSet == null) Debug.LogWarning("MicroButton: Integrated sprites are enabled but off set is not referenced.");
        //    if(_integratedColors)
        //        if(_colorOffSet == null) Debug.LogWarning("MicroButton: Integrated colors are enabled but off set is not referenced.");
        //}        

        #region Pointer Events
        virtual protected void ToggledChange(MicroToggleButton toggledButton) {
            UpdateVisuals();
        }
        virtual protected void Toggle(MicroInteractable interactable) {
            if(!CanBeToggled) return;
            Toggled = !Toggled;
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
                // Determine which set to use
                InteractableSpriteSet usedSet;
                if(Toggled) usedSet = SpriteSet;
                else {
                    if(SpriteOffSet != null) usedSet = SpriteOffSet;
                    else usedSet = SpriteSet;
                }

                if(!Interactable) Image.sprite = usedSet.SpriteByState(MicroUIStates.Disabled);
                else if(IsClicked) Image.sprite = usedSet.SpriteByState(MicroUIStates.Pressed);
                else if(IsHovered && !IsDragged) Image.sprite = usedSet.SpriteByState(MicroUIStates.Highlighted);
                else Image.sprite = usedSet.SpriteByState(MicroUIStates.Normal);
            }
            void UpdateColor() {
                // Determine which set to use
                InteractableColorSet usedSet;
                if(Toggled) usedSet = ColorSet;
                else {
                    if(ColorOffSet != null) usedSet = ColorOffSet;
                    else usedSet = ColorSet;
                }

                if(!Interactable) Image.color = usedSet.ColorByState(MicroUIStates.Disabled);
                else if(IsClicked) Image.color = usedSet.ColorByState(MicroUIStates.Pressed);
                else if(IsHovered && !IsDragged) Image.color = usedSet.ColorByState(MicroUIStates.Highlighted);
                else Image.color = usedSet.ColorByState(MicroUIStates.Normal);
            }
        }
        protected override void UpdateCursorClient() {
            if(_microCursorClient == null) return;

            if(!CanBeToggled) _microCursorClient.State = MicroCursorStates.NonInteractable;
            else {
                base.UpdateCursorClient();
                return;
            }

            if(IsHovered) _microCursorClient.OnPointerEnter(null);
        }
    }

    #region Custom Editor
#if UNITY_EDITOR
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    [CustomEditor(typeof(MicroToggleButton))]
    public class MicroToggleButton_Editor : MicroInteractable_Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            serializedObject.Update();

            // Store serialized properties
            SerializedProperty integratedSprites = serializedObject.FindProperty("_integratedSprites");
            SerializedProperty spriteSet = serializedObject.FindProperty("_spriteSet");
            SerializedProperty spriteOffSet = serializedObject.FindProperty("_spriteOffSet");
            SerializedProperty integratedColors = serializedObject.FindProperty("_integratedColors");
            SerializedProperty colorSet = serializedObject.FindProperty("_colorSet");
            SerializedProperty colorOffSet = serializedObject.FindProperty("_colorOffSet");

            SerializedProperty toggled = serializedObject.FindProperty("_toggled");

            SerializedProperty image = serializedObject.FindProperty("_image");
            SerializedProperty text = serializedObject.FindProperty("_text");

            // Toggle button
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Toggle", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(toggled, new GUIContent("Toggled", "Should button be toggled on or off on start."));

            // Visuals
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Visuals", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(integratedSprites, new GUIContent("Enable Integrated Sprites", "Should button automatically handle sprites between states."));
            if(integratedSprites.boolValue) EditorGUILayout.PropertyField(spriteSet, new GUIContent("Sprite On Set", "Sprite set for button in on state."));
            if(integratedSprites.boolValue) EditorGUILayout.PropertyField(spriteOffSet, new GUIContent("Sprite Off Set", "Sprite set for button in off state."));
            EditorGUILayout.PropertyField(integratedColors, new GUIContent("Enable Integrated Colors", "Should button automatically handle color between states."));
            if(integratedColors.boolValue) EditorGUILayout.PropertyField(colorSet, new GUIContent("Color On Set", "Color set for displaying button in on state."));
            if(integratedColors.boolValue) EditorGUILayout.PropertyField(colorOffSet, new GUIContent("Color Off Set", "Color set for displaying button in off state."));

            // Image and Text
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(image, new GUIContent("Target Graphics", "Reference to the graphics of the button."));
            EditorGUILayout.PropertyField(text, new GUIContent("Target Text", "Reference to the text element of the button."));

            serializedObject.ApplyModifiedProperties();   // Apply changes
        }
    }
#endif
    #endregion
}