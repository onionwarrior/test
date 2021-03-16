using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AttachToObject : MonoBehaviour
{
    [SerializeField]
    private GameObject newParentObject;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = newParentObject.transform;
        transform.localPosition = Vector3.zero;
        transform.rotation = newParentObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
