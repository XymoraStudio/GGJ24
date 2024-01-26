using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Base of MicroUI framework. Should be on every scene to designate where windows should appear
    // ****************************************************************************************************
    public class MicroUI : MonoBehaviour {
        // Modal
        static MicroModalController _modalController;
        public static MicroModalController ModalController => _modalController;
        [SerializeField] MicroModalWindow _modalWindowPrefab;
        [SerializeField] RectTransform _modalWindowsContainer;

        // Notification
        static MicroNotificationController _notificationController;
        public static MicroNotificationController NotificationController => _notificationController;
        [SerializeField] MicroNotificationWindow _notificationWindowPrefab;
        [SerializeField] RectTransform _notificationWindowsContainer;
        [SerializeField] float _notificationDefaultDuration = 3f;

        // Popup
        static MicroPopupController _popupController;
        public static MicroPopupController PopupController => _popupController;
        [SerializeField] MicroPopupWindow _popupWindowPrefab;
        [SerializeField] RectTransform _popupWindowsContainer;

        // Tooltip
        static MicroTooltipController _tooltipController;
        public static MicroTooltipController TooltipController => _tooltipController;
        [SerializeField] MicroTooltipWindow _tooltipWindowPrefab;
        [SerializeField] RectTransform _tooltipWindowsContainer;

        // Cursor
        static MicroCursorController _cursorController;
        public static MicroCursorController CursorController => _cursorController;
        [SerializeField] CursorSpriteSet _cursorSpriteSet;
        [SerializeField] float _cursorFrameRate = 0.1f;

        private void Start() {
            _modalController = new MicroModalController(_modalWindowPrefab, _modalWindowsContainer);
            _notificationController = new MicroNotificationController(_notificationWindowPrefab, _notificationWindowsContainer, _notificationDefaultDuration);
            _popupController = new MicroPopupController(_popupWindowPrefab, _popupWindowsContainer);
            _tooltipController = new MicroTooltipController(_tooltipWindowPrefab, _tooltipWindowsContainer);
            _cursorController = new MicroCursorController(_cursorSpriteSet, _cursorFrameRate);
        }

        void Update() {
            if(_modalController != null) _modalController.Update();
            if(_notificationController != null) _notificationController.Update();
            if(_popupController != null) _popupController.Update();
            if(_tooltipController != null) _tooltipController.Update();
            if(_cursorController != null) _cursorController.Update();
        }
    }

    #region Custom Editor
#if UNITY_EDITOR
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    [CustomEditor(typeof(MicroUI))]
    public class MicroUI_Editor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // Modal
            SerializedProperty modalWindowPrefab = serializedObject.FindProperty("_modalWindowPrefab");
            SerializedProperty modalWindowsContainer = serializedObject.FindProperty("_modalWindowsContainer");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Modal", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(modalWindowPrefab, new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(modalWindowsContainer, new GUIContent("Container", "Container where windows should be instantiated."));

            // Notification
            SerializedProperty notificationWindowPrefab = serializedObject.FindProperty("_notificationWindowPrefab");
            SerializedProperty notificationWindowsContainer = serializedObject.FindProperty("_notificationWindowsContainer");
            SerializedProperty notificationDefaultDuration = serializedObject.FindProperty("_notificationDefaultDuration");
            SerializedProperty notificationDefaultFontSize = serializedObject.FindProperty("_notificationDefaultFontSize");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Notification", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(notificationWindowPrefab, new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(notificationWindowsContainer, new GUIContent("Container", "Container where windows should be instantiated."));
            EditorGUILayout.PropertyField(notificationDefaultDuration, new GUIContent("Default Duration"));

            // Popup
            SerializedProperty popupWindowPrefab = serializedObject.FindProperty("_popupWindowPrefab");
            SerializedProperty popupWindowsContainer = serializedObject.FindProperty("_popupWindowsContainer");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Popup", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(popupWindowPrefab, new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(popupWindowsContainer, new GUIContent("Container", "Container where windows should be instantiated."));

            // Tooltip
            SerializedProperty tooltipWindowPrefab = serializedObject.FindProperty("_tooltipWindowPrefab");
            SerializedProperty tooltipWindowsContainer = serializedObject.FindProperty("_tooltipWindowsContainer");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tooltip", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(tooltipWindowPrefab, new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(tooltipWindowsContainer, new GUIContent("Container", "Container where windows should be instantiated."));

            // Cursor
            SerializedProperty cursorSpriteSet = serializedObject.FindProperty("_cursorSpriteSet");
            SerializedProperty cursorFrameRate = serializedObject.FindProperty("_cursorFrameRate");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tooltip", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(cursorSpriteSet, new GUIContent("Sprite Set", "Cursor Sprite Set that will be used to display cursor in various states."));
            EditorGUILayout.PropertyField(cursorFrameRate, new GUIContent("Frame Rate", "Time (in seconds) needed for one frame (0.1 default)."));

            serializedObject.ApplyModifiedProperties();   // Apply changes
        }
    }
#endif
    #endregion
}