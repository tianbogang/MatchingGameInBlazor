using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchingGame.Core
{
    public sealed class Game : IDisposable
    {
        public Guid Id { get; }
        public int Difficulty { get; } = (int)GameDifficulty.Medium;
        public List<Card> CardSet1 { get; }
        public List<Card> CardSet2 { get; }

        public TimeSpan TimeUsed { get; set; }

        public Game(int difficulty)
        {
            Id = Guid.NewGuid();
            Difficulty = difficulty;
            CardSet1 = new List<Card>();
            CardSet2 = new List<Card>();
            Init();
        }

        private void Init()
        {
            int count = Difficulty;
            var rng = new Random();

            int[] pts = rng.GenerateRandomNumbers(count);
            for (int n = 0; n < count; n++)
            {
                CardSet1.Add(new Card(pts[n], CardState.Closed));
            }

            pts = rng.GenerateRandomNumbers(count);
            for (int n = 0; n < count; n++)
            {
                CardSet2.Add(new Card(pts[n], CardState.Closed));
            }
        }

        public int RemainCards => ( CardSet1.Count(c => c.State != CardState.Hidden) + CardSet2.Count(c => c.State != CardState.Hidden) );

        public bool MatchedCards => (CardSet1.Any(c => c.State == CardState.OpenGreen) && CardSet2.Any(c => c.State == CardState.OpenGreen));

        public bool AnyRedCard => (CardSet1.Any(c => c.State == CardState.OpenRed) || CardSet2.Any(c => c.State == CardState.OpenRed));

        public event Action CardStateChanged = delegate { };

        public async Task UpdateCardFromUno(int point)
        {
            Card card = CardSet1.FirstOrDefault(c => c.Point == point);
            if (card != null)
            {
                ToggleStateInSameCardset(CardSet1, card);
                await UpdateStateBetweenCardset(CardSet2, card);
            }
        }

        public async Task UpdateCardFromDue(int point)
        {
            Card card = CardSet2.FirstOrDefault(c => c.Point == point);
            if (card != null)
            {
                ToggleStateInSameCardset(CardSet2, card);
                await UpdateStateBetweenCardset(CardSet1, card);
            }
        }

        private void ToggleStateInSameCardset(List<Card> cardSet, Card card)
        {
            Card selectedCard = cardSet.FirstOrDefault(c => c.State == CardState.OpenGreen);
            if (selectedCard != null)
            {
                selectedCard.State = CardState.Closed;
                card.State = CardState.OpenGreen;
                CardStateChanged();
            }
        }

        private async Task UpdateStateBetweenCardset(List<Card> cardSet, Card card)
        {
            Card selectedCard = cardSet.FirstOrDefault(c => c.State == CardState.OpenGreen);
            if (selectedCard != null)
            {
                if (card.Point == selectedCard.Point)
                {
                    card.State = CardState.OpenGreen;
                    CardStateChanged();

                    await Task.Delay(1000);

                    card.State = CardState.Hidden;
                    selectedCard.State = CardState.Hidden;
                }
                else
                {
                    card.State = CardState.OpenRed;
                    CardStateChanged();

                    await Task.Delay(3000);

                    card.State = CardState.Closed;
                }
            }
            else
            {
                card.State = CardState.OpenGreen;
            }

            CardStateChanged();
        }


        public void Dispose()
        {
            CardSet1.Clear();
            CardSet2.Clear();
        }
    }
}
