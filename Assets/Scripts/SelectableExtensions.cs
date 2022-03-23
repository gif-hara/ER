using System.Collections;
using System.Collections.Generic;
using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class SelectableExtensions
    {
        public static void SetupVerticalNavigations<T>(this IList<T> self) where T : Selectable
        {
            for (var i = 0; i < self.Count; i++)
            {
                var navigation = new Navigation();
                navigation.mode = Navigation.Mode.Explicit;
                var upIndex = i - 1;
                if (upIndex < 0)
                {
                    upIndex = self.Count - 1;
                }
                var downIndex = i + 1;
                if (downIndex >= self.Count)
                {
                    downIndex = 0;
                }

                navigation.selectOnUp = self[upIndex];
                navigation.selectOnDown = self[downIndex];

                self[i].navigation = navigation;
            }
        }
    }
}
