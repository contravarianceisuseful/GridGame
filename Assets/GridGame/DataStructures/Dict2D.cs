using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GridGame
{
    public class Dict2D<TType, TValue>
    {

        protected Dictionary<TType, Dictionary<TType, TValue>> TypeDict;

        public void SetPair(TType t1, TType t2, TValue value)
        {
            if (TypeDict[t1] == null)
            {
                TypeDict[t1] = new Dictionary<TType, TValue>();
            }
            if (TypeDict[t2] == null)
            {
                TypeDict[t2] = new Dictionary<TType, TValue>();
            }
            TypeDict[t1][t2] = value;
            TypeDict[t2][t1] = value;
        }

        public void RemoveKey(TType t)
        {
            foreach (var key in TypeDict[t].Keys)
            {
                TypeDict[key].Remove(t);
            }
            TypeDict.Remove(t);
        }

        public TValue GetPairValue(TType t1, TType t2)
        {
            var tempP = TypeDict[t1][t2];
            var tempS = TypeDict[t2][t1];

            if (tempP.Equals(tempS) && tempS.Equals(tempP))
            {
                return tempP;
            }
            throw new GridGameException("Elements are not synced. Two possible values. Use individual getter methods");
        }
    }
}

