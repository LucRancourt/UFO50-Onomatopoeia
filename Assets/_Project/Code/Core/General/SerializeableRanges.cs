using System;

namespace _Project.Code.Core.General
{
    [Serializable]
    public struct FloatRange
    {
        public float Min;
        public float Max;

        public FloatRange(float min, float max)
        {
            if (min < max)
            {
                Min = min;
                Max = max;
            }
            else
            {
                Min = max;
                Max = min;
            }
        }

        public float RandomValue()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }

    [Serializable]
    public struct IntRange
    {
        public int Min;
        public int Max;

        public IntRange(int min, int max)
        {
            if (min < max)
            {
                Min = min;
                Max = max;
            }
            else
            {
                Min = max;
                Max = min;
            }
        }

        public float RandomValue()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}