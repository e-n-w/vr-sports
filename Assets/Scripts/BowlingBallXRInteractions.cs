using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BowlingBallXRInteractions : MonoBehaviour
{
    [SerializeField] private float rotationScalar = 1f; 
    public enum HandType { LeftHand, RightHand, None}
    
    private XRDirectInteractor leftController, rightController;
    void Start()
    {
   
    }

    void Update()
    {
        if (leftController.attachTransform.CompareTag("Bowling Ball"))
        {

        }
        else if (rightController.attachTransform.CompareTag("Bowling Ball"))
        {

        }
    }
    private void OnGrabbed(XRBaseInteractor interactor)
    {
        string hand = interactor.transform.name.Contains("Left") ? "Left Hand" : "Right Hand";
    }

    void ReleaseBall() => transform.Rotate(new Vector3(0, 0, SetHandZRotation(SetHand()) * rotationScalar));

    private float SetHandZRotation(XRDirectInteractor hand)
    {
        if (hand.transform.rotation.z > 179)
            return 179;
        else if (hand.transform.rotation.z < -179)
            return -179;
        else
            return hand.transform.rotation.z;
    }
    private XRDirectInteractor SetHand()
    {
        if (CheckHand() == HandType.LeftHand)
            return leftController;
        else if (CheckHand() == HandType.RightHand)
            return rightController;

        return null;
    }

    //Returns 0 on left hand, 1 on right hand, and -1 for all other cases
    HandType CheckHand()
    {
        if (leftController.attachTransform.CompareTag("Bowling Ball")) return HandType.LeftHand;
        else if (rightController.attachTransform.CompareTag("Bowling Ball")) return HandType.RightHand;
        else return HandType.None;
    }


}
