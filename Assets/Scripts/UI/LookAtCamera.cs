using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        private void LateUpdate()
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}
