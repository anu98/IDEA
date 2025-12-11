using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AugmeNDT
{
    public class FollowViewingVector : MonoBehaviour
    {
        public float followSpeed = 5f; // Adjust how fast the book moves
        public Vector3 offset = new Vector3(0, 0, 2); // Offset from the camera

        private Transform mainCamera;

        void Start()
        {
            // Get the main camera
            mainCamera = Camera.main.transform;
        }

        void Update()
        {
            if (mainCamera == null) return;

            // Calculate the target position in front of the camera
            Vector3 targetPosition = mainCamera.position + mainCamera.forward * offset.z +
                                     mainCamera.up * offset.y +
                                     mainCamera.right * offset.x;

            // Smoothly move the book to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Make the book face the camera
            transform.LookAt(mainCamera.position);
            transform.Rotate(0, 180, 0); // Adjust if necessary to keep the book front-facing
        }
    }
}
