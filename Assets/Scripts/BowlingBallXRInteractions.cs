using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BowlingBallXRInteractions : MonoBehaviour
{
    [SerializeField] private float rotationScalar = 2f;
    Rigidbody body;
    public enum HandType { LeftHand, RightHand, None}
    
    [SerializeField] private XRDirectInteractor leftController, rightController;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        XRDirectInteractor[] hands = FindObjectsOfType<XRDirectInteractor>();
        Debug.Log(hands.Length);
        if (hands[0].transform.name == "Left Controller")
            leftController = hands[0];
        if (hands[1].transform.name == "Right Controller")
            rightController = hands[1];
        //rightController = GameObject.FindGameObjectWithTag("Right Controller");
    }

    void Update()
    {
      /*
        if (leftController.attachTransform.CompareTag("Bowling Ball"))
        {

        }
        else if (rightController.attachTransform.CompareTag("Bowling Ball"))
        {

        }
      */
    }

    public void OnGrabbed(XRBaseInteractor interactor)
    {
        
    }

    public void ReleaseBall()
    {
        transform.Rotate(new Vector3(0, 0, SetHandZRotation(SetHand()) * rotationScalar));
        body.velocity = new Vector3 (body.velocity.x, Mathf.Clamp(body.velocity.y, -100f, 10f), body.velocity.z);
    }
    private float SetHandZRotation(XRDirectInteractor hand)
    {
        //if(hand.transform.eulerAngles)
        if (hand.transform.rotation.x > 1)
            return 1;
        else if (hand.transform.rotation.x < -1)
            return -1;
        else
            return hand.transform.rotation.x;
    }

    private XRDirectInteractor SetHand()
    {
        if (CheckHand() == HandType.LeftHand)
            return leftController;
        else if (CheckHand() == HandType.RightHand)
            return rightController;

        return null;
    }

    HandType CheckHand()
    {
        if (leftController.attachTransform.CompareTag("BowlingBall")) return HandType.LeftHand;
        else if (rightController.attachTransform.CompareTag("BowlingBall")) return HandType.RightHand;
        else return HandType.None;
    }


}
