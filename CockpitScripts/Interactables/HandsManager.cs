using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsManager : MonoBehaviour
{

    [SerializeField] SphereCollider leftHandCollider;
    [SerializeField] SphereCollider rightHandCollider;

    void Start()
    {
        leftHandCollider.enabled = true;
        rightHandCollider.enabled = true;
    }
}
