using System;
using UnityEngine;
using System.Collections;
using GridGame;

public abstract class GridShape : MonoBehaviour
{
    public virtual void WidthAndHeight(out int width, out int height)
    {
        width = GetBuildPoints().GetLength(0);
        height = GetBuildPoints().GetLength(1);
    }

    public abstract bool[,] GetBuildPoints();  
}

public static class ShapePopulator
{
    public static void PopulateShape(this bool[,] arr, Predicate<Coordinate2> condition)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                arr[i, j] = condition(new Coordinate2(i, j));
            }
        }
    }
}

