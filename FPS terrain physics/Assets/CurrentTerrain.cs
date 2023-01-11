using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTerrain : MonoBehaviour
{
    public FirstPersonController playerScript;

    private BoxCollider floorCollider;
    private CapsuleCollider playerCollider;
   // private GameObject playerObject;

    void Start()
    {
        floorCollider = GetComponent<BoxCollider>();
        playerCollider = playerScript.GetComponentInChildren<CapsuleCollider>();

      //  playerObject = playerCollider.GetComponentInParent<GameObject>();
    }

    private void Update()
    { 
        Debug.Log(playerCollider);
      //  Debug.Log(playerObject.name);

        if (playerCollider.name == "PlayerObject")
            Debug.Log("Working");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject == )
    }
}
