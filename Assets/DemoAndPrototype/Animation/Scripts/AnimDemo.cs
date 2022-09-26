using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EGF.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

public class AnimDemo : MonoBehaviour
{
    public Animator animator;
    
    private readonly int jumpForward = Animator.StringToHash("Base Layer.jump-forward");
    
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
    async UniTaskVoid PlayList()
    {
        // TODO: 动画取消，比如在播放该动作组时，角色突然死亡，将停止该动作组
        // await animPlayer.Play("jump forward");
        // await animPlayer.Play("Base Layer.rifle run");
        // await animPlayer.Play("Base Layer.start walking backwards",0,0,0.3f);
        
        // await animator.PlayOnce("jump-forward");
        // await animator.PlayOnce("RifleRun2");
        float startTime = Time.timeSinceLevelLoad;
        
        await animator.PlayOnce("start-walking-backwards",0,0,0.3f);
        
        float usedTime = Time.timeSinceLevelLoad - startTime;
        Debug.Log($"1 Time Using: {usedTime}");
        startTime = Time.timeSinceLevelLoad;
        
        await animator.PlayOnce("walking-backwards",0,0,0.15f,0.8f);
        
        usedTime = Time.timeSinceLevelLoad - startTime;
        Debug.Log($"2 Time Using: {usedTime}");
        startTime = Time.timeSinceLevelLoad;
        
        await animator.PlayOnce("walk-backwards-stop");
        
        usedTime = Time.timeSinceLevelLoad - startTime;
        Debug.Log($"3 Time Using: {usedTime}");
        startTime = Time.timeSinceLevelLoad;
        
        await animator.PlayOnce("Walking1");
        
        usedTime = Time.timeSinceLevelLoad - startTime;
        Debug.Log($"4 Time Using: {usedTime}");
    }
}
