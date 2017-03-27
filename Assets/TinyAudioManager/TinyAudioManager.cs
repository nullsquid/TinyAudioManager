using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
//must be attached to a gameobject with an audioclip
public class TinyAudioManager : MonoBehaviour
{
    public static TinyAudioManager instance;
    public Dictionary<string, AudioClip> sounds;
    public Dictionary<string, AudioClip> backgrounds;
    public AudioSource[] audiosources;
    public AudioMixerGroup master;
    AudioClip[] rawSounds;
    AudioClip[] rawBkg;
    //GetComponents to get multiple audio sources
    //
    void Awake()
    {
        instance = this;
        //
        if(instance.GetComponents<AudioSource>().Length > 0)
        {
            instance.GetComponent<AudioSource>().outputAudioMixerGroup = master;
        }
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
    public static void CrossfadeBackground(string newTrackName, float fadeTime = 1.0f)
    {
        instance.StopAllCoroutines();

        AudioClip newTrack = new AudioClip();
        if(instance.GetComponents<AudioSource>().Length > 1)
        {
            Destroy(instance.GetComponent<AudioSource>());
        }

        
        foreach(KeyValuePair<string, AudioClip> clip in instance.backgrounds)
        {
            if (instance.GetComponent<AudioSource>().name != newTrackName)
            {
                if (clip.Key == newTrackName)
                {
                    newTrack = clip.Value;
                }
            }
            
        }
        if (instance.GetComponent<AudioSource>().clip != null)
        {
            if (instance.GetComponent<AudioSource>().clip.name != newTrack.name)
            {
                AudioSource newAudioSource = instance.gameObject.AddComponent<AudioSource>();
                if (instance.master != null)
                {
                    newAudioSource.outputAudioMixerGroup = instance.master;
                }
                newAudioSource.volume = 0.0f;
                newAudioSource.clip = newTrack;
                newAudioSource.Play();
                instance.StartCoroutine(instance.Crossfade(newAudioSource, fadeTime));

            }
            else
            {
                return;
            }
        }
        else
        {
            AudioSource newAudioSource = instance.gameObject.AddComponent<AudioSource>();
            if (instance.master != null)
            {
                newAudioSource.outputAudioMixerGroup = instance.master;
            }
            newAudioSource.volume = 0.0f;
            newAudioSource.clip = newTrack;
            newAudioSource.Play();
            instance.StartCoroutine(instance.Crossfade(newAudioSource, fadeTime));
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
        if (instance.master != null)
        {
            newAudioSource.outputAudioMixerGroup = instance.master;
        }

        newAudioSource.volume = 0.0f;
        //
        //this should retrieve from the sounds/bkgs Dictionary(s) in the "real" version
        newAudioSource.clip = newTrack;
        

        newAudioSource.Play();

        instance.StartCoroutine(instance.Crossfade(newAudioSource, fadeTime));
    }

    public static void PanAudio(float panStrength, float panTime)
    {
        instance.StopAllCoroutines();

        AudioSource source = instance.gameObject.GetComponent<AudioSource>();

        instance.StartCoroutine(instance.ChannelPan(source, panStrength, panTime));
    }

    public static void CenterAudio(float panTime)
    {
        instance.StopAllCoroutines();

        AudioSource source = instance.gameObject.GetComponent<AudioSource>();

        instance.StartCoroutine(instance.PanToCenter(source, panTime));
    }
    
    public static void ChangeVolume(float volAmount, float changeTime = 0.2f)
    {

    }
    public static void ResetVolume(float changeTime = 0.2f)
    {

    }
    public static void PlaySound(string clipName)
    {
        if(instance.GetComponents<AudioSource>().Length > 1)
        {
            Destroy(instance.GetComponents<AudioSource>()[1]);
            AudioSource newAudioSource = instance.gameObject.AddComponent<AudioSource>();
            newAudioSource.Stop();
            if (instance.sounds.ContainsKey(clipName))
            {
                foreach (KeyValuePair<string, AudioClip> clip in instance.sounds)
                {
                    if (clip.Key == clipName)
                    {

                        newAudioSource.clip = clip.Value;

                    }
                }
            }
            else
            {
                Debug.LogWarning("No clip with name " + clipName + " found");
            }
            newAudioSource.Play();
        }
        if (instance.GetComponents<AudioSource>().Length == 1)
        {

            AudioSource newAudioSource = instance.gameObject.AddComponent<AudioSource>();
            newAudioSource.Stop();
            if (instance.sounds.ContainsKey(clipName))
            {
                foreach (KeyValuePair<string, AudioClip> clip in instance.sounds)
                {
                    if (clip.Key == clipName)
                    {

                        newAudioSource.clip = clip.Value;

                    }
                }
            }
            else
            {
                Debug.LogWarning("No clip with name " + clipName + " found");
            }
            newAudioSource.Play();
            
        }
        
    }

    /*
    public static void PlaySound(string clipName)
    {
        if (instance.sounds.ContainsKey(clipName))
        {
            foreach (KeyValuePair<string, AudioClip> sound in instance.sounds)
            {

                if (clipName == sound.Key)
                {
                    instance.gameObject.GetComponent<AudioSource>().clip = sound.Value;
                    instance.gameObject.GetComponent<AudioSource>().Play();
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("AudioClip not found");
        }

    }*/

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

    IEnumerator ChannelPan(AudioSource source, float panStrength, float panTime = 1.0f)
    {
        float t = 0.0f;

        while(t < panTime)
        {
            source.panStereo = Mathf.Lerp(0.0f, panStrength, t / panTime);

            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator PanToCenter(AudioSource source, float panTime)
    {
        float t = 0.0f;
        float startPan = gameObject.GetComponent<AudioSource>().panStereo;
        while (t < panTime)
        {
            source.panStereo = Mathf.Lerp(startPan, 0.0f, t / panTime);

            t += Time.deltaTime;
            yield return null;
        }
    }
}
