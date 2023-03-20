using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace EGF.Runtime
{
    // PlayOnce 直接过渡到指定动画状态播放一次
    // WaitForSeconds 指定时间，等待几秒
    // TODO: PlayOnceInFixedTime 直接过渡到指定动画状态播放一次，且播放持续时间将强制更改为指定时间
    // TODO: 切换播放状态
    // TODO: 整体播放速度设置（要考虑功能2对播放速度的影响）
    public static class AnimatorExtension
    {
        // （实测过渡时间不是很精确，且动画相关操作要在 FixedUpdate 中进行）
        // CrossFade 是按照动画的自身时间进行混合。如果动画10秒，混合持续时间0.2，会在大概2秒后混合完成
        // CrossFadeInFixedTime 是按照实际时间进行混合。如果动画10秒，混合持续时间0.2，会在0.2秒后混合完成

        private static readonly int StatusId = Animator.StringToHash("stateId");
        
        private static async UniTask PlayOnceInternal(this Animator animator ,string stateName, CancellationToken token,
            int layer = 0, float normalizedStartTime = 0, float normalizedTransition = 0.25f, float normalizedExitTime = 0.75f)
        {
            if(token.IsCancellationRequested) return;           // 已经取消了就不播放
            var currentState = animator.GetCurrentAnimatorStateInfo(layer);
            
            animator.CrossFade(stateName, normalizedTransition, layer, normalizedStartTime);

            if (currentState.IsName(stateName))
            {
                bool WaitChange2SelfComplete()
                {
                    if (!animator) return true;
                    return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < currentState.normalizedTime;
                }
                await UniTask.WaitUntil(WaitChange2SelfComplete, PlayerLoopTiming.FixedUpdate, token);
            }
            else
            {
                bool WaitChangeComplete()
                {
                    if (!animator) return true;
                    return animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName) && animator.GetCurrentAnimatorClipInfoCount(layer) > 0;
                }
                await UniTask.WaitUntil(WaitChangeComplete, PlayerLoopTiming.FixedUpdate, token);
            }

            bool WaitExit()
            {
                if (!animator) return true;
                return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime > normalizedExitTime;
            }
            await UniTask.WaitUntil(WaitExit,PlayerLoopTiming.FixedUpdate, token);
        }

        /// <summary>
        /// 直接过渡到指定动画状态播放一次，直接用 Hash 效率更高，这里必须是 fullPathHash（例如 "Base Layer.run"）
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateHashName">动画状态机Hash</param>
        /// <param name="token"></param>
        /// <param name="layer">状态机所在层</param>
        /// <param name="normalizedStartTime">开始时间(归一化)</param>
        /// <param name="normalizedTransition">过渡耗时(归一化)</param>
        /// <param name="normalizedExitTime">退出时间(归一化)</param>
        /// <returns></returns>
        private static async UniTask PlayOnceInternal(this Animator animator ,int stateHashName, CancellationToken token,
            int layer = 0, float normalizedStartTime = 0, float normalizedTransition = 0.15f, float normalizedExitTime = 0.8f)
        {
            if(token.IsCancellationRequested) return;           // 已经取消了就不播放
            var currentState = animator.GetCurrentAnimatorStateInfo(layer);
            
            if(!animator.isActiveAndEnabled) return;
            
            animator.CrossFade(stateHashName, normalizedTransition, layer, normalizedStartTime);

            if (currentState.fullPathHash == stateHashName)
            {
                bool WaitChange2SelfComplete()
                {
                    if (!animator) return true;
                    return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < currentState.normalizedTime;
                }
                await UniTask.WaitUntil(WaitChange2SelfComplete, PlayerLoopTiming.FixedUpdate, token);
            }
            else
            {
                bool WaitChangeComplete()
                {
                    if (!animator) return true;
                    return animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == stateHashName && animator.GetCurrentAnimatorClipInfoCount(layer) > 0;
                }
                await UniTask.WaitUntil(WaitChangeComplete, PlayerLoopTiming.FixedUpdate, token);
            }

            bool WaitExit()
            {
                if (!animator) return true;
                return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime > normalizedExitTime;
            }
            await UniTask.WaitUntil(WaitExit,PlayerLoopTiming.FixedUpdate, token);
        }

        #region API

        /// <summary>
        /// 直接过渡到指定动画状态播放一次
        /// </summary>
        public static async UniTask PlayOnce(this Animator animator, 
            string stateName, 
            int layer = 0, 
            float normalizedStartTime = 0, 
            float normalizedTransition = 0.25f,
            float normalizedExitTime = 0.75f)
        {
            await PlayOnceInternal(animator, stateName, default(CancellationToken), layer, normalizedStartTime, normalizedTransition, normalizedExitTime);
        }

        /// <summary>
        /// 直接过渡到指定动画状态播放一次，直接用 Hash 效率更高，这里必须是 fullPathHash（例如 "Base Layer.run"）
        /// </summary>
        public static async UniTask PlayOnce(this Animator animator, 
            int stateHashName, 
            int layer = 0, 
            float normalizedStartTime = 0, 
            float normalizedTransition = 0.15f,
            float normalizedExitTime = 0.8f)
        {
            await PlayOnceInternal(animator, stateHashName, default(CancellationToken), layer, normalizedStartTime, normalizedTransition, normalizedExitTime);
        }

        /// <summary>
        /// 直接过渡到指定动画状态播放一次
        /// </summary>
        public static async UniTask PlayOnce(this Animator animator, 
            string stateName, 
            CancellationToken token,
            int layer = 0, 
            float normalizedStartTime = 0, 
            float normalizedTransition = 0.25f,
            float normalizedExitTime = 0.75f)
        {
            await PlayOnceInternal(animator, stateName, token, layer, normalizedStartTime, normalizedTransition, normalizedExitTime);
        }

        /// <summary>
        /// 直接过渡到指定动画状态播放一次，直接用 Hash 效率更高，这里必须是 fullPathHash（例如 "Base Layer.run"）
        /// </summary>
        public static async UniTask PlayOnce(this Animator animator, 
            int stateHashName, 
            CancellationToken token,
            int layer = 0, 
            float normalizedStartTime = 0, 
            float normalizedTransition = 0.15f,
            float normalizedExitTime = 0.8f)
        {
            await PlayOnceInternal(animator, stateHashName, token, layer, normalizedStartTime, normalizedTransition, normalizedExitTime);
        }
        
        public static async UniTask WaitForSeconds(this Animator animator, double waitTime, DelayType delayType = DelayType.DeltaTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), delayType, PlayerLoopTiming.FixedUpdate);
        }
        
        /// <summary>
        /// 设置 “statusId”，切换动画状态
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateId"></param>
        public static void PlayState(this Animator animator, int stateId)
        {
            animator.SetInteger(StatusId, stateId);
        }

        #endregion

        #region 废弃功能 | Abandon Function

        /*
         * 需要运行中动态改动动画播放速度的功能不建议直接通用化，不便于管理
         * Unity中设置好特定参数更容易管理：https://developer.unity.cn/ask/question/623eeef8edbc2a001d86c925
         * 
        /// <summary>
        /// UNDO：直接过渡到指定动画状态播放一次，且播放持续时间将强制更改为指定时间
        /// </summary>
        public static async UniTask PlayOnceInFixedTime(this Animator animator ,string stateName, float playTime, int layer = 0, float normalizedStartTime = 0, float normalizedTransition = 0.15f, float normalizedExitTime = 0.8f)
        {
            // ......
        }
        */

        #endregion
    }
}
