using UnityEngine;
using System.Collections;

public class TestingClass : MonoBehaviour {
    TinyAudioManager tam;
	// Use this for initialization
	void Start () {
        tam = FindObjectOfType<TinyAudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            tam.CrossfadeBackground("Division", tam.audiosources[0], tam.audiosources[1], 1.0f);
            //tam.ChangeBackgroundAudio("Division");
        }
        else if (Input.GetMouseButton(1))
        {
            tam.CrossfadeBackground("Tech Live", tam.audiosources[0], tam.audiosources[1], 1.0f);
            //tam.ChangeBackgroundAudio("Tech Live");
        }
	}
}
