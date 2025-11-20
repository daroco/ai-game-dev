using NUnit.Framework;
using BlackjackGame.Core;
using BlackjackGame.Data;

namespace BlackjackGame.Tests
{
    public class HandTests
    {
        [Test]
        public void Hand_GetValue_WithNoCards_ReturnsZero()
        {
            var hand = new Hand();
            Assert.AreEqual(0, hand.GetValue());
        }
        
        [Test]
        public void Hand_GetValue_WithNumberCards_ReturnsSum()
        {
            var hand = new Hand();
            hand.AddCard(new Card(CardSuit.Hearts, CardRank.Five));
            hand.AddCard(new Card(CardSuit.Diamonds, CardRank.Seven));
            
            Assert.AreEqual(12, hand.GetValue());
        }
        
        [Test]
        public void Hand_GetValue_WithAce_CountsAsEleven_WhenNotBusting()
        {
            var hand = new Hand();
            hand.AddCard(new Card(CardSuit.Hearts, CardRank.Ace));
            hand.AddCard(new Card(CardSuit.Diamonds, CardRank.Nine));
            
            Assert.AreEqual(20, hand.GetValue()); // Ace as 11
        }
        
        [Test]
        public void Hand_GetValue_WithAce_CountsAsOne_WhenWouldBust()
        {
            var hand = new Hand();
            hand.AddCard(new Card(CardSuit.Hearts, CardRank.Ace));
            hand.AddCard(new Card(CardSuit.Diamonds, CardRank.King));
            hand.AddCard(new Card(CardSuit.Clubs, CardRank.Five));
            
            Assert.AreEqual(16, hand.GetValue()); // Ace as 1
        }
        
        [Test]
        public void Hand_IsBlackjack_WithAceAndTen_ReturnsTrue()
        {
            var hand = new Hand();
            hand.AddCard(new Card(CardSuit.Hearts, CardRank.Ace));
            hand.AddCard(new Card(CardSuit.Diamonds, CardRank.King));
            
            Assert.IsTrue(hand.IsBlackjack());
        }
        
        [Test]
        public void Hand_IsBlackjack_WithThreeCards_ReturnsFalse()
        {
            var hand = new Hand();
            hand.AddCard(new Card(CardSuit.Hearts, CardRank.Seven));
            hand.AddCard(new Card(CardSuit.Diamonds, CardRank.Seven));
            hand.AddCard(new Card(CardSuit.Clubs, CardRank.Seven));
            
            Assert.IsFalse(hand.IsBlackjack());
        }
        
        [Test]
        public void Hand_IsBusted_WithValueOver21_ReturnsTrue()
        {
            var hand = new Hand();
            hand.AddCard(new Card(CardSuit.Hearts, CardRank.King));
            hand.AddCard(new Card(CardSuit.Diamonds, CardRank.Queen));
            hand.AddCard(new Card(CardSuit.Clubs, CardRank.Five));
            
            Assert.IsTrue(hand.IsBusted());
        }
        
        [Test]
        public void Hand_IsBusted_WithValue21_ReturnsFalse()
        {
            var hand = new Hand();
            hand.AddCard(new Card(CardSuit.Hearts, CardRank.Ace));
            hand.AddCard(new Card(CardSuit.Diamonds, CardRank.King));
            
            Assert.IsFalse(hand.IsBusted());
        }
        
        [Test]
        public void Hand_Clear_RemovesAllCards()
        {
            var hand = new Hand();
            hand.AddCard(new Card(CardSuit.Hearts, CardRank.King));
            hand.AddCard(new Card(CardSuit.Diamonds, CardRank.Queen));
            
            hand.Clear();
            
            Assert.AreEqual(0, hand.CardCount);
            Assert.AreEqual(0, hand.GetValue());
        }
    }
}
