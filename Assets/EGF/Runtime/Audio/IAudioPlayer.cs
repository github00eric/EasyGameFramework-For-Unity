using UnityEngine;

namespace EGF.Runtime
{
    public interface IAudioPlayer
    {
        /// <summary>
        /// 播放背景音乐（唯一，不受空间影响，循环）
        /// </summary>
        /// <param name="sound"></param>
        void PlayBackgroundMusic(AudioClip sound, int audioGroupId = -1);
        
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        void StopBackgroundMusic();

        //void Play2DSound(AudioClip sound);

        /// <summary>
        /// 播放2D音效（多通道，不受空间影响）
        /// </summary>
        /// <param name="sound">声音片段</param>
        /// <param name="audioGroupId">指定的声音组编号</param>
        void Play2DSound(AudioClip sound, int audioGroupId = -1);
        
        //void Play3DSound(AudioClip sound, Transform position);
        
        /// <summary>
        /// 播放3D音效（多通道，跟随指定Transform位置播放）
        /// </summary>
        /// <param name="sound">声音片段</param>
        /// <param name="position">Transform位置</param>
        /// <param name="audioGroupId">指定的声音组编号</param>
        void Play3DSound(AudioClip sound, Transform position, int audioGroupId = -1);

        /// <summary>
        /// 跳转到指定 音效快照Snapshot
        /// </summary>
        /// <param name="targetSnapshotsId"></param>
        /// <param name="targetWeight"></param>
        /// <param name="reachTime"></param>
        void TransitionToSnapshots(int targetSnapshotsId, float targetWeight, float reachTime);
        
        /// <summary>
        /// 全局停止播放音效
        /// </summary>
        void StopAllSound();
    }
}
