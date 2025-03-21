using System;
using UnityEngine;

namespace Core.Abilities
{
    public class Selectors : MonoBehaviour
    {
        [SerializeField]
        private Selector[] _selectors;

        public bool GetSelector(Type presetType, out Selector result)
        {
            for (int i = 0; i < _selectors.Length; i++)
            {
                Selector selector = _selectors[i];
                if (selector.PresetType == presetType)
                {
                    result = selector;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public void StopSelection()
        {
            throw new NotImplementedException();
        }
    }
}