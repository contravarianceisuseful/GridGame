using UnityEngine;
using System.Collections;
using BoardGame;
using GridGame;
using Multiplayer;

public class StartCheckersGame : MonoBehaviour
{

    public SquareWithDiags SquarePrefab;
    public GameObject PiecePrefab;
    public GameObject PlayerPrefab;
    public GameController Controller;
    protected Grid Grid;

    protected Player Player1;
    protected Player Player2;

    // Use this for initialization
    void Start () {
        Controller.GetComponentInChildren<UnitOwnershipHandler>().Initialize();
        Controller.GetComponentInChildren<PositionHandler>().Initialize();
        StartGame();
	}

    protected void StartGame()
    {
        SetUpBoard();
        SetUpPlayers();
        AddPieces();
    }

    

    protected void SetUpBoard()
    {
        Grid = Controller.GetComponentInChildren<Grid>();
        Grid.GenerateGrid(SquarePrefab.gameObject, Grid.Shape);
    }

    protected void SetUpPlayers()
    {
        var players = new GameObject {name = "Players"};
        Player1 = CreatePlayer(false);
        Player1.gameObject.name = "Player1";
        Player1.ID = 1;

        Player2 = CreatePlayer(true);
        Player2.gameObject.name = "Player2";
        Player2.ID = 2;

        
    }

    protected Player CreatePlayer(bool inverted)
    {
        var playerGO = GameObject.Instantiate(PlayerPrefab);
        if (inverted)
        {
            playerGO.AddComponent<Inverted>();
        }
        return playerGO.GetComponent<Player>();
    }

    protected void AddPieces()
    {
        //player 1
        foreach (var tile in Grid.GetAllTiles())
        {
            var coord = Grid.GetCoordOfTile(tile);
            if (coord.y <= 2 && IsOddSquare(tile))
            {
                AddPiece(Player1, tile);
            }
        }

        //player 2
        foreach (var tile in Grid.GetAllTiles())
        {
            var coord = Grid.GetCoordOfTile(tile);
            if (coord.y >= 5 && IsOddSquare(tile))
            {
                AddPiece(Player2, tile);
            }
        }
    }

    protected void AddPiece(Player player, Tile tile)
    {
        var tempTile = tile;
        var color = Color.gray;
        if (player.HasComponent<Inverted>())
        {
            tempTile = Grid.GetMirrored(tile, Mirror.Y);
            color = Color.black;
        }
        var pieceGO = GameObject.Instantiate(PiecePrefab);
        pieceGO.GetComponent<SpriteRenderer>().color = color;

        var unit = pieceGO.GetComponent<Unit>();
        Controller.GetComponentInChildren<UnitOwnershipHandler>().AddUnitToPlayer(unit, player);

        var mover = pieceGO.GetComponent<Mover>();
        Controller.GetComponentInChildren<PositionHandler>().AddMover(mover, tile);
    }

    private bool IsOddSquare(Tile tile)
    {
        return (Grid.GetCoordOfTile(tile).x + Grid.GetCoordOfTile(tile).y).IsOdd();
    }
}
