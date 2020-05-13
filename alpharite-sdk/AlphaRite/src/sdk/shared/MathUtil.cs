using System;
using UnityEngine;
using Vector2 = MathCore.Vector2;
using Vector3 = MathCore.Vector3;

namespace AlphaRite.sdk {
    public static class MathUtil {
        public static float normSquare(this Vector2 self) {
            return (float) Math.Pow(Math.Abs(self.X), 2) + (float) Math.Pow(Math.Abs(self.Y), 2);
        }

        public static float scalar(this Vector2 self, Vector2 other) {
            return self.X * other.X + self.Y * other.Y;
        }

        public static Vector2 projection(Vector2 source, Vector2 target) {
            var tmp = source.scalar(target) / target.normSquare();
            var projection = target * tmp;
            return projection;
        }
        
        public static float magnitude(this Vector2 self) {
            return Mathf.Sqrt( self.X * self.X + self.Y * self.Y);
        }
    }
}