using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    public class AudioPlayer : MonoSingleton<AudioPlayer>,IAudioPlayer,IAudioVolumeSetter
    {
        [SerializeField] private AudioSource bgmAudioSource;
        
        private IObjectPool<AudioSource> audioPool;
        private IObjectPoolOptimizer optimizer;
        // 所有取出对象池的音频源缓存池
        [SerializeField] private List<AudioSource> activeAudioSources;
        // 每次检查，将要归还对象池的音源列表
        private List<AudioSource> waitForReleaseAudios;
        
        [Serializable]
        private class AudioGroupInfo
        {
            public string groupName = "Undefined Audio Group";
            [Range(0,1)] public float volume = 0.7f;
        }
        
        [Header("音频组信息")]
        [SerializeField] private List<AudioGroupInfo> audioGroups = new List<AudioGroupInfo>(){new AudioGroupInfo(){groupName = "default"}};

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
            SetVolumeInternal(bgmAudioSource, audioGroupId);
            
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
            SetVolumeInternal(audioSource, audioGroupId);
            
            audioSource.clip = sound;
            audioSource.Play();
        }

        public void Play3DSound(AudioClip sound, Transform position,int audioGroupId)
        {
            AudioSource audioSource = audioPool.Get();
            SetVolumeInternal(audioSource, audioGroupId);
            
            audioSource.spatialBlend = 1;
            Transform transformTemp = audioSource.transform;
            transformTemp.SetParent(position);
            transformTemp.position = position.position;
            audioSource.clip = sound;
            audioSource.Play();
        }

        public void TransitionToSnapshots(int targetSnapshotsId,float targetWeight,float reachTime)
        {
            Logcat.Warning(this,"简易音频播放模块无音频转场功能，请使用高级音频模块AudioPlayerPro获得更强功能。");
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
            if (volume < 0 || volume > 1)
            {
                Logcat.Warning(this,"音量设置参数超出范围，无法设置，请保持在 0 ~ 1 的范围内。");
                return;
            }
            audioGroups[0].volume = volume;
        }

        public void SetVolume(int audioGroupId, float volume)
        {
            if (volume < 0 || volume > 1)
            {
                Logcat.Warning(this,"音量设置参数超出范围，无法设置，请保持在 0 ~ 1 的范围内。");
                return;
            }

            if (audioGroupId >= 0 && audioGroupId < audioGroups.Count)
            {
                audioGroups[audioGroupId].volume = volume;
                return;
            }
            Logcat.Warning(this,$"音量组编号{audioGroupId}越界。");
        }

        #endregion
        
        /// <summary>
        /// 确保设置文件有效，无效会返回false
        /// </summary>
        private bool MakeSureSettingIsValid()
        {
            if (audioGroups.Count < 1)
            {
                Logcat.Warning(this,"至少需要设置1个音频组才可使用");
                return false;
            }
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

        private void SetVolumeInternal(AudioSource audioSource,int groupId)
        {
            bool condition = groupId > 0 && groupId < audioGroups.Count;
            if (!condition)
            {
                Logcat.Debug(this,"所选音量组编号越界，将默认 0 组播放。");
            }
            // 默认以音频组 0 为总音量输出组
            var mainVolume = condition ? audioGroups[0].volume : 1;
            var validGroupId = condition ? groupId : 0;
            
            audioSource.volume = audioGroups[validGroupId].volume * mainVolume;
        }
    }
}
