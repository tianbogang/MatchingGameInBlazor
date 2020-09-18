using MatchingGame.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Service
{
    public class GameService : IGameService
    {
        public Game CurrentGame { get; private set; } = null;

        public Game CreateNewGame(int difficulty)
        {
            Game game = new Game(difficulty);

            CurrentGame?.Dispose();
            CurrentGame = game;
            return CurrentGame;
        }

    }
}
