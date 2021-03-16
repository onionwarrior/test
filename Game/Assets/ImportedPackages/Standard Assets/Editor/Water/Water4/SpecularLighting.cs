using UnityEngine;

namespace ImportedPackages.Standard_Assets.Editor.Water.Water4
{
    [RequireComponent(typeof(WaterBase))]
    [ExecuteInEditMode]
    public class SpecularLighting : MonoBehaviour
    {
        public Transform specularLight;
        private WaterBase m_WaterBase;


        public void Start()
        {
            m_WaterBase = (WaterBase)gameObject.GetComponent(typeof(WaterBase));
        }


        public void Update()
        {
            if (!m_WaterBase)
            {
                m_WaterBase = (WaterBase)gameObject.GetComponent(typeof(WaterBase));
            }

            if (specularLight && m_WaterBase.sharedMaterial)
            {
                m_WaterBase.sharedMaterial.SetVector("_WorldLightDir", specularLight.transform.forward);
            }
        }
    }
}