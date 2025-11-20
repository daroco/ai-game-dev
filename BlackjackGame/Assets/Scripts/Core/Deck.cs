using System.Collections.Generic;
using UnityEngine;
using BlackjackGame.Data;

namespace BlackjackGame.Core
{
    /// <summary>
    /// Manages a deck of playing cards
    /// </summary>
    public class Deck
    {
        private List<Card> _cards;
        private System.Random _random;
        
        public int RemainingCards => _cards.Count;
        
        public Deck()
        {
            _random = new System.Random();
            InitializeDeck();
        }
        
        public Deck(int seed)
        {
            _random = new System.Random(seed);
            InitializeDeck();
        }
        
        /// <summary>
        /// Creates a standard 52-card deck
        /// </summary>
        private void InitializeDeck()
        {
            _cards = new List<Card>();
            
            foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardRank rank in System.Enum.GetValues(typeof(CardRank)))
                {
                    _cards.Add(new Card(suit, rank));
                }
            }
        }
        
        /// <summary>
        /// Shuffles the deck using Fisher-Yates algorithm
        /// </summary>
        public void Shuffle()
        {
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                Card temp = _cards[i];
                _cards[i] = _cards[j];
                _cards[j] = temp;
            }
        }
        
        /// <summary>
        /// Draws a card from the top of the deck
        /// </summary>
        public Card DrawCard()
        {
            if (_cards.Count == 0)
            {
                Debug.LogWarning("Deck is empty, reshuffling");
                InitializeDeck();
                Shuffle();
            }
            
            Card card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }
        
        /// <summary>
        /// Resets and shuffles the deck
        /// </summary>
        public void Reset()
        {
            InitializeDeck();
            Shuffle();
        }
    }
}
