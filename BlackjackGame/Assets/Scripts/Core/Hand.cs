using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BlackjackGame.Data;

namespace BlackjackGame.Core
{
    /// <summary>
    /// Represents a player's hand in blackjack
    /// </summary>
    public class Hand
    {
        private List<Card> _cards;
        
        public IReadOnlyList<Card> Cards => _cards.AsReadOnly();
        
        public Hand()
        {
            _cards = new List<Card>();
        }
        
        /// <summary>
        /// Adds a card to the hand
        /// </summary>
        public void AddCard(Card card)
        {
            _cards.Add(card);
        }
        
        /// <summary>
        /// Clears all cards from the hand
        /// </summary>
        public void Clear()
        {
            _cards.Clear();
        }
        
        /// <summary>
        /// Gets the total value of the hand, accounting for Aces
        /// </summary>
        public int GetValue()
        {
            int value = 0;
            int aceCount = 0;
            
            foreach (var card in _cards)
            {
                value += card.GetBlackjackValue();
                if (card.IsAce())
                {
                    aceCount++;
                }
            }
            
            // Optimize Aces (count as 11 if it doesn't bust)
            while (aceCount > 0 && value <= 11)
            {
                value += 10; // Add 10 to make Ace count as 11 instead of 1
                aceCount--;
            }
            
            return value;
        }
        
        /// <summary>
        /// Returns true if the hand is busted (over 21)
        /// </summary>
        public bool IsBusted()
        {
            return GetValue() > 21;
        }
        
        /// <summary>
        /// Returns true if the hand is a blackjack (21 with 2 cards)
        /// </summary>
        public bool IsBlackjack()
        {
            return _cards.Count == 2 && GetValue() == 21;
        }
        
        /// <summary>
        /// Returns the number of cards in the hand
        /// </summary>
        public int CardCount => _cards.Count;
        
        public override string ToString()
        {
            return $"Hand: {string.Join(", ", _cards)} (Value: {GetValue()})";
        }
    }
}
