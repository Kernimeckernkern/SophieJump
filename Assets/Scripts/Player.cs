using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private Vector2 input;
    private Rigidbody2D rb;
    [SerializeField]
    private float speed=10f;
    [SerializeField]
    private GameObject cam;
    private Vector2 boundsMin;
    private Vector2 boundsMax;
    private bool alive = true;
    private bool eMode = false;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
        boundsMin = cam.GetComponent<Camera> ().ViewportToWorldPoint (new Vector2 (0f, 0f));
        boundsMax = cam.GetComponent<Camera> ().ViewportToWorldPoint (new Vector2(1f,0f));
        boundsMin.x += 0.3f;
        boundsMax.x -= 0.3f;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
#if UNITY_EDITOR
        input = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
        eMode = true;
#endif
#if UNITY_ANDROID 
        if (!eMode)
        {

            input = new Vector2 (Input.acceleration.x, 0f);
        }
#endif
        if (rb.position.x > boundsMax.x || rb.position.x < boundsMin.x)
        {
            rb.velocity = new Vector2 (0f, rb.velocity.y);
           if (rb.position.x > boundsMax.x && input.x < 0 || rb.position.x < boundsMin.x && input.x > 0)
            {
                rb.AddForce (input * speed*2); 
            }
        }
        else
       {
            rb.AddForce (input * speed);
      }

    }

    void SetLife (bool live)
    {
        alive = live;
        transform.position = new Vector2 (0,-1);
    }
    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (alive)
        {

            if (rb.velocity.y <= 0)
                rb.AddForce (-Physics2D.gravity, ForceMode2D.Impulse);
        }
   }
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer ("Monster"))
        {
            if (rb.velocity.y < 0)
            {
                cam.SendMessage ("DestroyMonster");
            }
            else
            {
                SetLife (false);
                cam.SendMessage ("SetDead", true);
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer ("Rocket"))
        {
            cam.SendMessage ("GoRocket");
          //  cam.SendMessage ("DestroyRocket");
        }
    }
}