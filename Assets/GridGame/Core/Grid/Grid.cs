using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridGame
{
    #region directions
    public enum SquareDir
    {
        Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft
    }
    public enum HexFlatDir
    {
        Up, UpRight, DownRight, Down, DownLeft, UpLeft
    }
    public enum HexPointyDir
    {
        UpRight,Right, DownRight, DownLeft, Left, UpLeft
    }

    public enum Mirror
    {
        X, Y, Flip
    }
#endregion

    public class Grid : MonoBehaviour
    {


#region TileCoord

        protected Tile[,] grid;
        protected Dictionary<Tile, Coordinate2> TileCoord;

        public List<Tile> GetAllTiles()
        {
            var tiles = new List<Tile>();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                    {
                        tiles.Add(grid[i,j]);
                    }
                }   
            }
            return tiles;
        }

        public Tile GetTileAtCoord(Coordinate2 coord)
        {
            return GetTileAtCoord(coord.x, coord.y);
        }

        public Tile GetTileAtCoord(int x, int y)
        {
            return grid[x, y];
        }

        public Coordinate2 GetCoordOfTile(Tile tile)
        {
            return TileCoord.ContainsKey(tile)? TileCoord[tile] : null;
        }

        public virtual List<Tile> GetNeighbours(Tile tile)
        {
            var tileList = new List<Tile>();

            if (TileTemplate is Square)
            {
                bool diag = TileTemplate is SquareWithDiags;
                var dirs = diag ? DirHelper.GetAllDirEnum<SquareDir>() : DirHelper.GetAllSquareDirsNoDiag();
                foreach (var squareDir in dirs)
                {
                    Tile tempTile = GetAdjecentTileInDir(tile, squareDir);
                    if (tempTile != null)
                    {
                        tileList.Add(tempTile);
                    }
                }
            }
            if (TileTemplate is HexFlat)
            {
                foreach (var dir in DirHelper.GetAllDirEnum<HexFlatDir>())
                {
                    Tile tempTile = GetAdjecentTileInDir(tile, dir);
                    if (tempTile != null)
                    {
                        tileList.Add(tempTile);
                    }
                }
            }
            if (TileTemplate is HexPointy)
            {
                foreach (var dir in DirHelper.GetAllDirEnum<HexPointyDir>())
                {
                    Tile tempTile = GetAdjecentTileInDir(tile, dir);
                    if (tempTile != null)
                    {
                        tileList.Add(tempTile);
                    }
                }
            }

            //TODO both hexes

            return tileList;
        }

        public List<Tile> GetFrontier(List<Tile> tiles)
        {
            List<Tile> frontier = new List<Tile>();
            foreach (var tile in tiles)
            {
                foreach (var neighbour in GetNeighbours(tile))
                {
                    if (!frontier.Contains(neighbour))
                    {
                        frontier.Add(neighbour);
                    }
                }
            }
            return frontier;
        }

        public Coordinate2 GetCoordFromOriginToTarget(Tile target, Tile origin)
        {
            int xDiff = GetCoordOfTile(target).x - GetCoordOfTile(origin).x;
            int yDiff = GetCoordOfTile(target).y - GetCoordOfTile(origin).y;
            return new Coordinate2(xDiff, yDiff);
        }

        #endregion

#region features
    #region pathfinding
        public virtual List<Tile> FindPathTo(Tile tile, Tile target, Func<Tile, float> CostFunc)
        {
            return Pathfinder.PathfinderHelper.GetPathTo(tile, target, GetNeighbours, CostFunc);
        }
        #endregion



        #endregion

#region dir helpers
        #region Square direction helpers

        public Tile GetAdjecentTileInDir(int x, int y, SquareDir dir)
        {
            try
            {
                switch (dir)
                {
                    case SquareDir.Up:
                        return grid[x, y + 1];
                    case SquareDir.Down:
                        return grid[x, y - 1];
                    case SquareDir.Left:
                        return grid[x - 1, y];
                    case SquareDir.Right:
                        return grid[x + 1, y];
                    case SquareDir.UpLeft:
                        return grid[x - 1, y + 1];
                    case SquareDir.UpRight:
                        return grid[x + 1, y + 1];
                    case SquareDir.DownLeft:
                        return grid[x - 1, y - 1];
                    case SquareDir.DownRight:
                        return grid[x + 1, y - 1];
                    default:
                        throw new Exception("Unhandled direction");
                }
            }
            catch(IndexOutOfRangeException e)
            {
                return null;
            }
        }

        public Tile GetAdjecentTileInDir(Tile tile, SquareDir dir)
        {
            return GetAdjecentTileInDir(GetCoordOfTile(tile).x, GetCoordOfTile(tile).y, dir);
        }

        public Tile GetTileInDirCombo(Tile tile, params SquareDir[] dirs)
        {
            Tile target = tile;
            foreach (var dir in dirs)
            {
                target = GetAdjecentTileInDir(target, dir);
                if (target == null)
                {
                    return null;
                }
            }
            return target;
        }

        #endregion

    #region HexFlat direction helpers

        public Tile GetAdjecentTileInDir(int x, int y, HexFlatDir dir)
        {
            bool oddCol = x.IsOdd();

            try
            {
                if (oddCol)
                {
                    switch (dir)
                    {
                        case HexFlatDir.Up:
                            return grid[x, y + 1];
                        case HexFlatDir.Down:
                            return grid[x, y - 1];
                        case HexFlatDir.UpLeft:
                            return grid[x - 1, y + 1];
                        case HexFlatDir.UpRight:
                            return grid[x + 1, y + 1];
                        case HexFlatDir.DownLeft:
                            return grid[x - 1, y];
                        case HexFlatDir.DownRight:
                            return grid[x + 1, y];
                        default:
                            throw new Exception("Unhandled direction");
                    }
                }
                else
                {
                    switch (dir)
                    {
                        case HexFlatDir.Up:
                            return grid[x, y + 1];
                        case HexFlatDir.Down:
                            return grid[x, y - 1];
                        case HexFlatDir.UpLeft:
                            return grid[x - 1, y];
                        case HexFlatDir.UpRight:
                            return grid[x + 1, y];
                        case HexFlatDir.DownLeft:
                            return grid[x - 1, y - 1];
                        case HexFlatDir.DownRight:
                            return grid[x + 1, y - 1];
                        default:
                            throw new Exception("Unhandled direction");
                    }
                }
                
            }
            catch (IndexOutOfRangeException e)
            {
                return null;
            }
        }

        public Tile GetAdjecentTileInDir(Tile tile, HexFlatDir dir)
        {
            return GetAdjecentTileInDir(GetCoordOfTile(tile).x, GetCoordOfTile(tile).y, dir);
        }

        public Tile GetTileInDirCombo(Tile tile, params HexFlatDir[] dirs)
        {
            Tile target = tile;
            foreach (var dir in dirs)
            {
                target = GetAdjecentTileInDir(target, dir);
                if (target == null)
                {
                    return null;
                }
            }
            return target;
        }

        #endregion

    #region HexPointy direction helpers

        public Tile GetAdjecentTileInDir(int x, int y, HexPointyDir dir)
        {
            bool evenRow = y.IsEven();

            try
            {

                if (evenRow)
                {
                    switch (dir)
                    {
                        case HexPointyDir.Left:
                            return grid[x - 1, y];
                        case HexPointyDir.Right:
                            return grid[x + 1, y];
                        case HexPointyDir.UpLeft:
                            return grid[x - 1, y + 1];
                        case HexPointyDir.UpRight:
                            return grid[x, y + 1];
                        case HexPointyDir.DownLeft:
                            return grid[x - 1, y - 1];
                        case HexPointyDir.DownRight:
                            return grid[x, y - 1];
                        default:
                            throw new Exception("Unhandled direction");
                    }
                }
                else
                {
                    switch (dir)
                    {
                        case HexPointyDir.Left:
                            return grid[x - 1, y];
                        case HexPointyDir.Right:
                            return grid[x + 1, y];
                        case HexPointyDir.UpLeft:
                            return grid[x, y + 1];
                        case HexPointyDir.UpRight:
                            return grid[x + 1, y + 1];
                        case HexPointyDir.DownLeft:
                            return grid[x, y - 1];
                        case HexPointyDir.DownRight:
                            return grid[x + 1, y - 1];
                        default:
                            throw new Exception("Unhandled direction");
                    }
                }
                
            }
            catch (IndexOutOfRangeException e)
            {
                return null;
            }
        }

        public Tile GetAdjecentTileInDir(Tile tile, HexPointyDir dir)
        {
            var x = GetCoordOfTile(tile).x;
            return GetAdjecentTileInDir(GetCoordOfTile(tile).x, GetCoordOfTile(tile).y, dir);
        }

        public Tile GetTileInDirCombo(Tile tile, params HexPointyDir[] dirs)
        {
            Tile target = tile;
            foreach (var dir in dirs)
            {
                target = GetAdjecentTileInDir(target, dir);
                if (target == null)
                {
                    return null;
                }
            }
            return target;
        }

        #endregion

        #region Mirrored

        public int GetWidth()
        {
            return grid.GetLength(0);
        }

        public int GetHeight()
        {
            return grid.GetLength(1);
        }

        public virtual Tile GetMirrored(Tile tile, params Mirror[] mirrorDir)
        {
            int newX = GetCoordOfTile(tile).x;
            int newY = GetCoordOfTile(tile).y;
            if (mirrorDir.Contains(Mirror.X))
            {
                newX = GetWidth() - GetCoordOfTile(tile).x;
            }
            if (mirrorDir.Contains(Mirror.X))
            {
                newY = GetHeight() - GetCoordOfTile(tile).y;
            }
            return GetTileAtCoord(newX, newY);
        }

#endregion
        #endregion

        #region Builder
        public GridShape Shape;
            public GameObject TilePrefab;

            protected Tile TileTemplate;

            public Tile AddTile(int x, int y, GameObject tilePrefab)
            {
                if (tilePrefab.GetComponent<Tile>() == null)
                {
                    throw new GridGameException("prefab must contain an Tile Component");
                }
                var tileGO = GameObject.Instantiate(tilePrefab);
                var tile = tileGO.GetComponent<Tile>();
                tileGO.transform.SetParent(this.transform);

                float xPos, yPos;

                AdjustForTileType(tile, out xPos, out yPos, x, y);

                grid[x, y] = tile;
                TileCoord[tile] = new Coordinate2(x,y);
                tileGO.name = String.Format("Tile ({0},{1})", x, y); 

                tileGO.transform.localPosition = new Vector3(xPos, yPos, 0);
                return tile;
            }

            #region tile shape specifics
            protected virtual void AdjustForTileType<T>(T tile, out float xPos, out float yPos, int x, int y) where T : Tile
            {
                xPos = x;
                yPos = y;
                if (tile is Square)
                {
                    return;
                }
                if (tile is HexFlat)
                {
                    xPos = (float)(1.5 / Math.Sqrt(3)) * x;
                    if (x.IsOdd())
                    {
                        yPos += 0.5f;
                    }
                    return;
                }
                if (tile is HexPointy)
                {
                    yPos = (float)(1.5 / Math.Sqrt(3)) * y;
                    if (y.IsOdd())
                    {
                        xPos += 0.5f;
                    }
                    return;
                }
            }
            #endregion

            public void GenerateGrid<S>(GameObject tilePrefab, S shape) where S : GridShape
            {           
                TileCoord = new Dictionary<Tile, Coordinate2>();
                var buildPoints = shape.GetBuildPoints();
                int width, height;
                shape.WidthAndHeight(out width, out height);
                grid = new Tile[width, height];

                TileTemplate = tilePrefab.GetComponent<Tile>();
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (buildPoints[i, j])
                        {
                            AddTile(i, j, tilePrefab);
                        }
                    }
                }
            }
        #endregion

    }

    public static class DirHelper
    {
        public static List<T> GetAllDirEnum<T>()
        {
            return ((T[])Enum.GetValues(typeof(T))).ToList();
        }

        public static List<SquareDir> GetAllSquareDirsNoDiag()
        {
            return new List<SquareDir> {SquareDir.Up, SquareDir.Right, SquareDir.Down, SquareDir.Left};
        }

        public static SquareDir GetNextDir(this SquareDir squareDir, bool clockwise)
        {
            var dirs = GetAllDirEnum<SquareDir>();
            if (squareDir == dirs.Last() && clockwise)
            {
                return dirs.First();
            }
            if (squareDir == dirs.First() && !clockwise)
            {
                return dirs.Last();
            }
            var index = (int)squareDir;
            var change = clockwise ? 1 : -1;
            return dirs[index + change];
        }

        public static HexFlatDir GetNextDir(this HexFlatDir hexFlatDir, bool clockwise)
        {
            var dirs = GetAllDirEnum<HexFlatDir>();
            if (hexFlatDir == dirs.Last() && clockwise)
            {
                return dirs.First();
            }
            if (hexFlatDir == dirs.First() && !clockwise)
            {
                return dirs.Last();
            }
            var index = (int)hexFlatDir;
            var change = clockwise ? 1 : -1;
            return dirs[index + change];
        }

        public static HexPointyDir GetNextDir(this HexPointyDir hexFlatDir, bool clockwise)
        {
            var dirs = GetAllDirEnum<HexPointyDir>();
            if (hexFlatDir == dirs.Last() && clockwise)
            {
                return dirs.First();
            }
            if (hexFlatDir == dirs.First() && !clockwise)
            {
                return dirs.Last();
            }
            var index = (int)hexFlatDir;
            var change = clockwise ? 1 : -1;
            return dirs[index + change];
        }

        public static SquareDir GetOpposite(this SquareDir squareDir)
        {
            var index = (int) squareDir;
            if ( index + 4 <= 7)
            {
                return (SquareDir) (index + 4);
            }
            return (SquareDir)(index - 4);
        }

        public static HexFlatDir GetOpposite(this HexFlatDir hexFlatDir)
        {
            var index = (int)hexFlatDir;
            if (index + 3 <= 5)
            {
                return (HexFlatDir)(index + 3);
            }
            return (HexFlatDir)(index - 3);
        }

        public static HexPointyDir GetOpposite(this HexPointyDir hexPointyDir)
        {
            var index = (int)hexPointyDir;
            if (index + 3 <= 5)
            {
                return (HexPointyDir)(index + 3);
            }
            return (HexPointyDir)(index - 3);
        }
    }

    
}