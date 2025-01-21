using System.Collections.Generic;

namespace BlackJackWEB.Models
{
    public class Hand
    {
        private List<Card> _cards;

        public Hand()
        {
            _cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public int HandValue
        {
            get
            {
                int value = 0;
                int aceCount = 0;

                foreach (var card in _cards)
                {
                    value += card.Value;
                    if (card.Rank == "A")
                    {
                        aceCount++;
                    }
                }

                // Adjust for Aces
                while (value > 21 && aceCount > 0)
                {
                    value -= 10;
                    aceCount--;
                }

                return value;
            }
        }

        public void ClearHand()
        {
            _cards.Clear();
        }

        public List<Card> GetCards()
        {
            return _cards;
        }
    }
}