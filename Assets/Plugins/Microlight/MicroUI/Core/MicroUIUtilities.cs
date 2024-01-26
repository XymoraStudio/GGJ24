using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Microlight.MicroUI {
    public static class MicroUIUtilities {
        public static void PositionRectInsideContainer(RectTransform rt, RectTransform container) {
            if(rt == null) return;
            if(container == null) return;

            float xPos = rt.position.x;
            float yPos = rt.position.y;
            if(rt.rect.xMax + rt.position.x > container.rect.width) xPos -= rt.rect.xMax + rt.position.x - container.rect.width;
            if(rt.rect.xMin + rt.position.x < 0) xPos -= rt.rect.xMin + rt.position.x;

            if(rt.rect.yMax + rt.position.y > container.rect.height) yPos -= rt.rect.yMax + rt.position.y - container.rect.height;
            if(rt.rect.yMin + rt.position.y < 0) yPos -= rt.rect.yMin + rt.position.y;

            rt.position = new Vector2(xPos, yPos);
        }
        /// <summary>
        /// Checks if passed gameObject is same as object mouse is over
        /// Can be blocked by other raycast targets
        /// </summary>
        /// <param name="go">Object we are looking for</param>
        /// <returns>Is that object below mouse</returns>
        public static bool IsMouseOverElement(GameObject go) {
            if(EventSystem.current == null) {
                Debug.LogWarning("MicroUI: Event system not found. Please create event system in your scene.");
                return false;
            }
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            foreach(RaycastResult x in raycastResults) {
                if(x.gameObject == go) return true;   // Found the match
            }

            return false;   // No match
        }
    }
}