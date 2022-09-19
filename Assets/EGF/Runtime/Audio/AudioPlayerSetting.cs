using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace EGF.Runtime
{
    [CreateAssetMenu(menuName = MenuPath,fileName = "AudioPlayer Setting")]
    public class AudioPlayerSetting : ScriptableObject
    {
        internal const string MenuPath = "EGF Item/Setting/AudioPlayerSetting";
        
        public AudioMixer outputAudioMixer;
        [Title("混音组")]
        public List<AudioMixerGroup> audioMixerGroups;
        [Title("混音设置快照")]
        public List<AudioMixerSnapshot> audioMixerSnapshots;
        [Title("参数设置")]
        public List<string> volumeParameters;
    }
}
