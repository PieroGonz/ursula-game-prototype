// DeckManager.cs
using UnityEngine;
using CardSystem.Animation;
using System.Linq;

namespace CardSystem
{
    public class DeckManager : MonoBehaviour
    {
        // Singleton instance
        public static DeckManager Instance { get; private set; }

        [SerializeField] private Transform deckPosition;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int maxCardsInHand = 7;

        [Header("Animation Settings")]
        [SerializeField] private float drawSpeed = 0.5f;
        [SerializeField] private AnimationCurve drawCurve;

        // Reference to the hand layout manager
        private HandLayoutManager handLayout;

        private void Awake()
        {
            // Set up singleton instance
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Debug.LogWarning("Multiple DeckManagers found. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            // Get or add hand layout manager
            handLayout = GetComponent<HandLayoutManager>();
            if (handLayout == null)
            {
                handLayout = gameObject.AddComponent<HandLayoutManager>();
            }

            if (drawCurve == null)
                drawCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        // Draw a card from deck to hand
        public void DrawCard()
        {
            if (handLayout.GetCards().Count >= maxCardsInHand)
                return;

            Debug.Log("Drawing");

            // Create new card at deck position
            GameObject cardObj = Instantiate(cardPrefab, deckPosition.position, deckPosition.rotation);

            cardObj.transform.SetParent(transform);

            Card card = cardObj.GetComponent<Card>();

            if (card == null)
            {
                Debug.LogError("Card prefab must have Card component attached!");
                return;
            }

            // Notify card animation is starting
            card.OnDraw();

            // Add to hand and position it
            int cardIndex = handLayout.GetCards().Count;
            Vector3 targetPos = handLayout.GetCardPosition(cardIndex, cardIndex + 1);
            Quaternion targetRot = handLayout.GetCardRotation(cardIndex, cardIndex + 1);

            // Create animation sequence
            AnimationQuery query = card.gameObject.AddComponent<AnimationQuery>();

            // Add animation actions
            query.AddToQuery(new MovementAction(card.transform, targetPos, drawSpeed, drawCurve));
            query.AddToQuery(new RotateAction(card.transform, targetRot, drawSpeed, drawCurve));

            // Start animation
            query.Start(this, () => {
                card.OnDrawComplete();
                handLayout.AddCard(card); // This will trigger rearrangement
            });
        }

        // Draw multiple cards at once
        public void DrawCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                DrawCard();
            }
        }

        // Play a card from the hand
        public void PlayCard(Card card)
        {
            if (card == null || !handLayout.GetCards().Contains(card))
                return;

            // Remove from hand and rearrange remaining cards
            handLayout.RemoveCard(card);

            // Here you can add card playing animation
            // ...

            // Then destroy or move to discard pile
            Destroy(card.gameObject, 1f); // Delay destruction to allow for play animation
        }

        // Add this method to DeckManager.cs
        public void ReturnAllCardsToDeck(float returnSpeed = 0.3f)
        {
            // Get all cards in hand
            var cards = handLayout.GetCards().ToList(); // Create a copy of the list

            // Return each card to the deck
            foreach (var card in cards)
            {
                ReturnCardToDeck(card, returnSpeed);
            }
        }

        // Helper method to return a single card to the deck
        private void ReturnCardToDeck(Card card, float returnSpeed)
        {
            if (card == null)
                return;

            // Remove from hand layout first to prevent rearrangement
            handLayout.RemoveCard(card);

            // Create animation sequence
            AnimationQuery query = card.gameObject.AddComponent<AnimationQuery>();

            // Add animation actions to return to deck position
            query.AddToQuery(new MovementAction(card.transform, deckPosition.position, returnSpeed, drawCurve));
            query.AddToQuery(new RotateAction(card.transform, deckPosition.rotation, returnSpeed, drawCurve));

            // Start animation and destroy card when complete
            query.Start(this, () => {
                Destroy(card.gameObject);
            });
        }

    }
}
