using UnityEngine;

namespace Euclid
{
    public class ObjectGrable : MonoBehaviour
    {
        private Rigidbody _objectRigidBody;
        private Transform _objectGrabPointTransform;
        private float _lerpSpeed = 100f;
        public Camera _playerCamera;
        private SphereCollider _ignoreSphereCollider;

        private void Awake()
        {
            _objectRigidBody = GetComponent<Rigidbody>();
        }

        public void Grab(Transform objectGrabPointTransform)
        {
            this._objectGrabPointTransform = objectGrabPointTransform;
            objectGrabPointTransform.position = transform.position;
            _objectRigidBody.useGravity = false;
            GetComponent<Collider>().enabled = false;
        }
        public void Drop()
        {
            this._objectGrabPointTransform = null;
            _objectRigidBody.useGravity = true;
            GetComponent<Collider>().enabled = true;
        }
         
        private void FixedUpdate()
        {
            if (_objectGrabPointTransform != null)
            {
                var rayhitPosition = _playerCamera.transform.root.GetComponent<InputController>().rayHit;
                Debug.Log(rayhitPosition);
                if (rayhitPosition.point != Vector3.zero)
                {
                    var newPosition = rayhitPosition.point - rayhitPosition.transform.forward * (transform.localScale.x / 2);
                    var resultPosition = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _lerpSpeed);
                    _objectRigidBody.MovePosition(resultPosition);
                }
                else
                {
                    var newPosition = Vector3.Lerp(transform.position, _objectGrabPointTransform.position, Time.deltaTime * _lerpSpeed);
                    _objectRigidBody.MovePosition(newPosition);
                }
            }
        }
    }
}
