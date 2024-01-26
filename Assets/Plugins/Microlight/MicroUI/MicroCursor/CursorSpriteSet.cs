using System;
using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Struct that enables to have offset specific for each texture
    // ****************************************************************************************************
    [Serializable]
    public struct CursorStateData {
        public Texture2D[] Textures;
        public Vector2 Offset;
    }

    // ****************************************************************************************************
    // Stores all sprite icons for the cursor
    // ****************************************************************************************************
    [CreateAssetMenu(fileName = "CursorSpriteSet", menuName = "Microlight/Micro UI/Cursor Sprite Set")]
    public class CursorSpriteSet : ScriptableObject {
        [SerializeField] CursorStateData _normal;
        [SerializeField] CursorStateData _hovering;
        [SerializeField] CursorStateData _interactable;
        [SerializeField] CursorStateData _nonInteractable;
        [SerializeField] CursorStateData _dragging;
        [SerializeField] CursorStateData _clicked;
        [SerializeField] CursorStateData _waiting;
        [SerializeField] CursorStateData _friendly;
        [SerializeField] CursorStateData _enemy;

        /// <summary>
        /// Returns texture list for cursor in specified state
        /// </summary>
        /// <param name="mode">State in which cursor is</param>
        /// <returns>Texture 2D of cursor</returns>
        public CursorStateData GetCursorStateData(MicroCursorStates mode) {
            return mode switch {
                MicroCursorStates.Normal => _normal,
                MicroCursorStates.Hovering => _hovering,
                MicroCursorStates.Interactable => _interactable,
                MicroCursorStates.NonInteractable => _nonInteractable,
                MicroCursorStates.Dragging => _dragging,
                MicroCursorStates.Clicked => _clicked,
                MicroCursorStates.Waiting => _waiting,
                MicroCursorStates.Friendly => _friendly,
                MicroCursorStates.Enemy => _enemy,
                _ => _normal,
            };
        }
    }
}