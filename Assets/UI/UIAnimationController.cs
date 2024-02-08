using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    public static UIAnimationController instance;

    [Header("Drag and Drop Parameters")]
    public PlayedCardCountAnimator playedCardCountAnimator;
    //public PlayedCardIntensityAnimatorSDF playedCardIntensityAnimator;
    public GameLoopMessaging loopMessager;

    [Header("UI Objects")]
    public GameObject Alien;
    public GameObject Cards;

    public GameObject MessageCard;


    [Header("Data")]
    private int animationsPlaying = 0;

    private int AnimationsPlaying { get => animationsPlaying; 
        set { 
            animationsPlaying = value;
            if (animationsPlaying == 0) AnimationsEnded?.Invoke();
        } }

    public delegate void OnAnimationsEnded();
    public event OnAnimationsEnded AnimationsEnded;

    public delegate bool AnimationStopCondition();

    void Awake()
    {
        if (instance == null)
        {
            UIAnimationController.instance = this;
        }
        else
        {
            Debug.LogError("Multiple UIAnimationController instances detected.");
        }
    }

    public bool AreAnimationsPlaying()
    {
        return AnimationsPlaying != 0;
    }

    IEnumerator PlayOverlordJudgmentAnimationRoutine()
    {
        AnimationsPlaying++;

        yield return null;

        AnimationsPlaying--;
    }

    public void PlayOverlordJudgmentAnimation()
    {
        StartCoroutine(PlayOverlordJudgmentAnimationRoutine());
    }

    IEnumerator EnablePlayerVotingAnimationRoutine(AnimationStopCondition stopCondition)
    {
        AnimationsPlaying++;
        playedCardCountAnimator.AnimationEnabled = true;
        loopMessager.EnablePlayersVotingMessage();
        yield return new WaitUntil(() => stopCondition());
        playedCardCountAnimator.AnimationEnabled = false;
        loopMessager.DisablePlayersVotingMessage();
        AnimationsPlaying--;
    }

    public void EnablePlayerVotingAnimation(AnimationStopCondition stopCondition)
    {
        StartCoroutine(EnablePlayerVotingAnimationRoutine(stopCondition));
    }

    IEnumerator EnableVoteEvaluationAnimationRoutine(AnimationStopCondition stopCondition)
    {
        AnimationsPlaying++;

        yield return new WaitUntil(() => stopCondition());

        AnimationsPlaying--;
    }
    public void EnableVoteEvaluationAnimation(AnimationStopCondition stopCondition) 
    { 
    
    }

    IEnumerator EnablePlayersVotingAndTimeoutMessageRoutine(AnimationStopCondition stopCondition)
    {
        AnimationsPlaying++;

        yield return new WaitUntil(() => stopCondition());

        AnimationsPlaying--;
    }
    public void EnablePlayersVotingAndTimeoutMessage(AnimationStopCondition stopCondition)
    {

    }

    IEnumerator PlayOverlordJudgmentResultAnimationRoutine()
    {
        AnimationsPlaying++;

        yield return new WaitForSeconds(10);

        AnimationsPlaying--;
    }

    public void PlayOverlordJudgmentResultAnimation()
    {

    }

    IEnumerator PlayPrepareForNextRoundAnimationRoutine()
    {
        AnimationsPlaying++;

        yield return new WaitForSeconds(10);

        AnimationsPlaying--;
    }
    public void PlayPrepareForNextRoundAnimation()
    {

    }

}
