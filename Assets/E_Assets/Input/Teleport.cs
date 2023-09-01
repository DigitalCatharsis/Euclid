using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Euclid
{   
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private BoxCollider teleportCollider;

        private void OnTriggerEnter(Collider hit)
        {
            var currentColliderWorldPosition = this.transform.position;
            var currentColliderWorldRotation = this.transform.rotation;
            var playerDeltaPosition = currentColliderWorldPosition - hit.transform.root.position;
            var playerDeltaRotation = Quaternion.Inverse(currentColliderWorldRotation) * hit.transform.root.rotation;
            playerDeltaPosition.y = 0;

            if (hit.gameObject.layer == 6)
            {
                Debug.Log(hit.gameObject.transform.root.name);
                Debug.Log(hit.gameObject.layer == 6);

                var charController = hit.GetComponentInChildren<CharacterController>();
                charController.enabled = false;
                charController.transform.position = teleportCollider.gameObject.transform.position + playerDeltaPosition;

                var rotation = teleportCollider.gameObject.transform.eulerAngles;
                rotation = new Vector3(rotation.x, rotation.y, rotation.z);
                charController.transform.rotation = Quaternion.Euler(rotation) * playerDeltaRotation;
                charController.enabled = true;
            }
        }
    }
}
