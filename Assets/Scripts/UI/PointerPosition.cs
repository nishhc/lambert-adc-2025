using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerPosition : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    void Update() {
        if (objectToFollow != null) {
            transform.position = new Vector3(objectToFollow.position.x, transform.position.y, objectToFollow.position.z);
        }
    }
}
