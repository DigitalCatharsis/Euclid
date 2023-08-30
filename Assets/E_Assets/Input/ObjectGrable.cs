using UnityEngine;

namespace Euclid
{
    public class ObjectGrable : MonoBehaviour
    { 
        private Rigidbody _objectRigidBody;
        private Transform _objectGrabPointTransform;
        private float _lerpSpeed = 100f;
        public Camera _playerCamera;

        private void Awake()
        {
            _objectRigidBody = GetComponent<Rigidbody>();
        }

        public void Grab(Transform objectGrabPointTransform)
        {            
            this._objectGrabPointTransform = objectGrabPointTransform;
            objectGrabPointTransform.position = transform.position;
            _objectRigidBody.useGravity = false;
        }

        public void Drop()
        {
            this._objectGrabPointTransform = null;
            _objectRigidBody.useGravity = true;
        }

        private void FixedUpdate()
        {
            if (_objectGrabPointTransform != null)
            {
                Vector3 newPosition = Vector3.Lerp(transform.position, _objectGrabPointTransform.position, Time.deltaTime * _lerpSpeed);
                _objectRigidBody.MovePosition(newPosition);
            }
        }
    }
}
