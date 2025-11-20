using NUnit.Framework;
using BlackjackGame.Data;

namespace BlackjackGame.Tests
{
    public class CardTests
    {
        [Test]
        public void Card_GetBlackjackValue_NumberCards_ReturnsCorrectValue()
        {
            var card = new Card(CardSuit.Hearts, CardRank.Five);
            Assert.AreEqual(5, card.GetBlackjackValue());
        }
        
        [Test]
        public void Card_GetBlackjackValue_FaceCards_ReturnsTen()
        {
            var jack = new Card(CardSuit.Hearts, CardRank.Jack);
            var queen = new Card(CardSuit.Diamonds, CardRank.Queen);
            var king = new Card(CardSuit.Clubs, CardRank.King);
            
            Assert.AreEqual(10, jack.GetBlackjackValue());
            Assert.AreEqual(10, queen.GetBlackjackValue());
            Assert.AreEqual(10, king.GetBlackjackValue());
        }
        
        [Test]
        public void Card_GetBlackjackValue_Ace_ReturnsOne()
        {
            var ace = new Card(CardSuit.Spades, CardRank.Ace);
            Assert.AreEqual(1, ace.GetBlackjackValue());
        }
        
        [Test]
        public void Card_IsAce_WithAce_ReturnsTrue()
        {
            var ace = new Card(CardSuit.Hearts, CardRank.Ace);
            Assert.IsTrue(ace.IsAce());
        }
        
        [Test]
        public void Card_IsAce_WithNonAce_ReturnsFalse()
        {
            var king = new Card(CardSuit.Hearts, CardRank.King);
            Assert.IsFalse(king.IsAce());
        }
        
        [Test]
        public void Card_GetCardId_ReturnsUniqueId()
        {
            var card1 = new Card(CardSuit.Hearts, CardRank.Ace);
            var card2 = new Card(CardSuit.Hearts, CardRank.Two);
            var card3 = new Card(CardSuit.Diamonds, CardRank.Ace);
            
            Assert.AreNotEqual(card1.GetCardId(), card2.GetCardId());
            Assert.AreNotEqual(card1.GetCardId(), card3.GetCardId());
        }
        
        [Test]
        public void Card_FromCardId_ReconstructsCard()
        {
            var original = new Card(CardSuit.Clubs, CardRank.Seven);
            int cardId = original.GetCardId();
            var reconstructed = Card.FromCardId(cardId);
            
            Assert.AreEqual(original.Suit, reconstructed.Suit);
            Assert.AreEqual(original.Rank, reconstructed.Rank);
        }
    }
}
