using NUnit.Framework;
using BlackjackGame.Core;

namespace BlackjackGame.Tests
{
    public class DeckTests
    {
        [Test]
        public void Deck_InitializedDeck_Has52Cards()
        {
            var deck = new Deck();
            Assert.AreEqual(52, deck.RemainingCards);
        }
        
        [Test]
        public void Deck_DrawCard_ReducesCardCount()
        {
            var deck = new Deck();
            deck.DrawCard();
            
            Assert.AreEqual(51, deck.RemainingCards);
        }
        
        [Test]
        public void Deck_DrawCard_ReturnsCard()
        {
            var deck = new Deck();
            var card = deck.DrawCard();
            
            Assert.IsNotNull(card);
        }
        
        [Test]
        public void Deck_Shuffle_MaintainsCardCount()
        {
            var deck = new Deck();
            deck.Shuffle();
            
            Assert.AreEqual(52, deck.RemainingCards);
        }
        
        [Test]
        public void Deck_Reset_RestoresAllCards()
        {
            var deck = new Deck();
            
            // Draw some cards
            for (int i = 0; i < 10; i++)
            {
                deck.DrawCard();
            }
            
            deck.Reset();
            
            Assert.AreEqual(52, deck.RemainingCards);
        }
        
        [Test]
        public void Deck_WithSameSeed_ProducesSameSequence()
        {
            var deck1 = new Deck(12345);
            var deck2 = new Deck(12345);
            
            deck1.Shuffle();
            deck2.Shuffle();
            
            var card1 = deck1.DrawCard();
            var card2 = deck2.DrawCard();
            
            Assert.AreEqual(card1.Suit, card2.Suit);
            Assert.AreEqual(card1.Rank, card2.Rank);
        }
    }
}
