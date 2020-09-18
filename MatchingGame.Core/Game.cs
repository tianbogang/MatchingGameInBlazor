using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchingGame.Core
{
    public class Game : IDisposable
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
            int[] pts = new int[count];
            for (int n = 0; n < count; n++)
            {
                pts[n] = n + 1;
            }

            var rng = new Random();
            rng.Shuffle(pts);
            for (int n = 0; n < count; n++)
            {
                CardSet1.Add(new Card(pts[n], CardState.Closed));
            }

            rng.Shuffle(pts);
            for (int n = 0; n < count; n++)
            {
                CardSet2.Add(new Card(pts[n], CardState.Closed));
            }
        }

        public bool NoMoreCard => ( CardSet1.All(c => c.State == CardState.Hidden) && CardSet2.All(c => c.State == CardState.Hidden) );

        public void Dispose()
        {
            CardSet1.Clear();
            CardSet2.Clear();
        }
    }
}
