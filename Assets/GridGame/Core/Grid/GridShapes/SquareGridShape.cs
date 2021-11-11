using UnityEngine;

public class SquareGridShape : GridShape
{
    [SerializeField]
    protected int Size;

    public override bool[,] GetBuildPoints()
    {
        var buildPoints = new bool[Size, Size];
        buildPoints.PopulateShape(x => true);
        return buildPoints;
    }
}