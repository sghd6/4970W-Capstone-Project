using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);

    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float rotationSmoothTime = 0.12f;
    [SerializeField] private bool lookAtTarget = true;

    [Header("Input Settings")]
    [SerializeField] private bool allowCameraRotation = true;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 60f;

    // Private variables
    private Vector3 smoothedPosition;
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    private void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning("No target assigned to CameraController and no GameObject with 'Player' tag found.");
            }
        }

        // Set initial rotation values
        currentRotationY = transform.eulerAngles.y;
        currentRotation = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (allowCameraRotation)
        {
            // Get mouse input for camera rotation
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Adjust current rotation based on input
            currentRotationY += mouseX;
            currentRotationX -= mouseY;

            // Clamp vertical rotation
            currentRotationX = Mathf.Clamp(currentRotationX, minVerticalAngle, maxVerticalAngle);

            // Create target rotation
            Vector3 targetRotation = new Vector3(currentRotationX, currentRotationY, 0f);

            // Smooth rotation
            currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

            // Apply rotation to camera
            transform.eulerAngles = currentRotation;

            // Calculate the rotated offset
            Vector3 rotatedOffset = Quaternion.Euler(currentRotation.x, currentRotation.y, 0) * offset;

            // Calculate the target position with rotated offset
            Vector3 targetPosition = target.position + rotatedOffset;

            // Smoothly move camera to target position
            smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            // Simple follow without rotation input
            Vector3 desiredPosition = target.position + offset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            if (lookAtTarget)
            {
                transform.LookAt(target);
            }
        }
    }
}