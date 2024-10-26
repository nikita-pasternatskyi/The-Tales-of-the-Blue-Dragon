using UnityEngine;

namespace MP.Game.Utils
{
    [System.Serializable]
    public struct MinMax
    {
        public float Min;
        public float Max;
        public float GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }
}
