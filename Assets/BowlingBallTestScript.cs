using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBallTestScript : MonoBehaviour
{
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(Vector3.right * -5000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
