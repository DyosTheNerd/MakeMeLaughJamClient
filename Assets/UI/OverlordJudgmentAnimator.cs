using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OverlordJudgmentAnimator : MonoBehaviour
{
    
    [Header("Objects and Positions")]
    public RectTransform pointer;
    public RectTransform backdrop;

    public Vector3 badMoodPosition;
    public Vector3 goodMoodPosition;

    public float oldOverlordMood;
    public float overlordMood;

    [Range(0,1)]
    //public float t;
    public RectTransform rectTransform;


    bool isPlaying = false;

    public bool IsPlaying()
    {
        return isPlaying;
    }

    IEnumerator JudgingCoroutine(float duration)
    {
        isPlaying = true;

        var randomEasingFunction = EasingFunctions.RandomFunction();
        float t = 0.0f;

        while(t < duration)
        {
            SetPointerPosition(randomEasingFunction(t/ duration));
            t += Time.deltaTime;
            yield return null;
        }
        isPlaying = false;
    }

    public void PlayOverLordJudgeAnimation()
    {

        oldOverlordMood = (float)OverlordJudging.instance.oldOverlordMood / 100.0f; //old T value
        overlordMood = (float)OverlordJudging.instance.overlordMood / 100.0f; //desired T value

        StartCoroutine(JudgingCoroutine(5.0f));
    }

    void SetPointerPosition(float t)
    {

        float mood_t = Mathf.Lerp(oldOverlordMood, overlordMood, t);
        mood_t = Mathf.Clamp01(mood_t);

        pointer.position = (rectTransform.position + new Vector3(0, -rectTransform.rect.height / 8, 0) + badMoodPosition) * (1-mood_t) +
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
