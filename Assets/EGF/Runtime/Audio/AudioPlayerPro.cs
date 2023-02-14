/*
 * 声音播放模块Pro
 * 需要和 Unity 的 AudioMixer 结合使用，以获得更强的音频效果
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace EGF.Runtime
{
    /// <summary>
    /// 声音播放模块Pro，需要和 Unity 的 AudioMixer 结合使用，以获得更强的音频效果
    /// <para>TODO: 暂未完成制作</para>
    /// </summary>
    public class AudioPlayerPro : MonoSingleton<AudioPlayerPro>, IAudioPlayer,IAudioVolumeSetter
    {
        private IObjectPool<AudioSource> audioPool;
        private IObjectPoolOptimizer optimizer;
        public AudioPlayerSetting setting;
        
        [SerializeField] private AudioSource bgmAudioSource;
        [SerializeField] private AudioMixerSnapshot currentActiveSnapshot;
        // 所有取出对象池的音频源缓存池
        [SerializeField] private List<AudioSource> activeAudioSources;
        // 每次检查，将要归还对象池的音源列表
        private List<AudioSource> waitForReleaseAudios;

        protected override void Initialization()
        {
            if(!MakeSureSettingIsValid()) return;
            
            audioPool = new BaseObjectPool<AudioSource>(CreateNewAudio,OnGetAudio,OnReleaseAudio,OnDestroyAudio,true,10,20);
            optimizer = new ObjectPoolOptimizer<AudioSource>(audioPool);
            activeAudioSources = new List<AudioSource>();
            waitForReleaseAudios = new List<AudioSource>();
            
            EgfEntry.RegisterModule<IAudioPlayer>(this);
            EgfEntry.RegisterModule<IAudioVolumeSetter>(this);
        }

        protected override void Release()
        {
            EgfEntry.UnRegisterModule<IAudioPlayer>();
            EgfEntry.UnRegisterModule<IAudioVolumeSetter>();
        }

        private void Update()
        {
            optimizer.Update();
        }

        #region audioPool创建

        private AudioSource CreateNewAudio()
        {
            AudioSource standardResult = new GameObject("AudioPlayer",typeof(AudioSource)).GetComponent<AudioSource>();
            standardResult.transform.SetParent(this.transform);
            standardResult.gameObject.SetActive(false);
            return standardResult;
        }

        private void OnGetAudio(AudioSource audioBehaviour)
        {
            CheckAndCleanActiveAudioSources();
            audioBehaviour.gameObject.SetActive(true);
            activeAudioSources.Add(audioBehaviour);
        }
        
        private void OnReleaseAudio(AudioSource audioBehaviour)
        {
            audioBehaviour.spatialBlend = 0;
            audioBehaviour.outputAudioMixerGroup = null;
            audioBehaviour.loop = false;
            audioBehaviour.transform.SetParent(transform);
            audioBehaviour.gameObject.SetActive(false);
        }
        
        private void OnDestroyAudio(AudioSource audioBehaviour)
        {
            Destroy(audioBehaviour.gameObject);
        }

        #endregion

        #region IAudioPlayer

        public void PlayBackgroundMusic(AudioClip sound, int audioGroupId)
        {
            if (!bgmAudioSource)
            {
                bgmAudioSource = audioPool.Get();
                bgmAudioSource.loop = true;
            }
            if (audioGroupId >= 0 && audioGroupId < setting.audioMixerGroups.Count)
            {
                bgmAudioSource.outputAudioMixerGroup = setting.audioMixerGroups[audioGroupId];
            }

            bgmAudioSource.clip = sound;
            bgmAudioSource.Play();
        }

        public void StopBackgroundMusic()
        {
            if (!bgmAudioSource) return;
            
            bgmAudioSource.Stop();
            bgmAudioSource = null;
        }
        
        public void Play2DSound(AudioClip sound,int audioGroupId)
        {
            AudioSource audioSource = audioPool.Get();
            if (audioGroupId >= 0 && audioGroupId < setting.audioMixerGroups.Count)
            {
                audioSource.outputAudioMixerGroup = setting.audioMixerGroups[audioGroupId];
            }
            
            audioSource.clip = sound;
            audioSource.Play();
        }

        public void Play3DSound(AudioClip sound, Transform position,int audioGroupId)
        {
            AudioSource audioSource = audioPool.Get();
            if (audioGroupId >= 0 && audioGroupId < setting.audioMixerGroups.Count)
            {
                audioSource.outputAudioMixerGroup = setting.audioMixerGroups[audioGroupId];
            }
            
            audioSource.spatialBlend = 1;
            Transform transformTemp = audioSource.transform;
            transformTemp.SetParent(position);
            transformTemp.position = position.position;
            audioSource.clip = sound;
            audioSource.Play();
        }

        public void TransitionToSnapshots(int targetSnapshotsId,float targetWeight,float reachTime)
        {
            if (targetSnapshotsId < 0 || targetSnapshotsId >= setting.audioMixerSnapshots.Count)
            {
                Logcat.Debug(this,"目标音效快照编号越界。");
                return;
            }
            
            var targetSnapshot = setting.audioMixerSnapshots[targetSnapshotsId];
            AudioMixerSnapshot[] transition = new AudioMixerSnapshot[] {currentActiveSnapshot, targetSnapshot};
            float[] targetWeights = new float[] {1 - targetWeight, targetWeight};
            setting.outputAudioMixer.TransitionToSnapshots(transition,targetWeights,reachTime);
            currentActiveSnapshot = targetSnapshot;
        }

        public void StopAllSound()
        {
            foreach (var audioSource in activeAudioSources)
            {
                audioSource.Stop();
            }
            bgmAudioSource = null;
            
            CheckAndCleanActiveAudioSources();
        }

        #endregion

        

        #region IAudioVolumeSetter

        public void SetMainVolume(float volume)
        {
            if (setting.volumeParameters.Count < 1)
                return;
            
            float setVolume = volume * 75 - 60;
            string volumeParameter = setting.volumeParameters[0];
            setting.outputAudioMixer.SetFloat(volumeParameter, setVolume);
        }

        public void SetVolume(int audioGroupId, float volume)
        {
            if (setting.volumeParameters.Count < 1)
                return;
            
            float setVolume = volume * 75 - 60;
            bool condition = audioGroupId < setting.volumeParameters.Count && audioGroupId >= 0;
            if (condition)
            {
                setting.outputAudioMixer.SetFloat(setting.volumeParameters[audioGroupId], setVolume);
            }
            else
            {
                Logcat.Debug(this,"指定的音量参数编号越界，将默认更改主音量参数。");
                setting.outputAudioMixer.SetFloat(setting.volumeParameters[0], setVolume);
            }
        }

        #endregion
        
        /// <summary>
        /// 确保设置文件有效，无效会返回false
        /// </summary>
        private bool MakeSureSettingIsValid()
        {
            if (!setting)
            {
                Logcat.Warning(this,$"缺少必要的设置文件，请project窗口右键 Create/{AudioPlayerSetting.MenuPath} 创建");
                return false;
            }

            if (!setting.outputAudioMixer || 
                setting.audioMixerGroups.Count <= 0 ||
                setting.audioMixerSnapshots.Count <= 0)
            {
                Logcat.Warning(this,"设置文件需要设置 输出混音器，至少1个混音组，至少1个音频快照");
                return false;
            }
            currentActiveSnapshot = setting.audioMixerSnapshots[0];
            
            // if (setting.volumeParameters.Count < 1)
            // {
            //     Logcat.Warning(this,"注意，未设置可用音量参数，将无法设置音量。");
            // }
            return true;
        }
        
        /// <summary>
        /// 检查和清理已经不再播放的音源
        /// </summary>
        private void CheckAndCleanActiveAudioSources()
        {
            foreach (var audioSource in activeAudioSources)
            {
                if (!audioSource.isPlaying) // 已经没有播放了
                {
                    waitForReleaseAudios.Add(audioSource);
                }
            }

            foreach (var temp in waitForReleaseAudios)
            {
                audioPool.Release(temp);
                activeAudioSources.Remove(temp);
            }
            waitForReleaseAudios.Clear();
        }
    }
}
