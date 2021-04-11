using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFloor : MonoBehaviour
{
    SpringJoint springJoint;
    // Start is called before the first frame update
    void Start()
    {
        springJoint = GetComponent<SpringJoint>();

        springJoint.connectedAnchor = transform.position;
        springJoint.anchor = new Vector3(0, -1, 0);
    }
}
