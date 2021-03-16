using UnityEngine;

namespace GameCamera
{
    public class CursorScript : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private Texture2D badCursorTexture;
        private CursorMode cursorMode = CursorMode.Auto;
        private Camera _camera;

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
            badCursorTexture = Resources.Load<Texture2D>("bad_cursor");
            _camera = UnityEngine.Camera.main;
        }

        // Update is called once per frame
        private void Update()
        {
            Ray ray= _camera.ScreenPointToRay(Input.mousePosition);
            Cursor.SetCursor(!Physics.Raycast(ray, Mathf.Infinity, (1 << 9)) ? null : badCursorTexture, Vector2.zero,
                cursorMode);
        }
    }
}
