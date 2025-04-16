// AnimationActions.cs
using System.Collections;
using UnityEngine;

namespace CardSystem.Animation
{
    // For moving a card from one position to another
    public class MovementAction : IAnimationAction
    {
        private Transform target;
        private Vector3 destination;
        private float duration;
        private AnimationCurve curve;

        public MovementAction(Transform target, Vector3 destination, float duration, AnimationCurve curve = null)
        {
            this.target = target;
            this.destination = destination;
            this.duration = duration;
            this.curve = curve ?? AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        public IEnumerator Execute()
        {
            Vector3 startPosition = target.position;
            float elapsed = 0;

            while (elapsed < duration)
            {
                float t = curve.Evaluate(elapsed / duration);
                target.position = Vector3.Lerp(startPosition, destination, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            target.position = destination;
        }
    }

    // For rotating a card
    public class RotateAction : IAnimationAction
    {
        private Transform target;
        private Quaternion targetRotation;
        private float duration;
        private AnimationCurve curve;

        public RotateAction(Transform target, Quaternion targetRotation, float duration, AnimationCurve curve = null)
        {
            this.target = target;
            this.targetRotation = targetRotation;
            this.duration = duration;
            this.curve = curve ?? AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        public IEnumerator Execute()
        {
            Quaternion startRotation = target.rotation;
            float elapsed = 0;

            while (elapsed < duration)
            {
                float t = curve.Evaluate(elapsed / duration);
                target.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            target.rotation = targetRotation;
        }
    }
}
