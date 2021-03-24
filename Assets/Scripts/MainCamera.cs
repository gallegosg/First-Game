using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    float smoothTime = 0.3F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Define a target position in front of player
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -7.5f);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
