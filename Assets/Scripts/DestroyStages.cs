using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyStages : MonoBehaviour {
    private bool wasVisible = false;
    // Update is called once per frame
    void OnBecameVisible ()
    {
        wasVisible = true;
    }
    void OnBecameInvisible ()
    {
        if (wasVisible)
        {
            Destroy (gameObject);
        }
    }
 }
