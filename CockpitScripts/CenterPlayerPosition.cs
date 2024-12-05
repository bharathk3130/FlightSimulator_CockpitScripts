using UnityEngine;

public class CenterPlayerPosition : MonoBehaviour
{
    [SerializeField] Transform headset;
    [SerializeField] Transform targetHeadsetPosition;
    [SerializeField] float recenteringDelay = 1;

    private void Start()
    {
        Invoke(nameof(CenterHeadset), recenteringDelay);
    }

    [ContextMenu("Center Position")]
    void CenterHeadset()
    {
        Vector3 offset = headset.position - targetHeadsetPosition.position;
        transform.position -= offset;
    }
}
