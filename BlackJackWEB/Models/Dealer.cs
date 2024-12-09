using BlackJackWEB.Models;

namespace BlackJackWEB.Models
{
    public class Dealer
    {
        public Hand Hand { get; private set; }

        public Dealer()
        {
            Hand = new Hand();
        }

        public void AddCard(Card card)
        {
            Hand.AddCard(card);
        }

        public int HandValue => Hand.HandValue;

        public void ClearHand()
        {
            Hand.ClearHand();
        }
    }
}
