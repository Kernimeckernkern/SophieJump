using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fotosession : MonoBehaviour {

    private bool amStart;
    private WebCamTexture front;
    private RawImage raw;
    private Quaternion baseRotation;
    private Vector2 hw;
    private bool eMode=false;


    // Use this for initialization
    private void Start () {
        raw = GetComponent<RawImage>();
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
                
                hw = new Vector2(raw.rectTransform.rect.height, raw.rectTransform.rect.width);
                front = new WebCamTexture(devi[i].name, 200, 200);
                baseRotation = raw.transform.rotation;


            }
        }
        if (front == null) {
            Debug.Log("no fronti");
            return;
        }
       
        front.Play();
#if UNITY_EDITOR
    eMode = true;
#endif
#if UNITY_ANDROID //turns camera
            if(!eMode){
                                raw.transform.rotation = baseRotation * Quaternion.AngleAxis(front.videoRotationAngle, Vector3.back);
                                raw.rectTransform.sizeDelta = hw;}
#endif
        raw.texture = front;
        amStart = true;
        //raw.transform.rotation = baseRotation * Quaternion.AngleAxis(front.videoRotationAngle, Vector3.back);
    }
	
}
