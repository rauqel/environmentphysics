using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyControl : MonoBehaviour
{
    public Image energyBar;
    public Text speedDisplay;

    public PlayerMovement moveScript;

    bool findFloor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speedDisplay.text = moveScript.moveSpeed.ToString();


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "SnowAir")
        {
            moveScript.moveSpeed = 8;
            Debug.Log("snow");

        }
        if (collision.gameObject.tag == "GrassAir")
        {
            moveScript.moveSpeed = 10;
            Debug.Log("grass");

        }
        if (collision.gameObject.tag == "DesertAir")
        {
            moveScript.moveSpeed = 6;
            Debug.Log("sand");
        }
    }
}
