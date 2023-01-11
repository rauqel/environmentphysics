using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTerrain : MonoBehaviour
{
    public FirstPersonController playerScript;

    private GameObject floorObject;
    private BoxCollider floorCollider;

    private CapsuleCollider playerCollider;
    public GameObject playerObject;
    public bool changeSpeed;

    void Start()
    {
        floorObject = GameObject.FindGameObjectWithTag("Terrain");
        floorCollider = floorObject.GetComponent<BoxCollider>();

        playerCollider = playerScript.GetComponentInChildren<CapsuleCollider>();
    }

    private void Update()
    {
        //
    }
}
