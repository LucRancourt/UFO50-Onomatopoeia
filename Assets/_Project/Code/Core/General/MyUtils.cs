using UnityEngine;


namespace _Project.Code.Core.General
{
    public static class MyUtils
    {
        #region Direction
        public static Vector2 GetDirection(Vector2 target, Vector2 self)
        {
            return (target - self).normalized;
        }

        public static Vector3 GetDirection(Vector3 target, Vector3 self)
        {
            return (target - self).normalized;
        }
        #endregion

        #region Clamp
        #region Ints
        public static int Clamp(int valueToClamp, int min, int max)
        {
            if (valueToClamp > max)
                return max;
            else if (valueToClamp < min)
                return min;
            else
                return valueToClamp;
        }

        public static void ClampRef(ref int valueToClamp, int min, int max)
        {
            if (valueToClamp > max)
                valueToClamp = max;
            else if (valueToClamp < min)
                valueToClamp = min;
        }
        #endregion

        #region Floats
        public static float Clamp(float valueToClamp, float min, float max)
        {
            if (valueToClamp > max)
                return max;
            else if (valueToClamp < min)
                return min;
            else
                return valueToClamp;
        }

        public static void ClampRef(ref float valueToClamp, float min, float max)
        {
            if (valueToClamp > max)
                valueToClamp = max;
            else if (valueToClamp < min)
                valueToClamp = min;
        }
        #endregion
        #endregion

        #region Absolute
        #region Returns
        public static int Abs(int a)
        {
            return a > 0 ? a : -a;
        }

        public static float Abs(float a)
        {
            return a > 0.0f ? a : -a;
        }

        public static Vector2 Abs(Vector2 a)
        {
            a.x = Abs(a.x);
            a.y = Abs(a.y);

            return a;
        }

        public static Vector3 Abs(Vector3 a)
        {
            a.x = Abs(a.x);
            a.y = Abs(a.y);
            a.z = Abs(a.z);

            return a;
        }
        #endregion

        #region References
        public static void Abs(ref int a)
        {
            a = a > 0 ? a : -a;
        }

        public static void Abs(ref float a)
        {
            a = a > 0.0f ? a : -a;
        }

        public static void Abs(ref Vector2 a)
        {
            a.x = Abs(a.x);
            a.y = Abs(a.y);
        }

        public static void Abs(ref Vector3 a)
        {
            a.x = Abs(a.x);
            a.y = Abs(a.y);
            a.z = Abs(a.z);
        }
        #endregion
        #endregion

        #region Min
        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }

        public static float Min(float a, float b)
        {
            return a < b ? a : b;
        }
        #endregion

        #region Max
        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }
        #endregion

        #region Approximately
        public static bool Approximately(float a, float b)
        {
            return Abs(b - a) < Max(1E-06f * Max(Abs(a), Abs(b)), Mathf.Epsilon * 8.0f);
        }

        public static bool Approximately(Vector2 a, Vector2 b)
        {
            if (a.x >= b.x - Mathf.Epsilon && a.x <= b.x + Mathf.Epsilon)
                if (a.y >= b.y - Mathf.Epsilon && a.y <= b.y + Mathf.Epsilon)
                    return true;

            return false;
        }

        public static bool Approximately(Vector3 a, Vector3 b, bool ignoreY = false)
        {
            if (Abs(b.x - a.x) < Max(1E-06f * Max(Abs(a.x), Abs(b.x)), Mathf.Epsilon * 8f))
                if ((Abs(b.y - a.y) < Max(1E-06f * Max(Abs(a.y), Abs(b.y)), Mathf.Epsilon * 8f)) || ignoreY)
                    if (Abs(b.z - a.z) < Max(1E-06f * Max(Abs(a.z), Abs(b.z)), Mathf.Epsilon * 8f))
                        return true;

            return false;
        }
        #endregion

        #region Random
        public static int RandomOne()
        {
            return Random.Range(0, 2) * 2 - 1;
        }

        public static bool RandomBool()
        {
            if (RandomOne() > 0)
                return true;
            else
                return false;
        }

        public static int RandomRange(int minInclusive, int maxInclusive)
        {
            return Random.Range(minInclusive, maxInclusive);
        }
        #endregion

        #region Null Values
        public static Vector3 _nullVector3Value { get; private set; } = new Vector3(-666.66f, -666.66f, -666.66f);
        #endregion
    }
}