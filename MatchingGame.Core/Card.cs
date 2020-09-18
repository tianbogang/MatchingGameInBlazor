namespace MatchingGame.Core
{
    public class Card
    {
        public Card(int point, CardState state)
        {
            Point = point;
            State = state;
        }

        public int Point { get; }

        public CardState State { get; set; } = CardState.Closed;
    }
}
