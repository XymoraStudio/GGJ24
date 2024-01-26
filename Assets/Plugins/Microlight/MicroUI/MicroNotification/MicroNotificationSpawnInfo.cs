namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Spawn info for MicroUI notification window
    // ****************************************************************************************************
    internal struct MicroNotificationSpawnInfo {
        internal MicroNotificationWindow Window;
        internal string Text;
        internal float Duration;

        internal MicroNotificationSpawnInfo(MicroNotificationWindow window, string text, float duration) {
            Window = window;
            Text = text;
            Duration = duration;
        }
    }
}