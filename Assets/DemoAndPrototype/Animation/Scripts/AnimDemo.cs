using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EGF.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimDemo : MonoBehaviour
{
    public Animator animator;
    [FormerlySerializedAs("CompareAnimator")] public Animator compareAnimator;        // 对照，普通动画连接
    
    private readonly int jumpForward = Animator.StringToHash("Base Layer.jump-forward");
    private static readonly int StatusId = Animator.StringToHash("statusId");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [Button]
    void ResetPosition()
    {
        var trans = animator.transform;
        trans.position = Vector3.zero;
        trans.rotation = Quaternion.identity;
    }

    [Button]
    void Play(string stateName)
    {
        animator.PlayOnce(stateName);
    }

    [Button]
    async UniTaskVoid PlayListWithCompare()
    {
        // TODO: 动画取消，比如在播放该动作组时，角色突然死亡，将停止该动作组
        
        compareAnimator.SetInteger(StatusId, 0);
        compareAnimator.SetTrigger("Test");
        
        await animator.PlayOnce("jump-forward");
        await animator.PlayOnce("jump-backward");
        await animator.PlayOnce("walking-to-dying");
    }

    [Button]
    async UniTaskVoid PlayRepeat()
    {
        await animator.PlayOnce("jump-forward");
        await animator.PlayOnce("jump-forward");
        await animator.PlayOnce("jump-forward");
    }

    [Button]
    void PlayRepeatOneByOne()
    {
        animator.PlayOnce("jump-forward");
        animator.PlayOnce("jump-forward");
        animator.PlayOnce("jump-forward");
    }
}
