using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Interface for signaling MicroUI element is clickable.
    // ****************************************************************************************************
    public interface IMicroClickable {
        public void Click();
        public void ForceClick();
        public GameObject GetGameObject();
    }
}