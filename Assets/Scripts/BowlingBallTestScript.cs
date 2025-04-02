using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BowlingBallTestScript : MonoBehaviour
{
    private Rigidbody body;
    private Vector3 startPosition;
    [SerializeField] private float moveScalar = 0.01f, forceScalar = 8000f;
    [SerializeField] private TextMeshProUGUI debugText;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        startPosition = body.position;
    }
    // Update is called once per frame
    private void Update()
    {

        debugText.text = "X: "+transform.rotation.x+" | Y: "+transform.rotation.y+" | Z: "+transform.rotation.z;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = Vector3.zero;
            body.rotation = Quaternion.identity;
            body.position = startPosition;
            
        }
        if (Input.GetKey(KeyCode.A))
            body.position += Vector3.right* moveScalar;
        if (Input.GetKey(KeyCode.D))
            body.position -= Vector3.right* moveScalar;
        if (Input.GetKeyDown(KeyCode.B))
            body.AddForce(Vector3.forward * -forceScalar);
    }
}
