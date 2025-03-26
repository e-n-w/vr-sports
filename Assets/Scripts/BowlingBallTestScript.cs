
using UnityEngine;

public class BowlingBallTestScript : MonoBehaviour
{
    Rigidbody body;
    Vector3 startPosition;
    [SerializeField] float moveScalar = 0.01f, forceScalar = 8000f; 

    void Start()
    {
        body = GetComponent<Rigidbody>();
        startPosition = body.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            body.velocity = Vector3.zero;
            body.position = startPosition;
            
        }
        if (Input.GetKey(KeyCode.A))
            body.position += Vector3.right* moveScalar;
        if (Input.GetKey(KeyCode.D))
            body.position -= Vector3.right* moveScalar;
        if (Input.GetKeyDown(KeyCode.Space))
            body.AddForce(Vector3.forward * -forceScalar);
    }
}
