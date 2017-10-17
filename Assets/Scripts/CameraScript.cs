using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject stage;
    [SerializeField]
    private GameObject border;
    [SerializeField]
    private GameObject borderDie;
    [SerializeField]
    private GameObject borderSpawn;
    [SerializeField]
    private GameObject highscore;
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private GameObject particles2;
    [SerializeField]
    private GameObject monster;
    private GameObject newMonster;
    [SerializeField]
    private GameObject rocket;
    private GameObject newRocket;
    private GameObject MotherSpawn;
    [SerializeField]
    private GUISkin skin;

    [SerializeField]
    private AudioClip[] audio;
    [SerializeField]
    private Sprite[] spriteS;
    [SerializeField]
    private Sprite[] spriteEl;
    [SerializeField]
    private Sprite[] spriteEm;
    private Vector2 startpos;
    private Vector2 startposSpawn;
    private Vector2 currentpos;
    private Vector2 highpos;
    private bool dead;
    private bool start;
    private bool character;
    private bool noMusic;
    private bool goRocket;
    private int cHigh;
    private int person=0;
    private float high;
    private float bTime;
    private Vector2 hBoundsMin;
    private Vector2 hBoundsMax;
    // Stage amount
    private int iStageC = 5;
    // Spawn Monster after giving amount of stages
    [SerializeField, Tooltip("Spawn Monster after giving amount of stages")]
    private int mSpawnC = 20;
    [SerializeField, Tooltip ("Spawn Rocket after giving amount of stages")]
    private int rSpawnC = 20;
    private List<GameObject> monsterInstances = new List<GameObject> ();
    private List<GameObject> stageInstances = new List<GameObject>();
    private List<GameObject> rocketInstances = new List<GameObject> ();

    // Use this for initialization
    void Start ()
    {
        hBoundsMin = GetComponent<Camera> ().ViewportToWorldPoint (new Vector2 (0f, 0f));
        hBoundsMax = GetComponent<Camera> ().ViewportToWorldPoint (new Vector2 (1f, 0f));
       // hBoundsMin.x += 0.3f;
        //hBoundsMax.x -= 0.3f;
        player.SendMessage ("SetLife", false);
        high = PlayerPrefs.GetFloat ("Highscore",0f);
        highpos = new Vector2 (0f, high);
        highscore.transform.position = highpos;
        startposSpawn = borderSpawn.transform.position;
        start = true;
        noMusic = false;
        
    }
    // Update is called once per frame
    void Update ()
    {//Camera Verschiebung
        if (player.transform.position.y >= border.transform.position.y)
        {
            currentpos.y += 0.1f;
            transform.position = Vector3.MoveTowards(transform.position,currentpos,Time.deltaTime * 5f);
        }
        // Player Tötung
      if (player.transform.position.y < borderDie.transform.position.y)
            {
            dead = true;
            player.SendMessage ("SetLife",false);
            GetComponent<AudioSource> ().Stop ();
            GetComponent<AudioSource> ().loop = false;
        }
      //Set Highscore
        if (player.transform.position.y > high)
        {
            high += 0.1f;
            highpos = new Vector2 (0f, high);
            highscore.transform.position = highpos;
            PlayerPrefs.SetFloat ("Highscore",high);
        }
        //Set CurrentSscore and Particles
        if (player.transform.position.y > cHigh)
        {
            cHigh += 1;
            if (cHigh % 100 == 0)
            {
                Instantiate (particles, new Vector2 (0f, player.transform.position.y), Quaternion.Euler (0, 0, 0));

                if (!noMusic)
                {
                    GetComponent<AudioSource>().loop = false;
                    Sound(2, 0);
                    
                }
            }
            if (cHigh % 1000 == 0)
                Instantiate (particles2, new Vector2(0f,player.transform.position.y), Quaternion.Euler (0, 0, 0));

        }
        if (!start)
        {
            if (!noMusic)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().loop = true;
                    Sound(0, 1);
                }
            }
        }
        // Instantiate new Stages
        if (player.transform.position.y > borderSpawn.transform.position.y)
        {
            Vector2 sBorder = new Vector2 (borderSpawn.transform.position.x, borderSpawn.transform.position.y);
            sBorder.y += 2.5f;
            Vector2 spawn = new Vector2 (Random.Range (hBoundsMin.x, hBoundsMax.x), sBorder.y + 5f);
            GameObject newStage = Instantiate (stage, spawn, Quaternion.Euler (0, 0, 0));
            stageInstances.Add (newStage);
            iStageC += 1;
            borderSpawn.transform.position = sBorder;
            if (iStageC % mSpawnC == 0)
            {
                Vector2 spawnM = new Vector2 (newStage.transform.position.x, newStage.transform.position.y);
                spawnM.y += 0.3f;
                newMonster = Instantiate (monster, spawnM, Quaternion.identity);
                monsterInstances.Add (newMonster);
                spawnM.x += 1f;
                newStage = Instantiate (stage, spawnM, Quaternion.Euler (0, 0, 0));
                stageInstances.Add (newStage);
            }
            if (iStageC % rSpawnC == 0)
            {
                Vector2 spawnR = new Vector2 (newStage.transform.position.x, newStage.transform.position.y);
                spawnR.y += 0.3f;
                newRocket = Instantiate (rocket, spawnR, Quaternion.identity);
                rocketInstances.Add (newRocket);
            }
        }
    }
    void FixedUpdate ()
    {
        if (goRocket)
        {
            player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0f, 5f)*5f);
            if (player.transform.position.y > newRocket.transform.position.y + 2f)
                goRocket = false;
        }
        if (!goRocket)
            SetSprite (person,0);
    }
    void SetDead (bool died)
    {
        dead = died;
    }
    void DestroyMonster ()
    {
        Destroy (newMonster);
    }
    void DestroyRocket ()
    {
        Destroy (newRocket);
    }
    void GoRocket ()
    {

        SetSprite (person,1);
        goRocket = true;
    }

    void NewGame ()
    {
        start = false;
        dead = false;
        transform.position = startpos;
        currentpos = startpos;
        borderSpawn.transform.position = startposSpawn;
        cHigh = 0;
   
        for (int a = monsterInstances.Count - 1; a >= 0; --a)
        {
            Destroy (monsterInstances[a]);
        }
        monsterInstances.Clear ();
        for (int a = rocketInstances.Count - 1; a >= 0; --a)
        {
            Destroy (rocketInstances[a]);
        }
        rocketInstances.Clear ();
        for (int i=stageInstances.Count-1;i >= 0; --i)
        {
            Destroy (stageInstances[i]);
        }
        stageInstances.Clear();
        for (float x = 0; x <= 7.5; x += 1.5f)
        {
            Vector2 spawn = new Vector2 (Random.Range (hBoundsMin.x, hBoundsMax.x),x);
            GameObject newStage = Instantiate (stage, spawn, Quaternion.Euler (0, 0, 0));
            stageInstances.Add (newStage);
        }
    }
    void Sound (int dies, ulong delay)
    {
        GetComponent<AudioSource> ().clip = audio[dies];
        GetComponent<AudioSource> ().PlayDelayed (delay);
    }
    void SetSprite (int person, int dies)
    {
        if (person==0) { player.GetComponent<SpriteRenderer>().sprite = spriteS[dies]; }
        if (person == 1) { player.GetComponent<SpriteRenderer>().sprite = spriteEl[dies]; }
        if (person == 2) { player.GetComponent<SpriteRenderer>().sprite = spriteEm[dies]; }
    }
    void OnGUI ()
    {
        GUI.skin = skin;
        GUI.Label (new Rect(20,20,50,40),cHigh.ToString(),skin.GetStyle("High"));
        if (GetComponent<AudioSource> ().isPlaying)
        {
          
                if (GUI.Button (new Rect (Screen.width - 110, 10, 100, 100), "Stop Music")) 
                {
                    GetComponent<AudioSource> ().Stop ();
                    noMusic = true;
                } 
        }
        if (!GetComponent<AudioSource> ().isPlaying)
        {
            
                if (GUI.Button (new Rect (Screen.width - 110, 10, 100, 100), "Play Music"))
                {
                    GetComponent<AudioSource> ().Play ();
                    noMusic = false;
                }
            
        }
        if (dead)
        {
            Rect rectos = new Rect (Screen.width/2-400,Screen.height/2-100,800,200);
            GUI.Label (rectos,"You Died");
            if (GUI.Button (new Rect (Screen.width / 2 - 250, Screen.height / 2 + 120, 500, 200),"New Game"))
            {
                
                start = true;
            }
        }
        if (start)
        {
            character = false;
            dead = false;
            if (GUI.Button (new Rect (Screen.width / 2 - Screen.width / 6, Screen.height / 2 - 205, Screen.width / 3, 100), "Start"))
            {
               if (!noMusic)
                     Sound (1,0);

                NewGame ();
                player.SendMessage ("SetLife", true);
            }
            if (GUI.Button (new Rect (Screen.width / 2 - Screen.width / 6, Screen.height / 2 - 85, Screen.width / 3, 100), ""))
            {
                character = true;
            }
            if (GUI.Button (new Rect (Screen.width / 2 - Screen.width / 6, Screen.height / 2 + 35, Screen.width / 3, 100), "Quit"))
            {
                Application.Quit ();
            }
        }
        if (character) {
            start = false;
            if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 6, Screen.height / 2 - 205, Screen.width / 3, 100), "Sophie"))
            {
                person = 0;
                start = true;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 6, Screen.height / 2 - 85, Screen.width / 3, 100), "Elida"))
            {
                person = 1;
                start = true;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 6, Screen.height / 2 + 35, Screen.width / 3, 100), "Emilia"))
            {
                person = 2;
                start = true;
            }
        }

    }
}
