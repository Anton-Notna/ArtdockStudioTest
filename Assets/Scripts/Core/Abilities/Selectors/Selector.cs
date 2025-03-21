using System;
using System.Collections;
using UnityEngine;

namespace Core.Abilities
{
    public abstract class Selector : MonoBehaviour
    {
        public abstract Type PresetType { get; }

        public abstract IEnumerator Select(SelectorPreset preset, ISelectionContext selection);
    }
}