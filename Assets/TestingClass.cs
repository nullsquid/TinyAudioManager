using UnityEngine;
using System.Collections;

public class TestingClass : MonoBehaviour {
    TinyAudioManager tam;

    public AudioClip[] tracks;
    public float fadeTime = 1.0f;

    private int currentTrack = 0;
	// Use this for initialization
	void Start () {
        tam = FindObjectOfType<TinyAudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            tam.ChangeBackgroundAudio("Division");
        }
        else if (Input.GetMouseButton(1))
        {
            tam.ChangeBackgroundAudio("Tech Live");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentTrack++;
            if(currentTrack >= tracks.Length)
            {
                currentTrack = 0;
            }
            TinyAudioManager.CrossfadeBackground(tracks[currentTrack], fadeTime);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            TinyAudioManager.PanAudio(-.7f, 3.0f);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TinyAudioManager.CrossfadeBackground("Division", 1f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TinyAudioManager.CrossfadeBackground("Tech Live");
        }
	}
}
