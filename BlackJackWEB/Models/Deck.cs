using System;
using System.Collections.Generic;

namespace BlackJackWEB.Models
{
    public class Deck
    {
        private List<Card> _cards;
        private const int NumberOfDecks = 1;

        public Deck()
        {
            _cards = new List<Card>();
            InitializeDeck();
            ShuffleDeck();
        }

        private void InitializeDeck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            int[] values = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };

            for (int d = 0; d < NumberOfDecks; d++)
            {
                foreach (var suit in suits)
                {
                    for (int i = 0; i < ranks.Length; i++)
                    {
                        _cards.Add(new Card(suit, ranks[i], values[i]));
                    }
                }
            }
        }

        public void ShuffleDeck()
        {
            Random rand = new Random();
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                Card temp = _cards[i];
                _cards[i] = _cards[j];
                _cards[j] = temp;
            }
        }

        public Card DrawCard()
        {
            if (_cards.Count == 0)
            {
                InitializeDeck();
                ShuffleDeck();
            }

            var card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }

        public int CardsRemaining()
        {
            return _cards.Count;
        }
    }
}
