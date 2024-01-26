using UnityEngine;
using DG.Tweening;

namespace Microlight.MicroUI {
    public enum MicroUIFeedbackAnimations {
        ScaleUpBounce,
    }

    // ****************************************************************************************************
    // Contains animatons for UI effects
    // ****************************************************************************************************
    public static class MicroUIFeedbacks {
        public static Tween PlayAnimation(Transform go, MicroUIFeedbackAnimations animation) {
            return animation switch {
                MicroUIFeedbackAnimations.ScaleUpBounce => ScaleUpBounce(go),
                _ => null
            };
        }
        const float DEFAULT_FEEDBACK_DURATION = 0.2f;
        static readonly Vector2 SCALE_UP_FACTOR = new Vector2(1.1f, 1.1f);
        public static Tween ScaleUpBounce(Transform go) => ScaleUpBounce(go, SCALE_UP_FACTOR);
        public static Tween ScaleUpBounce(Transform go, Vector2 scaleFactor, float duration = DEFAULT_FEEDBACK_DURATION) {
            return go.DOScale(go.localScale * scaleFactor, duration).SetEase(Ease.OutElastic);
        }
    }
}