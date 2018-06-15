using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField]
    public GameObject player;

    Vector3 offset;

    // Use this for initialization
    void Start()
    {
        // we are using the offset, because there is a 
        // Z-value that is important for the camera position,
        // the consequence of equating the camera transformation
        // to that of the player is that we lose the Z-value, 
        // thus losing the ability to see the scene. 
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // The offset is added to maintain the Z-value of the camera. 
        transform.position = player.transform.position + offset;
    }
}