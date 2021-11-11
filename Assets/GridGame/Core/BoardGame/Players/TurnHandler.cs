using System.Linq;
using UnityEngine;

namespace Multiplayer
{
    public class TurnHandler : MonoBehaviour
    {
        public PlayerHandler PlayerHandler;

        public Player CurrentTurnPlayer { get; internal set; }

        public virtual void SetFirstPlayer()
        {
            
        }

        public Player NextTurn()
        {
            var players = PlayerHandler.GetPlayers();
            if (CurrentTurnPlayer == null)
            {
                throw new GridGameException("no starting player determined");
            }
            if (CurrentTurnPlayer == players.Last())
            {
                CurrentTurnPlayer = players.First();
            }
            else
            {
                CurrentTurnPlayer = players[players.FindIndex(x => x == CurrentTurnPlayer) + 1];
            }
            return CurrentTurnPlayer;
        }

        //TODO add turn order list
    }
}