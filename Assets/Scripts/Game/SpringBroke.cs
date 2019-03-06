using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SpringBroke : StateMachineBehaviour
{
    private Vector3 normal_Rotation;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform spring = animator.transform;
        normal_Rotation = spring.eulerAngles;
        RandonVelocity(spring);

    }
    void RandonVelocity(Transform spring)
    {

        if (spring.tag.Contains(MyTags.Right_Tag))
        {
            float x_A = Random.Range(1, 3);
            float y_A = Random.Range(2, 5);

            Vector3 target_A = Vector3.right * (-x_A) + Vector3.up * (y_A);

            spring.DOBlendableMoveBy(target_A, 1f).SetEase(Ease.OutExpo);

            spring.DOBlendableMoveBy(Vector3.up * (-30f), 1f).SetEase(Ease.InExpo);

            spring.DOLocalRotate(new Vector3(0, 0, 90), 0.4f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
            spring.DOPlay();
        }
        else
        {
            float x_B = Random.Range(1, 3);
            float y_B = Random.Range(2, 5);

            Vector3 target = Vector3.right * (x_B) + Vector3.up * (y_B);

            spring.DOBlendableMoveBy(target, 1f).SetEase(Ease.OutExpo);

            spring.DOBlendableMoveBy(Vector3.up * (-30f), 1f).SetEase(Ease.InExpo);

            spring.DORotate(new Vector3(0, 0, 270), 0.4f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
            spring.DOPlay();
        }
        if (SceneManager.GetActiveScene().name.Contains("EndlessMode"))
        {
            HumanManager.Nature.HumanManager_Script.StartCoroutine(Wait(spring));
        }

    }

    IEnumerator Wait(Transform spring)
    {
        yield return new WaitForSeconds(1);
        spring.DOKill();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
