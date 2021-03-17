using UnityEngine;
using UnityEngine.SceneManagement;
namespace SceneManagement
{
    public class ClickToLoad : MonoBehaviour
    {
        public int sceneIndex;
        private void OnMouseDown()
        {
            SceneManager.Instance.player.anchorName = "InteriorAnchor";
            SceneManager.Instance.player.cameraAnchorName = "InteriorCameraAnchor";
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
    }
}
