using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainControl : MonoBehaviour
{
    [SerializeField] private LayerMask grassFloor;
    private bool isOnGrass;


    private void Update()
    {
        isOnGrass = Physics.Raycast(transform.position, Vector3.down, 1 * 0.5f + 0.2f, grassFloor);

        if (isOnGrass)
        {
            Debug.Log("hey");
        }
    }
}
