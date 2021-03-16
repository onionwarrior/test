using UnityEngine;

namespace GameCamera
{
    public class CameraZoomScript : MonoBehaviour
    {
        private Camera _myCamera;
        [SerializeField] 
        private float zoomSpeedModifier = 2;
        private void Start() => _myCamera = Camera.main;
        private void Update()
        {
            if (!InGameMenu.GameIsPaused)
            {
                float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
                if (mouseScroll != 0)
                    _myCamera.orthographicSize = Mathf.Clamp(_myCamera.orthographicSize - mouseScroll * zoomSpeedModifier, 7.0f, 17.0f);
            }
        }
    }
}
