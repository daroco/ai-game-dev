using UnityEngine;

namespace BlackjackGame.Data
{
    public enum CardSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }
    
    public enum CardRank
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }
    
    [CreateAssetMenu(fileName = "CardData", menuName = "Blackjack/Card Data")]
    public class CardData : ScriptableObject
    {
        public CardSuit suit;
        public CardRank rank;
        public Sprite cardSprite;
        
        /// <summary>
        /// Gets the blackjack value of the card
        /// </summary>
        public int GetBlackjackValue()
        {
            if (rank >= CardRank.Jack)
            {
                return 10;
            }
            return (int)rank;
        }
        
        /// <summary>
        /// Returns a string representation of the card
        /// </summary>
        public override string ToString()
        {
            return $"{rank} of {suit}";
        }
    }
}
