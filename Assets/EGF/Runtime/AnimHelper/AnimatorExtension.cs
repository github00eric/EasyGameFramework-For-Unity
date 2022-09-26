using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace EGF.Runtime
{
    public static class AnimatorExtension
    {
        /// <summary>
        /// 直接过渡到指定动画状态播放一次
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateName">动画状态机名</param>
        /// <param name="layer">状态机所在层</param>
        /// <param name="normalizedStartTime">开始时间(归一化)</param>
        /// <param name="normalizedTransition">过渡耗时(归一化)</param>
        /// <param name="normalizedExitTime">退出时间(归一化)</param>
        /// <returns></returns>
        public static async UniTask PlayOnce(this Animator animator ,string stateName, int layer = 0, float normalizedStartTime = 0, float normalizedTransition = 0.15f, float normalizedExitTime = 0.8f)
        {
            Debug.Log($"Play: {stateName}");
            animator.CrossFade(stateName, normalizedTransition, layer, normalizedStartTime);
            
        
            // 等待过渡完成
            var animationStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            //var currentClip = animator.GetCurrentAnimatorClipInfo(layer);
            // for (int i = 0; i < currentClip.Length; i++)
            // {
            //     var state = currentClip[i].clip;
            // }
            //Debug.Log($"currentClip 1->{currentClip.name}");
            //var transitionDuration = currentClip.length * (normalizedTransition + float.Epsilon);
            //Debug.Log($"animationStateInfo ->{animationStateInfo.normalizedTime} transitionDuration->{transitionDuration}");
            
            await UniTask.Delay(TimeSpan.FromSeconds(animationStateInfo.length), DelayType.DeltaTime);
            
            //
            //currentClip = animator.GetNextAnimatorClipInfo(layer)[0].clip;
            
            //Debug.Log($"currentClip 2->{currentClip.name}");

            //var startTime = animator.GetNextAnimatorStateInfo(layer).normalizedTime;

            //startTime = startTime - (int) startTime;
            //var playDuration = currentClip.length * (normalizedExitTime - startTime);
        
            await UniTask.Delay(TimeSpan.FromSeconds(animationStateInfo.length), DelayType.DeltaTime);
        }
        
        /// <summary>
        /// 直接过渡到指定动画状态播放一次，直接用 Hash 效率更高
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateHashName">动画状态机Hash</param>
        /// <param name="layer">状态机所在层</param>
        /// <param name="normalizedStartTime">开始时间(归一化)</param>
        /// <param name="normalizedTransition">过渡耗时(归一化)</param>
        /// <param name="normalizedExitTime">退出时间(归一化)</param>
        /// <returns></returns>
        public static async UniTask PlayOnce(this Animator animator ,int stateHashName, int layer = 0, float normalizedStartTime = 0, float normalizedTransition = 0.15f, float normalizedExitTime = 0.8f)
        {
            animator.CrossFade(stateHashName, normalizedTransition, layer, normalizedStartTime);
        
            var currentClip = animator.GetCurrentAnimatorClipInfo(layer)[0].clip;
            var transitionDuration = currentClip.length * normalizedExitTime;
        
            await UniTask.Delay(TimeSpan.FromSeconds(transitionDuration), DelayType.DeltaTime);
        }
        
        /// <summary>
        /// HACK：直接过渡到指定动画状态播放一次，且播放持续时间将强制更改为指定时间
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateName"></param>
        /// <param name="playTime"></param>
        /// <param name="layer"></param>
        /// <param name="normalizedStartTime"></param>
        /// <param name="normalizedTransition"></param>
        /// <param name="normalizedExitTime"></param>
        /// <returns></returns>
        public static async UniTask PlayOnceInFixedTime(this Animator animator ,string stateName, float playTime, int layer = 0, float normalizedStartTime = 0, float normalizedTransition = 0.15f, float normalizedExitTime = 0.8f)
        {
            animator.CrossFade(stateName, normalizedTransition, layer, normalizedStartTime);
        
            var currentClip = animator.GetCurrentAnimatorClipInfo(layer)[0].clip;
            var transitionDuration = currentClip.length * normalizedExitTime;
        
            await UniTask.Delay(TimeSpan.FromSeconds(transitionDuration), DelayType.DeltaTime);
        }
    }
}
