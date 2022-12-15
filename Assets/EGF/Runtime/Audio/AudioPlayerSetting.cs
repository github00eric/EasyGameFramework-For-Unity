using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace EGF.Runtime
{
    [CreateAssetMenu(menuName = MenuPath,fileName = "AudioPlayer Setting")]
    public class AudioPlayerSetting : ScriptableObject
    {
        internal const string MenuPath = "EGF Item/Setting/AudioPlayerSetting";
        
        public AudioMixer outputAudioMixer;
        [Header("混音组")]
        public List<AudioMixerGroup> audioMixerGroups;
        [Header("混音设置快照")]
        public List<AudioMixerSnapshot> audioMixerSnapshots;
        [Header("参数设置")]
        public List<string> volumeParameters;
    }
}
