using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    [SerializeField] BoxCollider shaftCollider;

    [ContextMenu("Ignore Collisions")]
    void DisableCollisions()
    {
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), shaftCollider);
    }

    [ContextMenu("Enable collisions")]
    void EnableCollision()
    {
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), shaftCollider, false);
    }
}
