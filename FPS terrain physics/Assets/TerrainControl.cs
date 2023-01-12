using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainControl : MonoBehaviour
{
    [Header("Terrain Parameters")]
    private float currentLowerDivision;
    private float currentUpperDivision;
    [SerializeField] private float grassLowerDivision = 1.0f;
    [SerializeField] private float grassUpperDivision = 2.0f;
    public bool isOnGrass;
    //
    [SerializeField] private float sandLowerDivision = 1.75f;
    [SerializeField] private float sandUpperDivision = 3;
    public bool isOnSand;
    //
    [SerializeField] private float snowLowerDivision = 1.4f;
    [SerializeField] private float snowUpperDivision = 2.5f;
    public bool isOnSnow;
    //
    private float energyTimer;
    private bool canChange;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            Debug.Log("Working");
    }
}
