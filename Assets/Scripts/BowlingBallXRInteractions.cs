using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BowlingBallXRInteractions : MonoBehaviour
{
    [SerializeField] private float rotationScalar = 2f;
    Rigidbody body;
    public enum HandType { LeftHand, RightHand, None}
    
    [SerializeField] private UnityEngine.XR.InputDevice leftController, rightController;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        leftController =  InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (args.interactorObject is XRBaseInputInteractor controllerInteractor && controllerInteractor != null)
        {
            var controller = controllerInteractor.xrController;
        }
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

    public void OnGrabbed()
    {
        rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion rotR);

    }

    public void ReleaseBall()
    {
        transform.Rotate(new Vector3(0, 0, SetHandZRotation(controller) * rotationScalar));
        body.velocity = new Vector3 (body.velocity.x, Mathf.Clamp(body.velocity.y, 0f, 10f), body.velocity.z);
    }
    private float SetHandZRotation(IXRSelectInteractor hand)
    {

        //if(hand.transform.eulerAngles)
        if (hand.transform.rotation.x > 1)
            return 1;
        else if (hand.transform.rotation.x < -1)
            return -1;
        else
            return hand.transform.rotation.x;
    }

    private UnityEngine.XR.InputDevice SetHand()

    {

        if (CheckHand() == HandType.LeftHand)

            return leftController;

        else if (CheckHand() == HandType.RightHand)

            return rightController;



        return new UnityEngine.XR.InputDevice();

    }


    HandType CheckHand()

    {

        if (leftController.attachTransform.CompareTag("BowlingBall")) return HandType.LeftHand;

        else if (rightController.attachTransform.CompareTag("BowlingBall")) return HandType.RightHand;

        else return HandType.None;

    }

}
