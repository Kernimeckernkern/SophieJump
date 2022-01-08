using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour {
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject character;
    [SerializeField]
    private GameObject dien;
    [SerializeField]
    private GameObject options;
    [SerializeField]
    private GameObject optionBut;
    [SerializeField]
    private Sprite[] modes;
    private CameraScript cam;
    private Player pla;
    private bool opMode = false;
    public bool DieText { get; set; }
    // Use this for initialization
    private void Start() { 
    
        DieText = true;
        cam = gameObject.GetComponent<CameraScript>();
        pla = player.gameObject.GetComponent<Player>();
    }
    public void Begin()
    {
        cam.StartSound();

        cam.NewGame();
        player.SendMessage("SetLife", true);
    }
    public void Dead()
    {
        start.SetActive(true);
        DieText = false;
        dien.SetActive(false);
    }
    public void Ende()
    {
        Application.Quit();
    }
    public void Character()
    {
        cam.ActivateCharacters();
        start.SetActive(false);       
    }
    public void Options()
    {
        if (opMode)
        {
            pla.Freeze(false);
            options.SetActive(false);
            optionBut.GetComponent<Image>().sprite = modes[0];
            opMode = false;
        }
        else
        {
            pla.Freeze(true);
            options.SetActive(true);
            optionBut.GetComponent<Image>().sprite = modes[1];
            opMode = true;
        }
    }
}
