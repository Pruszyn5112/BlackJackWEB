namespace BlackJackWEB.Models
{
    public class Game
    {
        public Deck Deck { get; private set; }
        public Player Player { get; private set; }
        public Dealer Dealer { get; private set; }
        public decimal CurrentBet { get; private set; }
        public bool IsInitialState { get; private set; }
        public bool IsDealerTurn { get; private set; }
        public Game(decimal initialBalance)
        {
            Deck = new Deck();
            Player = new Player("Player", initialBalance);
            Dealer = new Dealer();
            IsInitialState = true;
        }

        public void StartGame(decimal betAmount)
        {
            Player.ClearHands();
            Dealer.ClearHand();
            CurrentBet = betAmount;
           // Player.PlaceBet(betAmount);
            IsInitialState = false;
            IsDealerTurn = false;
            Player.AddCard(Deck.DrawCard());
            Player.AddCard(Deck.DrawCard());
            Dealer.AddCard(Deck.DrawCard());
            Dealer.AddCard(Deck.DrawCard());
        }
        public void PlayerHit()
        {
            if (!IsGameOver())
            {
                Player.AddCard(Deck.DrawCard());
                if (Player.HandValue > 21)
                {
                    if (!Player.MoveToNextHand())
                    {
                        DealerTurn();
                        UpdateBalance();
                    }
                }
            }
        }
        public void PlayerStand()
        {
            if (!Player.MoveToNextHand())
            {
                // If no more hands to play, complete dealer's turn
                DealerTurn();
                UpdateBalance();
                IsDealerTurn = true; 
            }
        }

        public void DealerTurn()
        {
            while (Dealer.HandValue < 17)
            {
                Dealer.AddCard(Deck.DrawCard());
            }
        }

        public bool IsGameOver()
        {
            // Game is over only when all hands are completed
            if (Player.CurrentHandIndex < Player.Hands.Count - 1)
            {
                return false; 
            }


            foreach (var hand in Player.Hands)
            {
                if (hand.HandValue <= 21)
                {
                    return false;  // Game isn't over if any player hand is still valid
                }
            }
            return true;  // Game is over if all conditions above are false
        }

        public bool IsBlackjack()
        {
            return Player.Hands.Exists(hand =>
                hand.HandValue == 21 && hand.GetCards().Count == 2);
        }

        public string GetWinner()
        {
            if (Player.Hands.TrueForAll(hand => hand.HandValue > 21))
                return "Dealer wins - Player busted!";
            if (Dealer.HandValue > 21)
                return "Player wins - Dealer busted!";
            if (Player.Hands.Exists(hand => hand.HandValue > Dealer.HandValue))
                return "Player wins!";
            if (Player.Hands.Exists(hand => hand.HandValue < Dealer.HandValue))
                return "Dealer wins!";
            return "Push - It's a tie!";
        }

        public void UpdateBalance()
        {
            foreach (var hand in Player.Hands)
            {
                decimal betAmount = CurrentBet;
                if (Player.HasDoubledDown)
                {
                    betAmount *= 2;
                }

                if (hand.HandValue > 21)
                {
                    // Player loses
                    continue;
                }
                else if (Dealer.HandValue > 21 || hand.HandValue > Dealer.HandValue)
                {
                    if (hand.HandValue == 21 && hand.GetCards().Count == 2)
                    {
                        Player.WinBet(betAmount * 2.5m); // Blackjack pays 3:2
                    }
                    else
                    {
                        Player.WinBet(betAmount * 2); // Regular win pays 1:1
                    }
                }
                else if (hand.HandValue == Dealer.HandValue)
                {
                    Player.WinBet(betAmount); // Push - return bet
                }
            }
        }
        public void SplitHand()
        {
            if (Player.CanSplit())
            {
                if (Player.SplitHand(CurrentBet))
                {
                    // Deal one card to each split hand
                    Player.CurrentHandIndex = 0;
                    Player.AddCard(Deck.DrawCard());

                    Player.CurrentHandIndex = 1;
                    Player.AddCard(Deck.DrawCard());

                    Player.CurrentHandIndex = 0; // Return to first hand
                }
            }
        }

        public void DoubleDown()
        {
            if (Player.CanDoubleDown())
            {
                Player.DoubleDown(CurrentBet);
                Player.AddCard(Deck.DrawCard());
                DealerTurn();
                UpdateBalance();
            }
        }

        public Card GetDealerFaceUpCard()
        {
            var cards = Dealer.Hand.GetCards();
            return cards.Count > 0 ? cards[0] : null!;
        }
    }
}
