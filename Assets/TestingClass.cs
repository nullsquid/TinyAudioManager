using UnityEngine;
using System.Collections;

public class TestingClass : MonoBehaviour {
    public TinyAudioManager tam;
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
	}
}
