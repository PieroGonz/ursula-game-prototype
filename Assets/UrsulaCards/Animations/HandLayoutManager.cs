// HandLayoutManager.cs
using System.Collections.Generic;
using UnityEngine;
using CardSystem.Animation;

namespace CardSystem
{
    public enum HandLayoutType
    {
        Horizontal,    // Cards arranged in a straight line
        Arc,           // Cards arranged in a curved arc
        Fan            // Cards arranged in a fan-like pattern
    }

    [System.Serializable]
    public class HandLayoutSettings
    {
        public HandLayoutType layoutType = HandLayoutType.Arc;

        [Header("Horizontal Layout")]
        public float cardSpacing = 0.8f;      // Space between cards

        [Header("Arc Layout")]
        public float radius = 8f;             // Radius of the arc
        public float arcAngle = 30f;          // Total angle of the arc in degrees
        public float arcHeight = 1f;          // Height of the arc

        [Header("Fan Layout")]
        public float fanRadius = 10f;         // Radius for fan arrangement
        public float fanSpread = 60f;         // Spread angle of the fan in degrees
        public float fanOffset = -3f;         // Vertical offset for the fan

        [Header("Animation")]
        public float arrangementSpeed = 0.3f;  // Speed of rearrangement animations
        public AnimationCurve arrangementCurve;
    }

    public class HandLayoutManager : MonoBehaviour
    {
        [SerializeField] private HandLayoutSettings layoutSettings;
        [SerializeField] private Transform handAnchor;  // Central point of the hand

        private List<Card> cardsInHand = new List<Card>();

        private void Start()
        {
            if (handAnchor == null)
                handAnchor = transform;

            if (layoutSettings.arrangementCurve == null)
                layoutSettings.arrangementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        // Add a card to the hand
        public void AddCard(Card card)
        {
            if (card == null)
                return;

            cardsInHand.Add(card);
            ArrangeCards();
        }

        // Remove a card from the hand
        public void RemoveCard(Card card)
        {
            if (card == null || !cardsInHand.Contains(card))
                return;

            cardsInHand.Remove(card);
            ArrangeCards();
        }

        // Get all cards in hand
        public List<Card> GetCards()
        {
            return new List<Card>(cardsInHand);
        }

        // Calculate position for a card at index
        public Vector3 GetCardPosition(int index, int totalCards)
        {
            if (index < 0 || (totalCards > 0 && index >= totalCards))
                return handAnchor.position;

            // If no cards, return center position
            if (totalCards <= 0)
                return handAnchor.position;

            switch (layoutSettings.layoutType)
            {
                case HandLayoutType.Horizontal:
                    return GetHorizontalPosition(index, totalCards);

                case HandLayoutType.Arc:
                    return GetArcPosition(index, totalCards);

                case HandLayoutType.Fan:
                    return GetFanPosition(index, totalCards);

                default:
                    return handAnchor.position;
            }
        }

        // Calculate rotation for a card at index
        public Quaternion GetCardRotation(int index, int totalCards)
        {
            if (index < 0 || totalCards <= 0 || index >= totalCards)
                return Quaternion.identity;

            switch (layoutSettings.layoutType)
            {
                case HandLayoutType.Horizontal:
                    return Quaternion.identity;

                case HandLayoutType.Arc:
                    // Slight rotation depending on position in arc
                    float angle = GetArcAngle(index, totalCards);
                    return Quaternion.Euler(0, 0, angle * 0.5f); // Adjust multiplier for rotation intensity

                case HandLayoutType.Fan:
                    // Cards rotated to point toward center
                    float fanAngle = GetFanAngle(index, totalCards);
                    return Quaternion.Euler(0, 0, fanAngle);

                default:
                    return Quaternion.identity;
            }
        }

        // Rearrange all cards in hand
        public void ArrangeCards()
        {
            int totalCards = cardsInHand.Count;

            for (int i = 0; i < totalCards; i++)
            {
                Card card = cardsInHand[i];
                Vector3 targetPos = GetCardPosition(i, totalCards);
                Quaternion targetRot = GetCardRotation(i, totalCards);

                // Create animation for repositioning
                AnimationQuery query = card.gameObject.AddComponent<AnimationQuery>();
                query.AddToQuery(new MovementAction(card.transform, targetPos, layoutSettings.arrangementSpeed, layoutSettings.arrangementCurve));
                query.AddToQuery(new RotateAction(card.transform, targetRot, layoutSettings.arrangementSpeed, layoutSettings.arrangementCurve));
                query.Start(this);
            }
        }

        // Helpers for different layout types
        private Vector3 GetHorizontalPosition(int index, int totalCards)
        {
            float width = layoutSettings.cardSpacing * (totalCards - 1);
            float startX = -width / 2f;

            return handAnchor.position + new Vector3(
                startX + (index * layoutSettings.cardSpacing),
                0,
                0
            );
        }

        private float GetArcAngle(int index, int totalCards)
        {
            // Calculate angle along the arc
            float startAngle = -layoutSettings.arcAngle / 2f;
            float step = layoutSettings.arcAngle / (totalCards <= 1 ? 1 : totalCards - 1);

            return startAngle + (index * step);
        }

        private Vector3 GetArcPosition(int index, int totalCards)
        {
            float angle = GetArcAngle(index, totalCards);
            float radians = angle * Mathf.Deg2Rad;

            // Calculate position on arc
            float x = Mathf.Sin(radians) * layoutSettings.radius;
            float y = -Mathf.Cos(radians) * layoutSettings.radius + layoutSettings.radius - layoutSettings.arcHeight;

            return handAnchor.position + new Vector3(x, y, 0);
        }

        private float GetFanAngle(int index, int totalCards)
        {
            float startAngle = 270f - (layoutSettings.fanSpread / 2f);
            float step = layoutSettings.fanSpread / (totalCards <= 1 ? 1 : totalCards - 1);

            return startAngle + (index * step);
        }

        private Vector3 GetFanPosition(int index, int totalCards)
        {
            float angle = GetFanAngle(index, totalCards);
            float radians = angle * Mathf.Deg2Rad;

            // Calculate position on fan
            float x = Mathf.Cos(radians) * layoutSettings.fanRadius;
            float y = Mathf.Sin(radians) * layoutSettings.fanRadius + layoutSettings.fanOffset;

            return handAnchor.position + new Vector3(x, y, 0);
        }
    }
}
