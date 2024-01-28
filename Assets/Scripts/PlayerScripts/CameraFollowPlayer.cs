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