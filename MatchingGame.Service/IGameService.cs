using MatchingGame.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Service
{
    public interface IGameService
    {
        Game CurrentGame { get; }
        Game CreateNewGame(int difficulty);
    }
}
