using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Scriptable object for storing visuals of interactable elements.
    // ****************************************************************************************************
    [CreateAssetMenu(fileName = "InteractableSpriteSet", menuName = "Microlight/Micro UI/Interactable Sprite Set")]
    public class InteractableSpriteSet : ScriptableObject {
        [SerializeField] MicroUIElements _forElement;   // Just for visual representation, for easier filling of sprites

        [SerializeField] Sprite _normal;
        public Sprite Normal => _normal;
        [SerializeField] Sprite _highlighted;
        public Sprite Highlighted => _highlighted;
        [SerializeField] Sprite _pressed;
        public Sprite Pressed => _pressed;
        [SerializeField] Sprite _disabled;
        public Sprite Disabled => _disabled;
        [SerializeField] Sprite _selected;
        public Sprite Selected => _selected;
        [SerializeField] Sprite _focused;
        public Sprite Focused => _focused;

        /// <summary>
        /// Returns the sprite based on passed state
        /// </summary>
        public Sprite SpriteByState(MicroUIStates state) {
            return state switch {
                MicroUIStates.Normal => _normal,
                MicroUIStates.Highlighted => _highlighted == null ? _normal : _highlighted,
                MicroUIStates.Pressed => _pressed == null ? _normal : _pressed,
                MicroUIStates.Disabled => _disabled == null ? _normal : _disabled,
                MicroUIStates.Selected => _selected == null ? _normal : _selected,
                MicroUIStates.Focused => _focused == null ? (_highlighted == null ? _normal : _highlighted) : _focused,
                _ => _normal,
            };
        }
    }

    #region Custom Editor
#if UNITY_EDITOR
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    [CustomEditor(typeof(InteractableSpriteSet))]
    public class InteractableSpriteSet_Editor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // Store serialized properties
            SerializedProperty forElement = serializedObject.FindProperty("_forElement");

            // Settings
            EditorGUILayout.PropertyField(forElement, new GUIContent("Element", "Help with visualizing which sprites should be filled in."));

            // Visuals
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sprites", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"), new GUIContent("Normal", "Default sprite for the button."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_highlighted"), new GUIContent("Highlighted", "When hovering over button."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"), new GUIContent("Pressed", "When pointer is clicked/pressed on button but not released."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_disabled"), new GUIContent("Disabled", "When button can't be interacted with."));
            switch(forElement.enumValueIndex) {
                case (int)MicroUIElements.Button:
                    break;
                case (int)MicroUIElements.ToggleButton:
                    break;
                case (int)MicroUIElements.RadioButton:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_selected"), new GUIContent("Selected", "When button is selected as a group choice."));
                    break;
                default:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_selected"), new GUIContent("Selected", "When button is selected, like radio button."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_focused"), new GUIContent("Focused", "When button is focused by controller/keyboard."));
                    break;
            }

            serializedObject.ApplyModifiedProperties();   // Apply changes
        }
    }
#endif
    #endregion
}