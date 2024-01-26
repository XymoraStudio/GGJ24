using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Button element of MicroUI library
    // ****************************************************************************************************
    public class MicroButton : MicroInteractable {
        // Integration
        [SerializeField] protected bool _integratedSprites;
        public bool IntegratedSprites {
            get => _integratedSprites;
            set => _integratedSprites = value;
        }
        [SerializeField] protected InteractableSpriteSet _spriteSet;
        public InteractableSpriteSet SpriteSet {
            get => _spriteSet;
            set => _spriteSet = value;
        }
        [SerializeField] protected bool _integratedColors;
        public bool IntegratedColors {
            get => _integratedColors;
            set => _integratedColors = value;
        }
        [SerializeField] protected InteractableColorSet _colorSet;
        public InteractableColorSet ColorSet {
            get => _colorSet;
            set => _colorSet = value;
        }

        // Properties
        [SerializeField] protected Image _image;
        public Image Image => _image;
        [SerializeField] protected TMP_Text _text;
        public TMP_Text Text => _text;

        // Event handling
        override protected void OnEnable() {
            base.OnEnable();
            OnInteractableChange += InteractableChange;
            OnIsDraggedChange += IsDraggedChange;
            OnHoverChange += HoveredChange;
            OnClickedChange += ClickedChange;
            OnClick += Click;

            CheckReferences();
            UpdateVisuals();
        }
        virtual protected void OnDisable() {          
            OnIsDraggedChange -= IsDraggedChange;
            OnHoverChange -= HoveredChange;
            OnClickedChange -= ClickedChange;
            OnClick -= Click;
        }

        /// <summary>
        /// Checks if references are done correctly
        /// </summary>
        virtual protected void CheckReferences() {
            // Checks
            if(IntegratedSprites || IntegratedColors)
                if(Image == null) Debug.LogWarning("MicroButton: Image reference is required for selected features yet it is not referenced.");
            if(IntegratedSprites)
                if(SpriteSet == null) Debug.LogWarning("MicroButton: Integrated sprites are enabled but not referenced.");
            if(IntegratedColors)
                if(ColorSet == null) Debug.LogWarning("MicroButton: Integrated colors are enabled but not referenced.");
        }

        #region Pointer Events
        virtual protected void InteractableChange(MicroInteractable interactable) {
            UpdateVisuals();
        }
        virtual protected void IsDraggedChange(MicroInteractable interactable) {
            UpdateVisuals();
        }
        virtual protected void HoveredChange(MicroInteractable interactable) {
            UpdateVisuals();
        }
        virtual protected void ClickedChange(MicroInteractable interactable) {
            UpdateVisuals();
        }
        virtual protected void Click(MicroInteractable interactable) {
            UpdateVisuals();
        }
        #endregion

        /// <summary>
        /// Updates visual appearance of the button based on settings
        /// </summary>
        virtual public void UpdateVisuals() {
            if(Image == null) return;   // Can't update visuals if image is not referenced
            if(IntegratedSprites && SpriteSet != null) UpdateSprite();
            if(IntegratedColors && ColorSet != null) UpdateColor();

            void UpdateSprite() {
                if(!Interactable) Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Disabled);
                else if(IsClicked) Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Pressed);
                else if(IsHovered && !IsDragged) Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Highlighted);
                else Image.sprite = SpriteSet.SpriteByState(MicroUIStates.Normal);
            }
            void UpdateColor() {
                if(!Interactable) Image.color = ColorSet.ColorByState(MicroUIStates.Disabled);
                else if(IsClicked) Image.color = ColorSet.ColorByState(MicroUIStates.Pressed);
                else if(IsHovered && !IsDragged) Image.color = ColorSet.ColorByState(MicroUIStates.Highlighted);
                else Image.color = ColorSet.ColorByState(MicroUIStates.Normal);
            }
        }
    }

    #region Custom Editor
#if UNITY_EDITOR
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    [CustomEditor(typeof(MicroButton))]
    public class MicroButton_Editor : MicroInteractable_Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            serializedObject.Update();

            // Store serialized properties
            SerializedProperty integratedSprites = serializedObject.FindProperty("_integratedSprites");
            SerializedProperty spriteSet = serializedObject.FindProperty("_spriteSet");
            SerializedProperty integratedColors = serializedObject.FindProperty("_integratedColors");
            SerializedProperty colorSet = serializedObject.FindProperty("_colorSet");

            SerializedProperty image = serializedObject.FindProperty("_image");
            SerializedProperty text = serializedObject.FindProperty("_text");

            // Visuals
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Visuals", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(integratedSprites, new GUIContent("Enable Integrated Sprites", "Should button automatically handle sprites between states."));
            if(integratedSprites.boolValue) EditorGUILayout.PropertyField(spriteSet, new GUIContent("Sprite Set", "Sprite set for displaying button in states."));
            EditorGUILayout.PropertyField(integratedColors, new GUIContent("Enable Integrated Colors", "Should button automatically handle color between states."));
            if(integratedColors.boolValue) EditorGUILayout.PropertyField(colorSet, new GUIContent("Color Set", "Color set for displaying button in states."));

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