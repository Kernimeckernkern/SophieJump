using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Vector2 input;
    private Rigidbody2D rb;
    [SerializeField]
    private float speed=10f;
    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private GameObject die;
    [SerializeField]
    private Slider slid;
    [SerializeField]
    private Toggle toggle;
    private Vector2 boundsMin;
    private Vector2 boundsMax;
    private bool alive = true;
    private bool eMode = false;
    private float ueSpeed;
    private float velocityy;

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
        boundsMin = cam.GetComponent<Camera> ().ViewportToWorldPoint (new Vector2 (0f, 0f));
        boundsMax = cam.GetComponent<Camera> ().ViewportToWorldPoint (new Vector2(1f,0f));
        boundsMin.x += 0.3f;
        boundsMax.x -= 0.3f;
        if (PlayerPrefs.HasKey("Speed")) {
        Speed = PlayerPrefs.GetFloat("Speed");
        }
        slid.value = Speed;

    }

    // Update is called once per frame
    void FixedUpdate ()
    {
#if UNITY_EDITOR
        input = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
        eMode = true;
#endif
#if UNITY_ANDROID 
        if (!eMode && toggle.isOn)
        {

            input = new Vector2 (Input.acceleration.x, 0f);
        }
        if (!eMode && !toggle.isOn)
        {
            
            if(Input.touchCount > 0)
            {

            if (Input.GetTouch(0).position.x < Screen.width / 2)
                input = Vector2.left;
            if (Input.GetTouch(0).position.x > Screen.width / 2)
                input = Vector2.right;
            }
            else
            {
                //input = new Vector2(-input.x, 0f);
                input = new Vector2(0f, 0f);
            }
        }
#endif
        if (rb.position.x > boundsMax.x || rb.position.x < boundsMin.x)
        {
            rb.velocity = new Vector2 (0f, rb.velocity.y);
           if (rb.position.x > boundsMax.x && input.x < 0 || rb.position.x < boundsMin.x && input.x > 0)
            {
                rb.velocity = new Vector2(input.x * Speed,rb.velocity.y); 
            }
        }
        else
       {
            rb.velocity = new Vector2(input.x * Speed, rb.velocity.y);
        }

    }

    void SetLife (bool live)
    {
        alive = live;
        transform.position = new Vector2 (0,-1);
    }
    public void DieOn()
    {
        if (cam.GetComponent<ButtonEvents>().DieText && !alive)
            die.SetActive(true);
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
                die.SetActive(true);
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer ("Rocket"))
        {
            cam.SendMessage ("GoRocket");
          //  cam.SendMessage ("DestroyRocket");
        }
    }
    public void Freeze(bool mode)
    {
        if (mode)
        {
            ueSpeed = Speed;
            Speed = 0f;
            transform.position = transform.position;
            velocityy = rb.velocity.y;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            Speed = slid.value;
            PlayerPrefs.SetFloat("Speed", slid.value);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(0f,velocityy);
        }
    }

}