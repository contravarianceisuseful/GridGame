using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


namespace Pathfinder
{
    public static class PathfinderHelper
    {

        public static List<T> GetPathTo<T>(T origin, T target, Func<T, List<T>> GetNeighbours, Func<T, float> CostFunc)
            where T : class
        {
            var frontier = new LowPriorityList<T> {origin};
            var cameFrom = new Dictionary<T, T>();
            var costSoFar = new Dictionary<T, float>();

            costSoFar[origin] = 0;

            while (frontier.Count > 0)
            {
                T current = frontier.Dequeue();
                foreach (var neighbour in GetNeighbours(current))
                {
                    float newCost = costSoFar[current] + CostFunc(neighbour);
                    if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
                    {
                        costSoFar[neighbour] = newCost;
                        frontier.Enqueue(neighbour, newCost);
                        cameFrom[neighbour] = current;
                    }
                    if (current == target)
                    {
                        return BuildPath(cameFrom, target, origin);
                    }
                }               
            }   
            return null;
        }
        private static List<T> BuildPath<T>(Dictionary<T, T> cameFrom, T target, T origin) where T : class 
        {
            var path = new List<T>();
            var iterator = target;
            while (iterator != origin)
            {
                path.Add(iterator);
                iterator = cameFrom[iterator];
            }
            path.Reverse();
            return path;
        }
    }



    public class LowPriorityList<T> : List<T>
    {
        protected Dictionary<T, float> costDict;

        protected float maxP;

        public T Enqueue(T element, float priority)
        {
            if (costDict == null)
            {
                costDict = new Dictionary<T, float>();
            }
            costDict[element] = priority;
            this.Add(element);
            return element;
        }

        public T Dequeue()
        {
            var temp = MinBy(this, x => costDict[x]);
            this.Remove(temp);
            return temp;
        }

        public static T MinBy(IEnumerable<T> source, Func<T, IComparable> score)
        {
            return source.Aggregate((x, y) => score(x).CompareTo(score(y)) < 0 ? x : y);
        }
    }
}
