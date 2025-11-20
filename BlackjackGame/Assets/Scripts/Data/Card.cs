using UnityEngine;

namespace BlackjackGame.Data
{
    /// <summary>
    /// Represents a single playing card in the game
    /// </summary>
    [System.Serializable]
    public class Card
    {
        public CardSuit Suit { get; private set; }
        public CardRank Rank { get; private set; }
        
        public Card(CardSuit suit, CardRank rank)
        {
            Suit = suit;
            Rank = rank;
        }
        
        /// <summary>
        /// Gets the blackjack value of the card (Aces can be 1 or 11)
        /// </summary>
        public int GetBlackjackValue()
        {
            if (Rank >= CardRank.Jack)
            {
                return 10;
            }
            return (int)Rank;
        }
        
        /// <summary>
        /// Returns true if this card is an Ace
        /// </summary>
        public bool IsAce()
        {
            return Rank == CardRank.Ace;
        }
        
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
        
        /// <summary>
        /// Creates a unique identifier for network serialization
        /// </summary>
        public int GetCardId()
        {
            return ((int)Suit * 13) + (int)Rank;
        }
        
        /// <summary>
        /// Creates a card from a unique identifier
        /// </summary>
        public static Card FromCardId(int cardId)
        {
            CardSuit suit = (CardSuit)(cardId / 13);
            CardRank rank = (CardRank)(cardId % 13);
            return new Card(suit, rank);
        }
    }
}
