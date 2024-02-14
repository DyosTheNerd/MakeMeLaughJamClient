using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Functions (mostly) defined for the signature [0, 1] -> [-inf, +inf]
/// </summary>
public static class EasingFunctions
{ 

    // [-inf, +inf] => [-inf, +inf]
    public static float Linear(float t)
    {
        return t;
    }

    //[0,1] -> [0,1]
    public static float Smoothstep(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }
    
    //[0,1] -> [0,1]
    public static float EaseOut(float t)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
    }

    public static float Impulse(float t)
    {
        return t * Mathf.Exp(1.0f - t);
    }

    public static float Impulse2(float t)
    {
        float f = 1.0f;
        float k = 2.0f;
        float s = Mathf.Max(t - f, 0);

        return Mathf.Min(t*t /(f*f), 1+ (2.0f/f)*s*Mathf.Exp(-k*s));
    }

    //public static float ExpStep(float t)
    //{
    //    float k = 1.0f;
    //    float n = 2.0f;
    //    return Mathf.Exp(-k * Mathf.Pow(t, n));
    //}

    public static float Gain(float t)
    {
        float k = 3.0f;
        float a = 0.5f * Mathf.Pow(2.0f * ((t < 0.5f) ? t : 1.0f - t), k);
        return (t < 0.5f) ? a : 1.0f - a;
    }

    //TODO fix later when i have time.
    // its supposed to be funny ( and work ).
    // need to normalize the bounds.
    //public static float HahaGotYou(float t)
    //{
    //    float k = 2.0f / 3.0f;
    //    return t > 2.0f / 3.0f ? - Linear(t / 3) * k : (Gain((t - k) / (1 + k)) * k)- k;
    //}

    public static System.Func<float, float> RandomFunction()
    {
        int r = Random.Range(0, 5);
        //int r = 6;
        switch (r)
        {
            case 0: return t => Linear(t);
            case 1: return t => Smoothstep(t);
            case 2: return t => EaseOut(t);
            case 3: return t => Impulse(t);
            case 4: return t => Impulse2(t);
            //case 5: return t => ExpStep(t);
            case 5: return t => Gain(t);
            //case 6: return t => HahaGotYou(t);
            default : return t => Linear(t);
        }
    }


}


