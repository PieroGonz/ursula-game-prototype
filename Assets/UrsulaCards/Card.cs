// Card.cs
using UnityEngine;
using CardSystem.Animation;

namespace CardSystem
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Animator cardAnimator;
        [SerializeField] private ParticleSystem drawEffect;

        // Optional animation parameter names - customize for your animator
        private const string AnimDrawn = "Drawn";
        private const string AnimDrawing = "Drawing";

        // Play animation when card is being drawn
        public void OnDraw()
        {
            if (cardAnimator != null)
            {
                cardAnimator.SetBool(AnimDrawing, true);
            }

            if (drawEffect != null)
            {
                drawEffect.Play();
            }
        }

        // Play animation when card finishes drawing
        public void OnDrawComplete()
        {
            if (cardAnimator != null)
            {
                cardAnimator.SetBool(AnimDrawing, false);
                cardAnimator.SetTrigger(AnimDrawn);
            }
        }
    }
}
