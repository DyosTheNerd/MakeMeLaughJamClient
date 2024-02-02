using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    public static UIAnimationController instance;

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

    public bool AreAnimationsPlaying()
    {
        return AnimationsPlaying != 0;
    }

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

    public void PlayOverlordJudgmentAnimation()
    {

    }

    public void EnablePlayerVotingAnimation(AnimationStopCondition stopCondition)
    {

    }

    public void EnableVoteEvaluationAnimation(AnimationStopCondition stopCondition) 
    { 
    
    }

    public void EnablePlayersVotingAndTimeoutMessage(AnimationStopCondition stopCondition)
    {

    }

    public void PlayOverlordJudgmentResultAnimation()
    {

    }

    public void PlayPrepareForNextRoundAnimation()
    {

    }

}
