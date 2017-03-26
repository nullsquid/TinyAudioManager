using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//must be attached to a gameobject with an audioclip
public class TinyAudioManager : MonoBehaviour
{
    public static TinyAudioManager instance;
    public Dictionary<string, AudioClip> sounds;
    public Dictionary<string, AudioClip> backgrounds;
    AudioSource[] audiosources;
    AudioClip[] rawSounds;
    AudioClip[] rawBkg;
    //GetComponents to get multiple audio sources
    //
    void Awake()
    {
        instance = this;
        //
        sounds = new Dictionary<string, AudioClip>();
        backgrounds = new Dictionary<string, AudioClip>();
        audiosources = gameObject.GetComponents<AudioSource>();
        rawSounds = Resources.LoadAll<AudioClip>("Sounds");
        rawBkg = Resources.LoadAll<AudioClip>("Background");
        for (int i = 0; i < rawSounds.Length; i++)
        {
            sounds.Add(rawSounds[i].name, rawSounds[i]);
        }
        for (int j = 0; j < rawBkg.Length; j++)
        {
            backgrounds.Add(rawBkg[j].name, rawBkg[j]);
        }
    }

    public void ChangeBackgroundAudio(string clipName)
    {
        if (audiosources[0].clip != null)
        {
            if (clipName == audiosources[0].clip.name)
            {
                return;
            }
        }
        if (backgrounds.ContainsKey(clipName))
        {
            foreach (KeyValuePair<string, AudioClip> background in backgrounds)
            {
                
                if (clipName == background.Key)
                {
                    audiosources[0].Stop();
                    audiosources[0].clip = background.Value;
                    audiosources[0].Play();

                }
            }

        }
    }
    public static void CrossfadeBackground(string newClipName, float fadetime = 1.0f)
    {
        instance.audiosources[1].volume = 0.0f;
        foreach (KeyValuePair<string, AudioClip> track in instance.backgrounds) {
            if (track.Key == newClipName)
            {
                instance.audiosources[1].clip = track.Value;
            }
        }
               
        instance.StartCoroutine(instance.Crossfade(instance.audiosources[1], newClipName, fadetime));

    }
    IEnumerator Crossfade(AudioSource source, string clipName, float time)
    {
        yield return null;
    }
    public void PlaySound(string clipName)
    {
        if (sounds.ContainsKey(clipName))
        {
            foreach (KeyValuePair<string, AudioClip> sound in sounds)
            {

                if (clipName == sound.Key)
                {
                    gameObject.GetComponent<AudioSource>().clip = sound.Value;
                    gameObject.GetComponent<AudioSource>().Play();
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("AudioClip not found");
        }

    }
}
