using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEditor;
public class TinyAudioMenuItem : MonoBehaviour
{
    
    [MenuItem("Custom Tools/Create New Tiny Audio Manager with 1 Source")]
    private static void CreateNewTinyAudioManager()
    {
        GameObject newTAMInstance;
        newTAMInstance = new GameObject("TinyAudioManager");
        newTAMInstance.AddComponent<TinyAudioManager>();
        newTAMInstance.AddComponent<AudioSource>();
        //
        AudioSource[] sources = newTAMInstance.GetComponents<AudioSource>();
        
    }

}