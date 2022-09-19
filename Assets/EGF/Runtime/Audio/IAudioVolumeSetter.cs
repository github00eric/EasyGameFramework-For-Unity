using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// 设置音量模块接口
    /// </summary>
    public interface IAudioVolumeSetter
    {
        /// <summary>
        /// 设置总音量
        /// </summary>
        /// <param name="volume"></param>
        void SetMainVolume(float volume);
        
        /// <summary>
        /// 指定=音频组设置音量
        /// </summary>
        /// <param name="audioGroupId">指定的音频组编号</param>
        /// <param name="volume">音量大小</param>
        void SetVolume(int audioGroupId, float volume);
    }
}
