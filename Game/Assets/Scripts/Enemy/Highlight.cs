using UnityEngine;

namespace Enemy
{
    public class Highlight : MonoBehaviour
    {
        private Material _mat;
        private static readonly int FirstOutlineWidth = Shader.PropertyToID("_FirstOutlineWidth");

        private void Start()
        {
            _mat = GetComponent<SkinnedMeshRenderer>().material;
        }

        private void OnMouseOver()
        {
            _mat.SetFloat(FirstOutlineWidth,0.08f);
        }

        private void OnMouseExit()
        {
            _mat.SetFloat(FirstOutlineWidth,0);
        }
    }
}
