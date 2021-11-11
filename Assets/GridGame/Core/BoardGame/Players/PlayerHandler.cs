using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Multiplayer
{
    public enum AllianceState
    {
        Enemy, Neutral, Ally
    }

    public class PlayerHandler : MonoBehaviour
    {
        public AllianceState DefaultAllianceState;

        protected List<Player> playerList;
        protected Dict2D<Player, AllianceState> playerAlliances;

        public virtual  void Initialize()
        {
            playerList = new List<Player>();
            playerAlliances = new Dict2D<Player, AllianceState>();
            DefaultAllianceState = AllianceState.Enemy;
        }

        public List<Player> GetPlayers()
        {
            return new List<Player>(playerList);
        }

        public virtual void AddPlayer(Player newPlayer)
        {
            foreach (var player in playerList)
            {
                playerAlliances.SetPair(newPlayer, player, DefaultAllianceState);
            }
            playerList.Add(newPlayer);
            
        }

        public virtual void RemovePlayer(Player toRemove)
        {
            playerAlliances.RemoveKey(toRemove);
        }

        public virtual AllianceState GetAllianceState(Player p1, Player p2)
        {
            return playerAlliances.GetPairValue(p1, p2);
        }

        public virtual void SetAllianceState(Player p1, Player p2, AllianceState state)
        {
            playerAlliances.SetPair(p1, p2, state);
        }
    }
}

