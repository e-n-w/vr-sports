
using UnityEngine;

public class BowlingBallTestScript : MonoBehaviour
{
    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(Vector3.forward * -8000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
