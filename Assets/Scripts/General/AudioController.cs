using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private Sprite[] sfxSprites;
    [SerializeField] private Sprite[] bgmSprites;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private GameObject audioButtons;

    public static AudioController instance;
    public Sound[] sounds;

    private float volumeMultiplierBGM = 1;
    private float volumeMultiplierSFX = 1;
    private bool sfx = true;
    private bool bgm = true;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(audioButtons);

            foreach (Sound sound in sounds)
                sound.source = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Play(string soundName, string clipName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);

        if (sound == null)
        {
            Debug.LogError("Cannot find Audio Source.");

            return;
        }

        Clip soundClip = Array.Find(sound.clips, clip => clip.name == clipName);

        if (soundClip == null)
        {
            Debug.LogError("Cannot find Audio Clip.");

            return;
        }

        if (sound.source.clip != null && sound.source.clip.name != soundClip.name)
        {
            sound.source.Stop();
        }

        sound.currentClip = soundClip;
        sound.source.clip = soundClip.clip;
        sound.source.volume = soundClip.volume * (sound.isSFX ? volumeMultiplierSFX : volumeMultiplierBGM);

        if (!sound.source.isPlaying)
            sound.source.Play();
    }

    public void MuteUnmuteBgm()
    {
        Click();
        bgm = !bgm;
        bgmButton.GetComponent<Image>().sprite = bgmSprites[bgm ? 1 : 0];
        volumeMultiplierBGM = bgm ? 1 : 0;

        foreach(Sound sound in sounds)
        {
            if(!sound.isSFX)
            {
                if (bgm && sound.source.clip != null)
                    sound.source.volume = sound.currentClip.volume;
                else
                    sound.source.volume = bgm ? 1 : 0;
            }
        }
    }

    public void MuteUnmuteSfx()
    {
        Click();
        sfx = !sfx;
        sfxButton.GetComponent<Image>().sprite = sfxSprites[sfx ? 1 : 0];
        volumeMultiplierSFX = sfx ? 1 : 0;

        foreach (Sound sound in sounds)
        {
            if (sound.isSFX)
            {
                if (bgm && sound.source.clip != null)
                    sound.source.volume = sound.currentClip.volume;
                else
                    sound.source.volume = bgm ? 1 : 0;
            }
        }
    }

    public void Click()
    {
        Play("Sfx", "Click");
    }
}

[Serializable]
public class Sound
{
    public string name;
    public Clip[] clips;
    public bool isSFX;

    [HideInInspector] public AudioSource source;
    [HideInInspector] public Clip currentClip;
}

[Serializable]
public class Clip
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume;
}
