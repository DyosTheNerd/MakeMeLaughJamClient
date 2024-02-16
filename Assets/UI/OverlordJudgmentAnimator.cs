using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OverlordJudgmentAnimator : MonoBehaviour
{
    
    [Header("Objects and Positions")]
    public RectTransform pointer;
    public RectTransform shadowPointer;
    public RectTransform backdrop;
    public Material ShadowPointerMaterial;
    public Material PointerMaterial;

    public Vector3 badMoodPosition;
    public Vector3 goodMoodPosition;

    [Header("Presentation Data")]

    public float oldOverlordMood;
    public float overlordMood;
    //[Range(0,1)]
    //public float t;
    public RectTransform rectTransform;


    [Header("Presentation Parameters")]
    public float pointerGlowFrequency = 15.0f;
    public float pointerGlowIntensity = 10.0f;

    bool isPlaying = false;

    public bool IsPlaying()
    {
        return isPlaying;
    }

    IEnumerator FadeShadowPointer(float duration)
    {
        float t = 0.0f;
        while(t < duration)
        {
            ShadowPointerMaterial.SetFloat("_Fadeout", 0.5f + t / duration * 0.5f) ;
            t += Time.deltaTime;
            yield return null;
        }

        shadowPointer.gameObject.SetActive(false);
    }

    IEnumerator GlowPointer(float duration)
    {
        float t = 0.0f;
        while (t < duration / 2.0)
        {
            PointerMaterial.SetFloat("_Brightness", 10.0f + Mathf.Sin(t * pointerGlowFrequency * Mathf.PI / duration) * pointerGlowIntensity);
            t += Time.deltaTime;
            yield return null;
        }

        while(t > 0)
        {
            PointerMaterial.SetFloat("_Brightness", 10.0f + Mathf.Sin(t * pointerGlowFrequency * Mathf.PI / duration) * pointerGlowIntensity);
            t -= Time.deltaTime;
            yield return null;
        }
        
        PointerMaterial.SetFloat("_Brightness", 10.0f);

        //shadowPointer.gameObject.SetActive(false);
    }

    IEnumerator JudgingCoroutine(float duration)
    {
        isPlaying = true;

        var randomEasingFunction = EasingFunctions.RandomFunction();
        float t = 0.0f;

        shadowPointer.gameObject.SetActive(true);

        SetPointerPosition(0, shadowPointer);
        ShadowPointerMaterial.SetFloat("_Fadeout", 0.5f);

        StartCoroutine(GlowPointer(duration));

        while (t < duration)
        {
            SetPointerPosition(randomEasingFunction(t/ duration), pointer);
            t += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeShadowPointer(2));

        isPlaying = false;
    }

    public void PlayOverLordJudgeAnimation()
    {

        oldOverlordMood = (float)OverlordJudging.instance.oldOverlordMood / 100.0f; //old T value
        overlordMood = (float)OverlordJudging.instance.overlordMood / 100.0f; //desired T value

        StartCoroutine(JudgingCoroutine(5.0f));
    }

    void SetPointerPosition(float t, RectTransform target)
    {

        float mood_t = Mathf.Lerp(oldOverlordMood, overlordMood, t);
        mood_t = Mathf.Clamp01(mood_t);

        target.position = (rectTransform.position + new Vector3(0, -rectTransform.rect.height / 8, 0) + badMoodPosition) * (1-mood_t) +
                           (rectTransform.position + new Vector3(rectTransform.rect.width / 4, -rectTransform.rect.height / 8, 0) + goodMoodPosition) * mood_t;
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(rectTransform.position + new Vector3(0,-rectTransform.rect.height / 8,0)+ badMoodPosition, "sadIcon.png", true);
        Gizmos.DrawIcon(rectTransform.position + new Vector3(rectTransform.rect.width/4,-rectTransform.rect.height / 8,0)+ goodMoodPosition, "happyIcon.png", true);
    }

#endif 

}
