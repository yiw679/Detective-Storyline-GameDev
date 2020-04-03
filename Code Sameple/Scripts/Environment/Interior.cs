using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interior : MonoBehaviour
{
    public NavMeshSurface surface;

    private GameObject interior;
    private GameObject exterior;
    private bool inside = false;
    private bool navMeshUpdated = true;

    private void Start()
    {
        interior = transform.GetChild(0).gameObject;
        exterior = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        /*if(inside)
        {
            exterior.SetActive(false);
            interior.SetActive(true);

            if(!navMeshUpdated)
            {
                surface.BuildNavMesh();
                navMeshUpdated = true;
            }
        }
        else
        {
            exterior.SetActive(true);
            interior.SetActive(false);

            if (!navMeshUpdated)
            {
                surface.BuildNavMesh();
                navMeshUpdated = true;
            }
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            exterior.SetActive(false);
            interior.SetActive(true);
            surface.BuildNavMesh();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            exterior.SetActive(true);
            interior.SetActive(false);
            surface.BuildNavMesh();
        }
    }
}
