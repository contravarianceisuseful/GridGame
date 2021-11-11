using System;
using System.Collections.Generic;

namespace GridGame
{

    public interface IDubDictManySecondaries<in TPrimary, TSecondary>
    {
        List<TSecondary> GetSecondaryList(TPrimary key);
    }

    public interface IDubDictManyPrimaries<TPrimary, in TSecondary>
    {
        List<TPrimary> GetPrimaryList(TSecondary key);
    }

    public abstract class DoubleDict<TPrimary, TSecondary> 
    {
        protected Dictionary<TPrimary, List<TSecondary>> PrimaryToSecondary;
        protected Dictionary<TSecondary, List<TPrimary>> SecondaryToPrimary;

        protected DoubleDict()
        {
            PrimaryToSecondary = new Dictionary<TPrimary, List<TSecondary>>();
            SecondaryToPrimary = new Dictionary<TSecondary, List<TPrimary>>();
        }

        public abstract void AddPair(TPrimary primary, TSecondary secondary);

        public void RemovePair(TPrimary primary, TSecondary secondary)
        {
            if (!ContainsPair(primary, secondary))
            {
                throw new Exception("Double Dict does not contain that pair");
            }
            PrimaryToSecondary[primary].Remove(secondary);
            if (PrimaryToSecondary[primary].Count == 0)
            {
                PrimaryToSecondary.Remove(primary);
            }
            SecondaryToPrimary[secondary].Remove(primary);
            if (SecondaryToPrimary[secondary].Count == 0)
            {
                SecondaryToPrimary.Remove(secondary);
            }
        }

        public void RemovePrimary(TPrimary primary)
        {
            var tempSecondaries = PrimaryToSecondary[primary];
            foreach (var secondary in tempSecondaries)
            {
                RemovePair(primary, secondary);
            }
        }

        public void RemoveSecondary(TSecondary secondary)
        {
            var tempPrimaries =SecondaryToPrimary[secondary];
            foreach (var primary in tempPrimaries)
            {
                RemovePair(primary, secondary);
            }
        }

        public virtual void SetPair(TPrimary primary, TSecondary secondary)
        {
            if (ContainsPair(primary, secondary))
            {
                RemovePair(primary, secondary);
            }
            AddPair(primary, secondary);
        }

        public bool ContainsPair(TPrimary primary, TSecondary secondary)
        {
            bool pToS = PrimaryToSecondary[primary].Contains(secondary);
            bool sToP = SecondaryToPrimary[secondary].Contains(primary);

            if (pToS && sToP)
            {
                return true;
            }

            if (pToS || sToP) //XOR because of above statement
            {
                throw new GridGameException("DoubleDictionary out of sync");
            }

            return false;
        }

        public bool ContainsPrimary(TPrimary primary)
        {
            return PrimaryToSecondary.ContainsKey(primary) && PrimaryToSecondary[primary] != null;
        }

        public bool ContainsSecondary(TSecondary secondary)
        {
            return SecondaryToPrimary.ContainsKey(secondary) && SecondaryToPrimary[secondary] != null;
        }
    }

    public class DoubleDictOneToOne<TPrimary, TSecondary> : DoubleDict<TPrimary, TSecondary> 
    {
        public override void AddPair(TPrimary primary, TSecondary secondary)
        {
            PrimaryToSecondary[primary] = new List<TSecondary> {secondary};
            SecondaryToPrimary[secondary] = new List<TPrimary> {primary};
        }

        public TPrimary GetPrimary(TSecondary key)
        {
            if (!SecondaryToPrimary.ContainsKey(key))
            {
                throw new GridGameException("Key not found");
            }
            return SecondaryToPrimary[key][0];
        }

        public TSecondary GetSecondary(TPrimary key)
        {
            if (!PrimaryToSecondary.ContainsKey(key))
            {
                throw new Exception("Key not found");
            }
            return PrimaryToSecondary[key][0];
        }
    }

    public class DoubleDictOneToMany<TPrimary, TSecondary> : DoubleDict<TPrimary, TSecondary>, IDubDictManySecondaries<TPrimary, TSecondary>
    {
        public override void AddPair(TPrimary primary, TSecondary secondary)
        {
            if (!PrimaryToSecondary.ContainsKey(primary) || PrimaryToSecondary[primary] == null)
            {
                PrimaryToSecondary[primary] = new List<TSecondary>();
            }
            PrimaryToSecondary[primary].Add(secondary);
            SecondaryToPrimary[secondary] = new List<TPrimary> {primary};
        }

        public TPrimary GetPrimary(TSecondary key)
        {
            if (!SecondaryToPrimary.ContainsKey(key))
            {
                throw new GridGameException("Key not found");
            }
            return SecondaryToPrimary[key][0];
        }

        public List<TSecondary> GetSecondaryList(TPrimary key)
        {
            if (!PrimaryToSecondary.ContainsKey(key))
            {
                throw new Exception("Key not found");
            }
            return PrimaryToSecondary[key];
        }
    }

    public class DoubleDictManyToMany<TPrimary, TSecondary> : DoubleDict<TPrimary, TSecondary>, IDubDictManyPrimaries<TPrimary, TSecondary>, IDubDictManySecondaries<TPrimary, TSecondary>
    {
        public override void AddPair(TPrimary primary, TSecondary secondary)
        {
            bool pToS = PrimaryToSecondary[primary].Contains(secondary);
            bool sToP = SecondaryToPrimary[secondary].Contains(primary);

            if (pToS && sToP)
            {
                throw new GridGameException("Variables already synced");
            }

            if (pToS || sToP) //XOR because of above statement
            {
                throw new GridGameException("DoubleDictionary out of sync");
            }

            if (PrimaryToSecondary[primary] == null)
            {
                PrimaryToSecondary[primary] = new List<TSecondary>();
            }
            if (SecondaryToPrimary[secondary] == null)
            {
                SecondaryToPrimary[secondary] = new List<TPrimary>();
            }

            PrimaryToSecondary[primary].Add(secondary);
            SecondaryToPrimary[secondary].Add(primary);
        }

        public List<TPrimary> GetPrimaryList(TSecondary key)
        {
            if (!SecondaryToPrimary.ContainsKey(key))
            {
                throw new Exception("Key not found");
            }
            return SecondaryToPrimary[key];
        }

        public List<TSecondary> GetSecondaryList(TPrimary key)
        {
            if (!PrimaryToSecondary.ContainsKey(key))
            {
                throw new Exception("Key not found");
            }
            return PrimaryToSecondary[key];
        }
    }
}