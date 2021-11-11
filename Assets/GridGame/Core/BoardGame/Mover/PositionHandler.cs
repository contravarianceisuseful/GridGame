using UnityEngine;
using System.Collections;
using System.Linq;
using GridGame;

namespace BoardGame
{
    public class PositionHandler : ControllerComponent
    {
        protected DoubleDictOneToOne<Mover, Tile> MoverTile;

        public virtual void Initialize()
        {
            MoverTile = new DoubleDictOneToOne<Mover, Tile>();
        }

        public virtual void AddMover(Mover mover, Tile tile)
        {
            MoverTile.AddPair(mover, tile);
        }

        public virtual void RemoveMover(Mover mover)
        {
            MoverTile.RemovePrimary(mover);
        }

        public virtual bool CanMoveTo(Mover mover, Tile destination)
        {
            return mover.GetPossibleMoves(controller).Select(x => x.Destination).Contains(destination);
        }

        public virtual void Move(Mover mover, Tile destination)
        {
            MoverTile.SetPair(mover, destination);
            MoveGameObject(mover.transform, destination.transform);
        }

        public virtual void MoveGameObject(Transform mover, Transform tile)
        {
            mover.SetParent(tile);
            mover.localPosition = Vector3.zero;
        }

        #region Helpers

        public bool IsEmpty(Tile tile)
        {
            return MoverTile.ContainsSecondary(tile);
        }

        public Mover GetMoverOnTile(Tile tile)
        {
            return MoverTile.GetPrimary(tile);
        }

        public Tile GetTileOfMover(Mover mover)
        {
            return MoverTile.GetSecondary(mover);
        }
#endregion
    }

}
