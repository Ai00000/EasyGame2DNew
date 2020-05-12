using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FastyTools.MonoController;
using FastyTools.ResLoad;
using FastyTools.Singleton;
using UnityEngine;

/*
 * @Author:Fasty97
 * @UpdateTime:2020年5月9日11:11:40
 */

namespace FastyTools.Music
{
    /// <summary>
    /// 音效管理器
    /// </summary>
    public class MusicManager : Singleton<MusicManager>
    {
        private AudioSource bgmSource;

        private float bgmVolume = 1;

        private float soundVolume = 1;

        private float totalVolume = 1;

        private List<AudioSource> soundSource;

        /// <summary>
        /// 播放bgm
        /// </summary>
        /// <param name="clipName">clipName</param>
        public void PlayBgm(string clipName)
        {
            if (bgmSource == null)
            {
                var go = new GameObject("bgmSource");
                bgmSource = go.AddComponent<AudioSource>();
            }

            ResManager.Instance.LoadAsync<AudioClip>("music/bgm/" + clipName, (c) =>
            {
                bgmSource.clip = c;
                bgmSource.volume = bgmVolume*totalVolume;
                bgmSource.loop = true;
                bgmSource.Play();
            });
        }
    
        /// <summary>
        /// 播放bgm
        /// </summary>
        public void PlayBgm()
        {
            if (bgmSource!=null&& bgmSource.clip!=null&& !bgmSource.isPlaying)
            {
                bgmSource.Play();
            }
        }

        /// <summary>
        /// 暂停bgm
        /// </summary>
        public void PauseBgm()
        {
            if (bgmSource != null)
            {
                bgmSource.Pause();
            }
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopBgm()
        {
            if (bgmSource == null)
            {
                return;
            }

            bgmSource.Stop();
        }

        
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="sound"></param>
        public void PlaySound(string sound)
        {
            ResManager.Instance.LoadAsync<AudioClip>("music/sound/" + sound, (c) =>
            {
               
                var source = GetSoundSource();
                source.clip = c;
                source.volume = soundVolume*totalVolume;
                source.PlayOneShot(source.clip);

                MonoManager.Instance.StartCoroutine(PlaySoundCallBack(source));
            });
        }

        public void StopSound(string sound)
        {
        }


       
        
        /// <summary>
        /// 设置是否静音
        /// </summary>
        /// <param name="isMute"></param>
        public void SetMuteAll(bool isMute)
        {
            bgmSource.mute = isMute;
            foreach (var audioSource in soundSource.Where(m=>m.isPlaying==true&& m.mute!=isMute))
            {
                audioSource.mute = isMute;
            }
        }

        
        /// <summary>
        /// 设置bgm的音量0-1
        /// </summary>
        /// <param name="volume"></param>
        public void SetBgmVolume(float volume)
        {
            bgmSource.volume = volume*totalVolume;
        }
        /// <summary>
        /// 设置sound音量0-1
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundVolume(float volume)
        {
            foreach (var a in soundSource.Where(m=>m.isPlaying==true))
            {
                a.volume = totalVolume * volume;
            }
        }
        
        /// <summary>
        /// 设置总音量0-1
        /// </summary>
        /// <param name="volume"></param>
        public void SetTotalVolume(float volume)
        {
            totalVolume = volume;
        }
        
        
        //音效播放完成回调
        private IEnumerator PlaySoundCallBack(AudioSource ass)
        {
            yield return new WaitForSeconds(ass.clip.length*Time.timeScale);
            ass.clip = null;
        }
        
        private AudioSource GetSoundSource()
        {
            //第一次使用
            if (soundSource == null)
            {
                var go = new GameObject("soundSource");
                var s = go.AddComponent<AudioSource>();
                soundSource = new List<AudioSource>() {s};
                return s;
            }

            AudioSource cc = null;
            foreach (var source in soundSource)
            {
                if (source.clip == null)
                {
                    cc = source;
                    return cc;
                }
            }

            {
                var s= GameObject.Find("soundSource").AddComponent<AudioSource>();
                soundSource.Add(s);
                return s;
            }
        }
    }
}