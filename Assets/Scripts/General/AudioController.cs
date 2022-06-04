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

        sound.source.clip = soundClip.clip;
        sound.source.volume = soundClip.volume * (sound.isSFX ? volumeMultiplierSFX : volumeMultiplierBGM);

        sound.source.Play();
    }

    public void MuteUnmuteBgm()
    {
        bgm = !bgm;
        bgmButton.GetComponent<Image>().sprite = bgmSprites[bgm ? 1 : 0];
        volumeMultiplierBGM = bgm ? 1 : 0;
    }

    public void MuteUnmuteSfx()
    {
        sfx = !sfx;
        sfxButton.GetComponent<Image>().sprite = sfxSprites[sfx ? 1 : 0];
        volumeMultiplierSFX = sfx ? 1 : 0;
    }

    public void Click()
    {
        AudioController.instance.Play("Sfx", "Click");
    }
}

[Serializable]
public class Sound
{
    public string name;
    public Clip[] clips;
    public bool isSFX;

    [HideInInspector] public AudioSource source;
}

[Serializable]
public class Clip
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume;
}
