using ER.ERBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace ER
{
    /// <summary>
    /// 
    /// </summary>
    public static class Vector2Extensions
    {
        public static float ToAngle(this Vector2 self)
        {
            return -90.0f + Mathf.Atan2(self.y, self.x) * Mathf.Rad2Deg;
        }
    }
}
