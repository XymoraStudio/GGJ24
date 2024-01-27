using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;

    void LateUpdate()
    {
        transform.position = playerPosition.position;
        transform.rotation = playerPosition.rotation;
    }
}
