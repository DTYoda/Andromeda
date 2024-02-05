using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundScriptRun : StateMachineBehaviour
{
    public AudioSource source;
    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip landClip;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        source = GameObject.Find("Player").GetComponent<AudioSource>();

        if(stateInfo.IsName("Run"))
        {
            source.loop = true;
            source.clip = runClip;
        }
        else if (stateInfo.IsName("Jump"))
        {
            source.loop = false;
            source.clip = jumpClip;
        }
        else if (stateInfo.IsName("land"))
        {
            source.loop = false;
            source.clip = landClip;
        }
        source.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        source.Stop();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
