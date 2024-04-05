using UnityEngine;

namespace DeadNation
{
    public static class Calculating
    {
        public static bool Probability(int rate)
        {
            return UnityEngine.Random.Range(0, 101) <= rate;
        }
    }
}