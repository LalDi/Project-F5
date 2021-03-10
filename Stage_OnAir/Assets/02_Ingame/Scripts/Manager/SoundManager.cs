﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioClip[] clips;

    Dictionary<string, AudioClip> clipDic;
    AudioSource sfxPlayer;
    AudioSource bgmPlayer;

    float sfxVolume = 1f;
    float bgmVolume = 1f;

    new void Awake()
    {
        base.Awake();

        sfxPlayer = GetComponent<AudioSource>();
        bgmPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        clipDic = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in clips)
        {
            clipDic.Add(clip.name, clip);
        }
    }

    public void Start()
    {
        StartCoroutine(Fade("in"));
        bgmPlayer.Play();
    }

    public void PlaySound(string clipname, float volume = 1f)
    {
        if (!clipDic.ContainsKey(clipname))
        { Debug.Log("coludn't find sound"); return; }
        sfxPlayer.PlayOneShot(clipDic[clipname], volume * sfxVolume);
    }

    public GameObject LoopSound(string clipname)
    {
        if (!clipDic.ContainsKey(clipname))
        { Debug.Log("coludn't find sound"); return null; }

        GameObject loop = new GameObject("Loop");
        AudioSource source = loop.AddComponent<AudioSource>();

        source.clip = clipDic[clipname];
        source.volume = bgmVolume;
        source.loop = true;

        StartCoroutine(Fade("in"));
        source.Play();

        return loop;
    }

    public void StopBGM()
    {
        StartCoroutine(Fade("out"));
        bgmPlayer.Stop();
    }
    public void PlayBGM()
    {
        StartCoroutine(Fade("In"));
        bgmPlayer.Play();
    }

    public void SetSFX(float volume)
    {
        sfxVolume = volume;
    }

    public void SetBGM(float volume)
    {
        bgmPlayer.volume = volume;
    }

    IEnumerator Fade(string fadekind)
    {
        // BGM Base
        float curVolume = 0f;
        float volume = 1f;

        while (curVolume <= volume)
        {
            if (fadekind == "In")
                bgmPlayer.volume = curVolume;
            if (fadekind == "Out")
                bgmPlayer.volume -= curVolume;

            curVolume += Time.deltaTime;
            yield return null;
        }

        bgmPlayer.volume = 1f;
    }

}
