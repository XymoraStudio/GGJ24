using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Radio group for radio button element of MicroUI library
    // Only one radio button in the group can be selected
    // ****************************************************************************************************
    public class MicroRadioGroup : MonoBehaviour {
        // Properties
        [SerializeField] List<MicroRadioButton> _radioButtons = new List<MicroRadioButton>();
        public List<MicroRadioButton> RadioButtons {
            get => _radioButtons;
        }
        [SerializeField] MicroRadioButton _defaultButton;   // Which button is selected by default. Must be part of _radioButtons list
        public MicroRadioButton DefaultButton {
            get => _defaultButton;
            set => _defaultButton = value;
        }
        MicroRadioButton _selectedButton;
        public MicroRadioButton SelectedButton {
            get => _selectedButton;
            set => _selectedButton = value;
        }

        // Initialization at start
        private void Start() {
            // Checks
            if(RadioButtons.Count < 1) Debug.LogWarning("MicroRadioButtonGroup: Group doesn't contain any radio buttons.");
            if(DefaultButton != null && !RadioButtons.Contains(DefaultButton)) {
                Debug.LogWarning("MicroRadioButtonGroup: Default button is not part of this radio group.");
                DefaultButton = null;
            }
            if(DefaultButton == null && RadioButtons.Count > 0) DefaultButton = RadioButtons[0];

            foreach(MicroRadioButton x in RadioButtons) {
                x.Selected = false;
                x.OnClick += SelectButton;
            }

            SynchronizeButtons();
        }
        /// <summary>
        /// Unselects every button of the group that should not be selected
        /// </summary>
        public void SynchronizeButtons() {
            if(SelectedButton != null && !RadioButtons.Contains(SelectedButton)) SelectedButton = null;
            if(DefaultButton != null && !RadioButtons.Contains(DefaultButton)) DefaultButton = null;
            if(DefaultButton == null && RadioButtons.Count > 0) DefaultButton = RadioButtons[0];
            if(SelectedButton == null) SelectedButton = DefaultButton;

            foreach(MicroRadioButton x in RadioButtons) {
                x.Selected = x == SelectedButton;
            }
        }
        public void SelectButton(MicroInteractable button) {
            if(button == null) return;
            MicroRadioButton radioButton = button as MicroRadioButton;
            if(radioButton == null) return;
            if(radioButton.Selected) return;
            if(!RadioButtons.Contains(radioButton)) return;

            if(SelectedButton != null) SelectedButton.Selected = false;
            radioButton.Selected = true;
            SelectedButton = radioButton;
        }
        public void AddRadioButton(MicroRadioButton radioButton) {
            if(radioButton == null) return;
            if(RadioButtons.Contains(radioButton)) return;

            RadioButtons.Add(radioButton);
            SynchronizeButtons();
        }
        public void RemoveRadioButton(MicroRadioButton radioButton) {
            if(radioButton == null) return;
            if(!RadioButtons.Contains(radioButton)) return;

            RadioButtons.Remove(radioButton);
            SynchronizeButtons();
        }
    }
}