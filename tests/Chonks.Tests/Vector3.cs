using System;

namespace Chonks.Tests {
    public struct Vector3 {
        // *Undocumented*
        public const float kEpsilon = 0.00001F;
        // *Undocumented*
        public const float kEpsilonNormalSqrt = 1e-15F;

        // X component of the vector.
        public float x;
        // Y component of the vector.
        public float y;
        // Z component of the vector.
        public float z;
        public Vector3 normalized { get { return Vector3.Normalize(this); } }
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }


        static readonly Vector3 zeroVector = new Vector3(0F, 0F, 0F);
        public static Vector3 zero { get { return zeroVector; } }
        public static float Magnitude(Vector3 vector) { return (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z); }
        public static Vector3 Normalize(Vector3 value) {
            float mag = Magnitude(value);
            if (mag > kEpsilon)
                return value / mag;
            else
                return zero;
        }
        public static Vector3 operator /(Vector3 a, float d) { return new Vector3(a.x / d, a.y / d, a.z / d); }
    }
}
