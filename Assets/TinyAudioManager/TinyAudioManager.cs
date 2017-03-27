using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//must be attached to a gameobject with an audioclip
public class TinyAudioManager : MonoBehaviour
{
    public static TinyAudioManager instance;
    public Dictionary<string, AudioClip> sounds;
    public Dictionary<string, AudioClip> backgrounds;
    public AudioSource[] audiosources;
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
    public void CrossfadeBackground(string newClipName, AudioSource curSource, AudioSource targetSource, float fadetime = 1.0f)
    {
        StopAllCoroutines();
        AudioClip newAudioClip = null;
        
        foreach (KeyValuePair<string, AudioClip> track in backgrounds) {
            if (track.Key == newClipName)
            {
                //instance.audiosources[1].clip = track.Value;
                newAudioClip = track.Value;
            }
        }
        StartCoroutine(Crossfade(fadetime, curSource, targetSource));
        /*
        if (instance.audiosources[0].isPlaying)
        {
            instance.audiosources[1].clip = newAudioClip;
            instance.audiosources[1].volume = 0.0f;
            instance.StartCoroutine(instance.Crossfade(instance.audiosources[1], fadetime, audiosources[0], audiosources[1]));
        }
        else if (instance.audiosources[1].isPlaying)
        {
            instance.audiosources[0].clip = newAudioClip;
            instance.audiosources[0].volume = 0.0f;
            instance.StartCoroutine(instance.Crossfade(instance.audiosources[0], fadetime, audiosources[1], audiosources[0]));
        }
        */

        

    }
    IEnumerator Crossfade(float fadeTime, AudioSource start, AudioSource end)
    {
        float t = 0.0f;

        while(t < fadeTime)
        {
            end.volume = Mathf.Lerp(0.0f, 1.0f, t/fadeTime);
            start.volume = 1.0f - start.volume;
            t += Time.deltaTime;
            yield return null;
        }
        end.volume = 1.0f;
        
        
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
