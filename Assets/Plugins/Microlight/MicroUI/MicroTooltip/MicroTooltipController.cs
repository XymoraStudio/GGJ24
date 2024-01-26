using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Controller for tooltip system
    // ****************************************************************************************************
    public class MicroTooltipController : MicroUIController {
        MicroTooltipWindow _defaultPrefab;
        RectTransform _container;
        MicroTooltipWindow _defaultTooltipWindow;
        public MicroTooltipWindow DefaultTooltipWindow => _defaultTooltipWindow;

        internal MicroTooltipController(MicroTooltipWindow defaultPrefab, RectTransform container) {
            _container = container;
            _defaultPrefab = defaultPrefab;

            CheckReferences();
            SpawnDefaultTooltipWindow();
        }
        protected void CheckReferences() {
            if(_container == null) {
                //Debug.LogWarning("MicroTooltip: Container is not referenced.");
            }
        }

        MicroTooltipWindow SpawnDefaultTooltipWindow() {
            if(_defaultPrefab == null) return null;
            if(_container == null) return null;

            _defaultTooltipWindow = Object.Instantiate(_defaultPrefab, _container);
            _defaultTooltipWindow.Display = false;
            return _defaultTooltipWindow;
        }
        public void DisplayChangeWindow(bool value, MicroTooltipWindow tooltipWindow = null) {
            if(tooltipWindow != null) tooltipWindow.Display = value;
            else {
                if(_defaultTooltipWindow == null) return;
                _defaultTooltipWindow.Display = value;
            }
        }
        public void UpdateWindowText(string text, MicroTooltipWindow tooltipWindow = null) {
            if(tooltipWindow != null) tooltipWindow.Text = text;
            else {
                if(_defaultTooltipWindow == null) return;
                _defaultTooltipWindow.Text = text;
            }
        }

        #region MicroUI methods
        override internal void Update() { }
        #endregion
    }
}