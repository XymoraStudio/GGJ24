using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;

    void Update()
    {
        transform.position = playerPosition.position;
        transform.rotation = playerPosition.rotation;
    }
}
