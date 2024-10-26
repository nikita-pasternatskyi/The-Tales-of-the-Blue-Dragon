using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Game.Utils
{
    [System.Serializable]
    public struct RandomSelectFromArray<T>
    {
        public T[] Items;
        public bool Sequential;
        private int _lastIdx;
        private int _tries;
        public T GetRandom()
        {
            if(Sequential)
            {
                if (_lastIdx == Items.Length)
                    _lastIdx = 0;
                return Items[_lastIdx++];
            }
            int idx = Random.Range(0, Items.Length);
            if (_lastIdx == idx && _tries > 3)
            {
                idx = Random.Range(idx + 1, Items.Length);
                _tries = 0;
            }
            idx = Mathf.Clamp(idx, 0, Items.Length - 1);
            _tries++;
            _lastIdx = idx + 1;
            return Items[idx];
        }
    }
}
