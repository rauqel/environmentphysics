using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTerrain : MonoBehaviour
{
    public FirstPersonController playerScript;

    private BoxCollider floorCollider;
    private CapsuleCollider playerCollider;

    void Start()
    {
        floorCollider = GetComponent<BoxCollider>();
        playerCollider = playerScript.GetComponentInChildren<CapsuleCollider>();
    }

    private void Update()
    {
        Debug.Log(playerCollider);

        if (playerCollider.name == "PlayerObject")
            Debug.Log("Working");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == )
    }
}
