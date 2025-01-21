using System;
using System.Collections.Generic;

namespace BlackJackWEB.Models
{
    public class Player
    {
        public string Name { get; set; }
        public List<Hand> Hands { get; private set; }
        public decimal Balance { get; private set; }
        public int CurrentHandIndex { get; set; }
        public bool HasDoubledDown { get; private set; }
        public decimal InsuranceBet { get; private set; }

        public Player(string name, decimal initialBalance)
        {
            Name = name;
            Hands = new List<Hand> { new Hand() };
            Balance = initialBalance;
            CurrentHandIndex = 0;
            HasDoubledDown = false;
            InsuranceBet = 0;
        }

        public void AddCard(Card card)
        {
            Hands[CurrentHandIndex].AddCard(card);
        }

        public int HandValue => Hands[CurrentHandIndex].HandValue;

        public void ClearHands()
        {
            Hands.Clear();
            Hands.Add(new Hand());
            CurrentHandIndex = 0;
            HasDoubledDown = false;
        }

        public bool PlaceBet(decimal amount)
        {
            if (amount > Balance)
            {
                return false;
            }
            Balance -= amount;
            return true;
        }

        public void WinBet(decimal amount)
        {
            Balance += amount;
        }
        public bool CanSplit()
        {
            if (Hands.Count > 1) return false;
            var cards = Hands[0].GetCards();
            return cards.Count == 2 && cards[0].Value == cards[1].Value;
        }
        public bool SplitHand(decimal currentBet)
        {
            if (!CanSplit()) return false;
            if (currentBet > Balance) return false;

            var cards = Hands[0].GetCards();

            // Create two new hands
            var firstHand = new Hand();
            firstHand.AddCard(cards[0]);

            var secondHand = new Hand();
            secondHand.AddCard(cards[1]);

            // Clear and update hands list
            Hands.Clear();
            Hands.Add(firstHand);
            Hands.Add(secondHand);

            CurrentHandIndex = 0;
            PlaceBet(currentBet);
            return true;
        }

        public bool MoveToNextHand()
        {
            if (CurrentHandIndex < Hands.Count - 1)
            {
                CurrentHandIndex++;
                return true;
            }
            return false;
        }
        public bool IsCurrentHandFinished()
        {
            return Hands[CurrentHandIndex].HandValue >= 21;
        }

        public bool CanDoubleDown()
        {
            return Hands[CurrentHandIndex].GetCards().Count == 2 && !HasDoubledDown;
        }

        public void DoubleDown(decimal currentBet)
        {
            if (!CanDoubleDown()) throw new InvalidOperationException("Cannot double down.");
            PlaceBet(currentBet); // Double the bet
            HasDoubledDown = true;
        }
        public bool CanPlaceInsuranceBet()
        {
            return Hands[CurrentHandIndex].GetCards().Count == 2 && InsuranceBet == 0;
        }

        public void PlaceInsuranceBet(decimal currentBet)
        {
            if (!CanPlaceInsuranceBet()) throw new InvalidOperationException("Cannot place insurance bet.");
            decimal insuranceAmount = currentBet / 2;
            if (insuranceAmount > Balance)
            {
                throw new InvalidOperationException("Insufficient balance to place insurance bet.");
            }
            Balance -= insuranceAmount;
            InsuranceBet = insuranceAmount;
        }

        public void WinInsuranceBet()
        {
            Balance += InsuranceBet * 3; // 2:1 payoff + original bet
            InsuranceBet = 0;
        }

        public void LoseInsuranceBet()
        {
            InsuranceBet = 0;
        }
    }
}