using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fotosession : MonoBehaviour {

    private bool amStart;
    private WebCamTexture front;
    private Texture texture;
    public RawImage raw;


    // Use this for initialization
    private void Start () {
        texture = raw.texture;
        WebCamDevice[] devi = WebCamTexture.devices;

        if (devi.Length == 0)
        {
            Debug.Log("No cam dect");
            amStart = false;
            return;

        }
        for(int i=0; i< devi.Length; i++)
        {
            if (devi[i].isFrontFacing)
            {
                front = new WebCamTexture(devi[i].name, 200, 200);

            }
        }
        if (front == null) {
            Debug.Log("no fronti");
            return;
        }
        front.Play();
        raw.texture = front;
        amStart = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (!amStart)
            return;

	}
}
