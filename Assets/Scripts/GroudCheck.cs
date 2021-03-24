using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroudCheck : MonoBehaviour
{
    [SerializeField]private GameObject dustCloud;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y-0.3f, transform.position.z);
            Instantiate(dustCloud, pos, dustCloud.transform.rotation);
        }
    }
}
