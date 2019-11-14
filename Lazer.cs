﻿using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public GameObject dot;
    public int rows = 10;
    public int columns = 10;
    public float spacing = 10.0f;
    private List<GameObject> dots = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns; j++){
                GameObject temp = Instantiate(dot, transform.position, Quaternion.identity);
                temp.name = "i : "+i+ " , j : "+j;
                dots.Add(temp);
            }
        }
    }
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        RaycastHit hit;
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns; j++){
                GameObject dot = dots[i*rows+j];
                Vector3 direction = Quaternion.AngleAxis(spacing*i-(columns*spacing/2), Vector3.right) * Vector3.forward;
                direction = Quaternion.AngleAxis(spacing*j-(rows*spacing/2), Vector3.up) * direction;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    Vector3 hitLocation =  transform.TransformDirection(direction) * hit.distance;
                    Debug.DrawRay(transform.position, hitLocation, Color.yellow);
                    dot.transform.position = transform.position + hitLocation;
                    dot.SetActive(true);
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(direction) * 1000, Color.white);
                    dot.SetActive(false);
                }
                }
        }
    }
}
