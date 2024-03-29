using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target = null;

    private float smoothSpeed = 0.125f;

    [SerializeField]
    private Vector3 offset = new Vector3(0f,2.25f,-1f);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate() {
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
