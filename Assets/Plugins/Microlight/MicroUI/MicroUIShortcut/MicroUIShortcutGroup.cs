namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Stores buttons that will respond on MicroUIShortcut buttons
    // ****************************************************************************************************
    public class MicroUIShortcutGroup {
        readonly IMicroClickable _confirm;
        public IMicroClickable Confirm => _confirm;
        readonly IMicroClickable _decline;
        public IMicroClickable Decline => _decline;
        readonly int _priority;   // Priority of this group. Higher priority means it gets looked at first
        public int Priority => _priority;

        public MicroUIShortcutGroup(IMicroClickable confirm, IMicroClickable decline, int priority) {
            _confirm = confirm;
            _decline = decline;
            _priority = priority;
        }
    }
}