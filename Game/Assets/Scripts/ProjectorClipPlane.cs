using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Projector))]
public class ProjectorClipPlane : MonoBehaviour
{
    private Projector _attachedProjector;
    void Start()
    {
        _attachedProjector = GetComponent<Projector>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray,out RaycastHit floorHit))
        {
            _attachedProjector.farClipPlane = Vector3.Distance(transform.position, floorHit.point)*1.001f;
        }
    }
}
