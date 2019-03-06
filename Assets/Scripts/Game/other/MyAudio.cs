using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MyAudio : MonoBehaviour
{
    private static GameObject _audio;
    private static AudioSource _audio_Source;
    private static readonly Dictionary<string, AudioClip> s_AudioClips_Dictionary = new Dictionary<string, AudioClip>();
    void Awake()
    {
        //预加载所有音频
        LoadAllAudio();
        //保持本类对象只有一个
        KeepOnlyOne();
    }

    void KeepOnlyOne()
    {
        if (_audio == null)
        {
            _audio = gameObject;
            //游戏中唯一的声源组件，用来播放背景音乐
            _audio_Source = _audio.GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void LoadAllAudio()
    {
        if(!StartGame.First_In)
            return;
        AudioClip[] temp = Resources.LoadAll<AudioClip>("Sound");
        foreach (AudioClip clip in temp)
        {
            s_AudioClips_Dictionary[clip.name] = clip;
        }
    }

    public static void ChangeBGM()
    {
        try
        {
            if (SceneManager.GetActiveScene().name.Equals("UI"))
            {
                _audio_Source.clip = s_AudioClips_Dictionary[StaticParameter.s_UI_BGM];
            }
            else
            {
                _audio_Source.clip = s_AudioClips_Dictionary[StaticParameter.s_Stage_BGM];
            }
            _audio_Source.Play();
        }
        catch (Exception)
        {

            Debug.Log("无法播放音频");
        }

    }

    //音频播放方法
    public static void PlayAudio(string name, bool loop, float volume)
    {
        try
        {
            AudioClip BGM = s_AudioClips_Dictionary[name];

            _audio_Source.PlayOneShot(BGM, volume);
        }
        catch (Exception)
        {
            Debug.Log(name+"音频错误");
        }
        
    }

}
