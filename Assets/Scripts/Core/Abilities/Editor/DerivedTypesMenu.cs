using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Core.Abilities.Editor
{
    public class DerivedTypesMenu
    {
        private readonly GenericMenu _menu;

        public DerivedTypesMenu(Type baseType, Action<Type> onSelected)
        {
            _menu = new GenericMenu();

            IEnumerable<Type> componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(baseType) && type.IsAbstract == false);

            foreach (Type type in componentTypes)
                _menu.AddItem(new GUIContent(type.Name), false, () => onSelected.Invoke(type));
        }

        public void ShowAsContext() => _menu.ShowAsContext();
    }
}