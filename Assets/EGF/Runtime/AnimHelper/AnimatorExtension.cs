using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace EGF.Runtime
{
    // PlayOnce 直接过渡到指定动画状态播放一次
    // WaitForSeconds 指定时间，等待几秒
    // TODO: PlayOnceInFixedTime 直接过渡到指定动画状态播放一次，且播放持续时间将强制更改为指定时间
    // TODO: 切换播放状态
    // TODO: 整体播放速度设置（要考虑功能2对播放速度的影响）
    // HACK: 需要考虑跳转到自身状态可能引起无限等待，也许需要超时取消？
    public static class AnimatorExtension
    {
        // （实测过渡时间不是很精确，且动画相关操作要在 FixedUpdate 中进行）
        // CrossFade 是按照动画的自身时间进行混合。如果动画10秒，混合持续时间0.2，会在大概2秒后混合完成
        // CrossFadeInFixedTime 是按照实际时间进行混合。如果动画10秒，混合持续时间0.2，会在0.2秒后混合完成

        private static readonly int StatusId = Animator.StringToHash("statusId");
        
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
        public static async UniTask PlayOnce(this Animator animator ,string stateName, int layer = 0, float normalizedStartTime = 0, float normalizedTransition = 0.25f, float normalizedExitTime = 0.75f)
        {
            animator.CrossFade(stateName, normalizedTransition, layer, normalizedStartTime);

            bool WaitChangeComplete() => animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName) && animator.GetCurrentAnimatorClipInfoCount(layer) > 0;
            await UniTask.WaitUntil(WaitChangeComplete, PlayerLoopTiming.FixedUpdate);
            
            bool WaitExit() => animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < normalizedExitTime;
            await UniTask.WaitWhile(WaitExit,PlayerLoopTiming.FixedUpdate);
        }

        public static async UniTask WaitForSeconds(this Animator animator, double waitTime, DelayType delayType = DelayType.DeltaTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), delayType, PlayerLoopTiming.FixedUpdate);
        }

        public static async UniTask PlayOnceTest(this Animator animator ,string stateName, int layer = 0, float normalizedStartTime = 0, float normalizedTransition = 0.15f, float normalizedExitTime = 0.8f)
        {
            
            Debug.Log($"Play: {stateName}, normalizedTransition: {normalizedTransition}");
            
            var currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            AnimationClip currentClip = null;
            if (animator.GetCurrentAnimatorClipInfoCount(layer) > 0)
            {
                currentClip = animator.GetCurrentAnimatorClipInfo(layer)[0].clip;
                Debug.Log($"currentClipLength = {currentClip.length}");
            }
            Debug.Log($"currentState = {currentStateInfo.fullPathHash},\n" +
                      $"currentStateLength = {currentStateInfo.length}\n" +
                      $"target transition time={normalizedTransition}\n");
            
            double startTime = Time.time;
            animator.CrossFade(stateName, normalizedTransition, layer, normalizedStartTime);
            
        
            // HACK: 1
            // 等待过渡完成

            await UniTask.WaitWhile(
                (() => animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == currentStateInfo.fullPathHash),PlayerLoopTiming.FixedUpdate);

            double endTime = Time.timeSinceLevelLoadAsDouble;
            double deltaTime = endTime - startTime;

            var newState = animator.GetCurrentAnimatorStateInfo(layer);
            AnimationClip newClip = null;
            if (animator.GetCurrentAnimatorClipInfoCount(layer) > 0)
            {
                newClip = animator.GetCurrentAnimatorClipInfo(layer)[0].clip;
                Debug.Log($"new ClipLength = {newClip.length}");
                
                Debug.Log($"Waiting for change state, currentState = {animator.GetCurrentAnimatorClipInfo(layer)[0].clip.name},\n" +
                          $"new State Length = {newState.length}\n" +
                          $"Waited for {deltaTime},\n" +
                          $"normalized Wait Time 1 {deltaTime/currentStateInfo.length}\n" +
                          $"normalized Wait Time 2 {deltaTime/newState.length}\n\n");
            }
            else
            {
                Debug.Log($"No Clip, new State {newState.fullPathHash}\n" +
                          $"new State Length = {newState.length}\n" +
                          $"Waited for {deltaTime},\n" +
                          $"normalized Wait Time 1 {deltaTime/currentStateInfo.length}\n" +
                          $"normalized Wait Time 1 {deltaTime/newState.length}\n\n");
            }
            
            
            // Debug.Log($"Current: \n" +
            //           $"clip count: {animator.GetCurrentAnimatorClipInfoCount(layer)}\n" +
            //           $"clip 0 length: {animator.GetCurrentAnimatorClipInfo(layer)[0].clip.length}\n" +
            //           $"clip 0 name: {animator.GetCurrentAnimatorClipInfo(layer)[0].clip.name}\n" +
            //           $"state length: {currentStateInfo.length}\n" +
            //           $"played: {currentStateInfo.normalizedTime}");

            // 无Next

            // var delayTime = currentStateInfo.length * normalizedTransition * 0.5f;
            var delayTime = currentStateInfo.length * normalizedTransition * 0.5f;
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime), DelayType.DeltaTime);
            Debug.Log($"Delay: {delayTime}");
            
            // HACK: 2
            // currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            // Debug.Log($"Current: \n" +
            //           $"clip count: {animator.GetCurrentAnimatorClipInfoCount(layer)}\n" +
            //           $"clip 0 length: {animator.GetCurrentAnimatorClipInfo(layer)[0].clip.length}\n" +
            //           $"clip 0 name: {animator.GetCurrentAnimatorClipInfo(layer)[0].clip.name}\n" +
            //           $"state length: {currentStateInfo.length}\n" +
            //           $"played: {currentStateInfo.normalizedTime}");
            //
            // var nextStateInfo = animator.GetNextAnimatorStateInfo(layer);
            // Debug.Log($"Next: \n" +
            //           $"clip count: {animator.GetNextAnimatorClipInfoCount(layer)}\n" +
            //           $"clip 0 length: {animator.GetNextAnimatorClipInfo(layer)[0].clip.length}\n" +
            //           $"clip 0 name: {animator.GetNextAnimatorClipInfo(layer)[0].clip.name}\n" +
            //           $"state length: {nextStateInfo.length}\n" +
            //           $"played: {nextStateInfo.normalizedTime}");
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime), DelayType.DeltaTime);
            Debug.Log($"Delay: {delayTime}");
            
            
            // HACK: 3
            // currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            // Debug.Log($"Current: \n" +
            //           $"clip count: {animator.GetCurrentAnimatorClipInfoCount(layer)}\n" +
            //           $"clip 0 length: {animator.GetCurrentAnimatorClipInfo(layer)[0].clip.length}\n" +
            //           $"clip 0 name: {animator.GetCurrentAnimatorClipInfo(layer)[0].clip.name}\n" +
            //           $"state length: {currentStateInfo.length}\n" +
            //           $"played: {currentStateInfo.normalizedTime}");

            // 无Next
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
            var currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            
            animator.CrossFade(stateHashName, normalizedTransition, layer, normalizedStartTime);

            bool WaitStateChange() => animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == currentStateInfo.fullPathHash || animator.GetCurrentAnimatorClipInfoCount(layer) < 1;
            await UniTask.WaitWhile(WaitStateChange,PlayerLoopTiming.FixedUpdate);
            
            bool WaitExit() => animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < normalizedExitTime;
            await UniTask.WaitWhile(WaitExit,PlayerLoopTiming.FixedUpdate);
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
