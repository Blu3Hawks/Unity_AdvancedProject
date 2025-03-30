using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        private void LateUpdate()
        {
            if (mainCamera)
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }

        public void SetupCamera(Camera mainCamera)
        {
            this.mainCamera = mainCamera;
        }
    }
}
