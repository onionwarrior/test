using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorLayers : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()=> GetComponent<Projector>().ignoreLayers = ~(1 << 8);
}
