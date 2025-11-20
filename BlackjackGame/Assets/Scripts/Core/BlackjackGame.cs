using System;
using UnityEngine;
using BlackjackGame.Data;

namespace BlackjackGame.Core
{
    /// <summary>
    /// Core blackjack game logic for 1v1 gameplay
    /// </summary>
    public class BlackjackGame
    {
        public enum GameState
        {
            WaitingForPlayers,
            Dealing,
            Player1Turn,
            Player2Turn,
            ShowingResults,
            GameOver
        }
        
        public enum GameResult
        {
            None,
            Player1Wins,
            Player2Wins,
            Push
        }
        
        private Deck _deck;
        private Hand _player1Hand;
        private Hand _player2Hand;
        private GameState _currentState;
        
        public Hand Player1Hand => _player1Hand;
        public Hand Player2Hand => _player2Hand;
        public GameState CurrentState => _currentState;
        
        public event Action<GameState> OnStateChanged;
        public event Action<int, Card> OnCardDealt; // playerIndex, card
        public event Action<GameResult, int, int> OnGameEnded; // result, player1Score, player2Score
        
        public BlackjackGame()
        {
            _deck = new Deck();
            _player1Hand = new Hand();
            _player2Hand = new Hand();
            _currentState = GameState.WaitingForPlayers;
        }
        
        /// <summary>
        /// Starts a new game round
        /// </summary>
        public void StartNewRound()
        {
            _player1Hand.Clear();
            _player2Hand.Clear();
            _deck.Reset();
            
            ChangeState(GameState.Dealing);
            
            // Deal initial cards (2 to each player)
            DealCardToPlayer(1);
            DealCardToPlayer(2);
            DealCardToPlayer(1);
            DealCardToPlayer(2);
            
            // Start with player 1's turn
            ChangeState(GameState.Player1Turn);
        }
        
        /// <summary>
        /// Player hits (draws a card)
        /// </summary>
        public void Hit(int playerIndex)
        {
            if (playerIndex == 1 && _currentState != GameState.Player1Turn)
                return;
            if (playerIndex == 2 && _currentState != GameState.Player2Turn)
                return;
            
            DealCardToPlayer(playerIndex);
            
            Hand hand = playerIndex == 1 ? _player1Hand : _player2Hand;
            if (hand.IsBusted())
            {
                // Player busted, move to next phase
                if (playerIndex == 1)
                {
                    ChangeState(GameState.Player2Turn);
                }
                else
                {
                    EndGame();
                }
            }
        }
        
        /// <summary>
        /// Player stands (ends their turn)
        /// </summary>
        public void Stand(int playerIndex)
        {
            if (playerIndex == 1 && _currentState == GameState.Player1Turn)
            {
                ChangeState(GameState.Player2Turn);
            }
            else if (playerIndex == 2 && _currentState == GameState.Player2Turn)
            {
                EndGame();
            }
        }
        
        /// <summary>
        /// Deals a card to the specified player
        /// </summary>
        private void DealCardToPlayer(int playerIndex)
        {
            Card card = _deck.DrawCard();
            
            if (playerIndex == 1)
            {
                _player1Hand.AddCard(card);
            }
            else
            {
                _player2Hand.AddCard(card);
            }
            
            OnCardDealt?.Invoke(playerIndex, card);
        }
        
        /// <summary>
        /// Ends the game and determines the winner
        /// </summary>
        private void EndGame()
        {
            ChangeState(GameState.ShowingResults);
            
            int player1Value = _player1Hand.GetValue();
            int player2Value = _player2Hand.GetValue();
            
            GameResult result = DetermineWinner(player1Value, player2Value);
            
            OnGameEnded?.Invoke(result, player1Value, player2Value);
            ChangeState(GameState.GameOver);
        }
        
        /// <summary>
        /// Determines the winner based on hand values
        /// </summary>
        private GameResult DetermineWinner(int player1Value, int player2Value)
        {
            bool player1Busted = player1Value > 21;
            bool player2Busted = player2Value > 21;
            
            // Check for busts
            if (player1Busted && player2Busted)
            {
                return GameResult.Push;
            }
            if (player1Busted)
            {
                return GameResult.Player2Wins;
            }
            if (player2Busted)
            {
                return GameResult.Player1Wins;
            }
            
            // Check for blackjack
            bool player1Blackjack = _player1Hand.IsBlackjack();
            bool player2Blackjack = _player2Hand.IsBlackjack();
            
            if (player1Blackjack && !player2Blackjack)
            {
                return GameResult.Player1Wins;
            }
            if (player2Blackjack && !player1Blackjack)
            {
                return GameResult.Player2Wins;
            }
            
            // Compare values
            if (player1Value > player2Value)
            {
                return GameResult.Player1Wins;
            }
            if (player2Value > player1Value)
            {
                return GameResult.Player2Wins;
            }
            
            return GameResult.Push;
        }
        
        /// <summary>
        /// Changes the game state and notifies listeners
        /// </summary>
        private void ChangeState(GameState newState)
        {
            _currentState = newState;
            OnStateChanged?.Invoke(newState);
        }
        
        /// <summary>
        /// Gets the current player's turn (1 or 2, or 0 if not in a turn state)
        /// </summary>
        public int GetCurrentPlayer()
        {
            return _currentState switch
            {
                GameState.Player1Turn => 1,
                GameState.Player2Turn => 2,
                _ => 0
            };
        }
    }
}
