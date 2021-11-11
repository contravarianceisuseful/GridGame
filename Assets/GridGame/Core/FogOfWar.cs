using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GridGame
{
    public class FogOfWar
    {
        public enum FogState
        {
            Dark, Fog, Revealed
        }

        public interface ISeer
        {
            List<Tile> GetSeenTiles(Grid grid);
        }

        protected DoubleDictManyToMany<ISeer, Tile> SeerTile;
        protected DoubleDictOneToMany<FogState, Tile> FogStateTile;

        public void Initialize()
        {
            SeerTile = new DoubleDictManyToMany<ISeer, Tile>();
            FogStateTile = new DoubleDictOneToMany<FogState, Tile>();
        }

        public virtual void SetFogState(Tile tile, FogState state)
        {
            FogStateTile.SetPair(state, tile);
        }

        public virtual void GetSightChangedTiles(ISeer seer, Grid grid, out List<Tile> lostTiles, out List<Tile> gainedTiles)
        {
            var oldTiles = SeerTile.GetSecondaryList(seer);
            var newTiles = seer.GetSeenTiles(grid);
            lostTiles = oldTiles.Except(newTiles).ToList();
            gainedTiles = newTiles.Except(oldTiles).ToList();
        }
    }

}

