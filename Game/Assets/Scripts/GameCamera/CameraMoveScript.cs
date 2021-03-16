using System;
using UnityEngine;

namespace GameCamera
{
    public class CameraMoveScript : MonoBehaviour
    {
        private int _width;//Ширина экрана
        private int _height;//Высота экрана
        private float _startYOffset;//Начальная y координата
        [SerializeField] private float cameraMoveSpeed;
        [SerializeField] private int edgeDistance = 10;
        [SerializeField] 
        private float zoomSpeedModifier = 2;

        private Camera _camera;
        private void Start()
        {
            _width = Screen.width;
            _height = Screen.height;
            _camera = Camera.main;
            _startYOffset = transform.position.y;
        }
        private void FixedUpdate()
        {
            var mousePosition = Input.mousePosition;
            var currentPosition = Vector3.zero;
            float moveDelta = cameraMoveSpeed * Time.fixedDeltaTime;
            if (!InGameMenu.GameIsPaused)
            {
                float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
                if (mouseScroll != 0)
                    _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - mouseScroll * zoomSpeedModifier, 7.0f, 17.0f);
            }
            if (mousePosition.x > _width - edgeDistance||Input.GetKey(KeyCode.RightArrow))
            {
                currentPosition.x += moveDelta ;
            }
            if (Input.mousePosition.x < edgeDistance || Input.GetKey(KeyCode.LeftArrow))
            {
                currentPosition.x -= moveDelta ;
            }   
            if (Input.mousePosition.y > _height - edgeDistance || Input.GetKey(KeyCode.UpArrow))
            {
                currentPosition.z += moveDelta/0.866f;
            }
            if (Input.mousePosition.y < edgeDistance || Input.GetKey(KeyCode.DownArrow))
            {
                currentPosition.z -= moveDelta / 0.866f;
            }
            transform.Translate(currentPosition);
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit groundBelowCamera,Mathf.Infinity,1<<8))
            {
                Vector3 newCoord = transform.position;
                newCoord.y = groundBelowCamera.point.y + _startYOffset;
                transform.position = newCoord;
            }
        }
    }
}
