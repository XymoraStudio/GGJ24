using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Scriptable object for storing visuals of interactable elements.
    // ****************************************************************************************************
    [CreateAssetMenu(fileName = "InteractableColorSet", menuName = "Microlight/Micro UI/Interactable Color Set")]
    public class InteractableColorSet : ScriptableObject {
        [SerializeField] MicroUIElements _forElement;   // Just for visual representation, for easier choosing of colors

        [SerializeField] Color _normal = new Color(0.9f, 0.9f, 0.9f, 1f);
        public Color Normal => _normal;
        [SerializeField] Color _highlighted = new Color(1f, 1f, 1f, 1f);
        public Color Highlighted => _highlighted;
        [SerializeField] Color _pressed = new Color(0.75f, 0.75f, 0.75f, 1f);
        public Color Pressed => _pressed;
        [SerializeField] Color _disabled = new Color(0.5f, 0.5f, 0.5f, 1f);
        public Color Disabled => _disabled;
        [SerializeField] Color _selected = new Color(1f, 1f, 1f, 1f);
        public Color Selected => _selected;
        [SerializeField] Color _focused = new Color(1f, 1f, 1f, 1f);
        public Color Focused => _focused;

        /// <summary>
        /// Returns the color based on passed state
        /// </summary>
        public Color ColorByState(MicroUIStates state) {
            return state switch {
                MicroUIStates.Normal => _normal,
                MicroUIStates.Highlighted => _highlighted,
                MicroUIStates.Pressed => _pressed,
                MicroUIStates.Disabled => _disabled,
                MicroUIStates.Selected => _selected,
                MicroUIStates.Focused => _focused,
                _ => _normal,
            };
        }
    }

    #region Custom Editor
#if UNITY_EDITOR
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    [CustomEditor(typeof(InteractableColorSet))]
    public class InteractableColorSet_Editor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // Store serialized properties
            SerializedProperty forElement = serializedObject.FindProperty("_forElement");

            // Settings
            EditorGUILayout.PropertyField(forElement, new GUIContent("Element", "Help with visualizing which colors should be filled in."));

            // Visuals
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"), new GUIContent("Normal", "Default color for the button."));
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