using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
  

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.position - new Vector3(0, -1, 3);
    }
}
