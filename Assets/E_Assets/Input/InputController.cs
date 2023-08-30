using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid
{
    public class InputController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 100f;

        [SerializeField] private Transform _playerCameraTransform;

        [SerializeField] private float _pickupDistance = 1000f;
        [SerializeField] private LayerMask _pickUpLayerMask;
        [SerializeField] private Transform _objectGrabPointTransform;
        private ObjectGrable _objectGrabble;

        private float _distance;
        private float _kScale;
        private bool _isGrabbing = false;

        private void Update()
        {
            MovementAndTurning();
            DragAndDrop();
        }

        private void DrawLine()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.white, 2.0f);
        }

        private void DragAndDrop()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                DrawLine();
                if (_objectGrabble == null)
                {
                    if (Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out RaycastHit raycastHit, _pickupDistance))
                    {
                        if (raycastHit.transform.TryGetComponent(out _objectGrabble))
                        {
                            _isGrabbing = true;
                            Debug.Log(raycastHit.transform);
                            _objectGrabble.Grab(_objectGrabPointTransform);
                            GetProportion();
                            StartCoroutine(Proportion());
                        }
                    }
                }
            }
            else if (_isGrabbing)
            {
                StopAllCoroutines();
                _isGrabbing = false;
                _objectGrabble.Drop();
                _objectGrabble = null;
            }
        }

        private IEnumerator Proportion()
        {
            while (_isGrabbing)
            {
                float localScale = Vector3.Distance(GetComponentInChildren<Camera>().transform.position, _objectGrabble.transform.position) / _kScale;
                _objectGrabble.transform.localScale = new Vector3(1, 1, 1) * localScale;
                yield return null;
            }
        }

        private void GetProportion()
        {
            Debug.Log("Setting");
            _distance = Vector3.Distance(GetComponentInChildren<Camera>().transform.position, _objectGrabble.transform.position); //distance between player and obj
            var objScale = _objectGrabble.transform.localScale;
            _kScale = _distance / objScale.x;
            Debug.Log($"distance = {_distance} || objScale = {objScale} || k = {_kScale}");
        }

        private void MovementAndTurning()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            move.y = -100 * Time.deltaTime;
            GetComponent<CharacterController>().Move(move * _speed * Time.deltaTime);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void LateUpdate()
        {
            _playerCameraTransform.position = this.transform.position;
        }
    }
}
