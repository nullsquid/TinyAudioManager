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

    public static void CrossfadeBackground(AudioClip newTrack, float fadeTime = 1.0f)
    {
        instance.StopAllCoroutines();

        if(instance.GetComponents<AudioSource>().Length > 1)
        {
            Destroy(instance.GetComponent<AudioSource>());
        }

        AudioSource newAudioSource = instance.gameObject.AddComponent<AudioSource>();

        newAudioSource.volume = 0.0f;
        //
        //this should retrieve from the sounds/bkgs Dictionary(s) in the "real" version
        newAudioSource.clip = newTrack;

        newAudioSource.Play();

        instance.StartCoroutine(instance.Crossfade(newAudioSource, fadeTime));
    }
    
    IEnumerator Crossfade(AudioSource newSource, float fadeTime)
    {
        float t = 0.0f;
        float initVolume = gameObject.GetComponent<AudioSource>().volume;

        while (t < fadeTime)
        {
            gameObject.GetComponent<AudioSource>().volume = Mathf.Lerp(initVolume, 0.0f, t / fadeTime);
            newSource.volume = Mathf.Lerp(0.0f, 1.0f, t / fadeTime);
            
            t += Time.deltaTime;

            yield return null;
        }
        newSource.volume = 1.0f;
        Destroy(gameObject.GetComponent<AudioSource>());
        
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
