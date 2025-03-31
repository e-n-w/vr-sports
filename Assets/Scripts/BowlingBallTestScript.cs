using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BowlingBallTestScript : MonoBehaviour
{
    private Rigidbody body;
    private Vector3 startPosition;
    [SerializeField] private float moveScalar = 0.01f, forceScalar = 8000f;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        startPosition = body.position;
    }
    // Update is called once per frame
    private void Update()
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
