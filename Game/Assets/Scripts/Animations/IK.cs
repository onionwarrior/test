using UnityEditor.SearchService;
using UnityEngine;

namespace Animations
{
    public class IK : MonoBehaviour
    {
        public GameObject triggerFinger;
        public GameObject handHolder;
        private void Update()
        {
            if (SceneManagement.SceneManager.Instance.player.ikActive)
            {
                transform.parent.position += triggerFinger.transform.position - transform.position;
                Vector3 v = handHolder.transform.position - triggerFinger.transform.position;
                transform.parent.rotation = Quaternion.FromToRotation(Vector3.right, v);
            }

        }
    }
}