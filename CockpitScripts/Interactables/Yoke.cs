using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Yoke : MonoBehaviour
{
    [SerializeField] XRInteractionManager interactionManager;
    [SerializeField] SphereCollider leftHandCollider;
    [SerializeField] SphereCollider rightHandCollider;

    XRGrabInteractable grabInteractable;
    IXRSelectInteractor handInteractor;

    bool grabbed;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        leftHandCollider.enabled = true;
        rightHandCollider.enabled = true;
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Hand"))
        {
            print("Hand entered");
            handInteractor = col.GetComponent<IXRSelectInteractor>();

            if (handInteractor != null && !grabInteractable.isSelected)
            {
                print("Hand entered");
                interactionManager.SelectEnter(handInteractor, grabInteractable);
                grabbed = true;
            }
        }
    }

    private void Update()
    {
        if (!grabbed) return;

        handInteractor.selectExited.AddListener(_ => ExitGrab());
    }

    void ExitGrab()
    {
        if (!grabbed) return;

        interactionManager.SelectExit(handInteractor, grabInteractable);
        grabbed = false;
    }
}
