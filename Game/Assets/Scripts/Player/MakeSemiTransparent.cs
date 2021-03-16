using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;

public class MakeSemiTransparent : MonoBehaviour
{
    //private List<GameObject> TransparentObjects = new List<GameObject>();
    private Dictionary<GameObject, Renderer> _transparentObjectRenderers = new Dictionary<GameObject, Renderer>();
    private Transform _myCamera;
    private NavMeshAgent _playerAgent;
    private SkinnedMeshRenderer _playerRenderer;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material outlineMaterial;

    private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
    private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
    private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

    private static void ToOpaqueMode(Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }

    private static void ToFadeMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt(ZWrite, 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
    void Start()
    {
        _playerRenderer= GetComponent<SkinnedMeshRenderer>();
        //DefaultShader = PlayerRenderer.material.shader;
        defaultMaterial = _playerRenderer.material;
        _playerAgent = GetComponentInParent<NavMeshAgent>();
        if (Camera.main is { }) _myCamera = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 target = _playerAgent.transform.position;
        Ray ray = new Ray(_myCamera.position, target - _myCamera.position);
        GameObject hitGameObject = null;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            hitGameObject = hit.transform.gameObject;
            if (hitGameObject != _playerAgent.gameObject)
            {
                switch (hitGameObject.layer)
                {
                    case 11:
                        _playerRenderer.material = outlineMaterial;
                        break;
                    case 12:
                        if (_transparentObjectRenderers.ContainsKey(hitGameObject))
                            break;
                        _transparentObjectRenderers.Add(hitGameObject, hitGameObject.GetComponent<Renderer>());
                        Color currentColor = _transparentObjectRenderers[hitGameObject].material.color;
                        ToFadeMode(_transparentObjectRenderers[hitGameObject].material);
                        currentColor.a = 0.5f;
                        _transparentObjectRenderers[hitGameObject].material.color = currentColor;
                        break;
                    default:
                        _playerRenderer.material = defaultMaterial;
                        break;
                }
            }
            else
                _playerRenderer.material = defaultMaterial;
            foreach(var key in _transparentObjectRenderers.Keys)
            {
                if (key == hitGameObject) continue;
                Color currentColor = _transparentObjectRenderers[key].material.color;
                ToOpaqueMode(_transparentObjectRenderers[key].material);
                _transparentObjectRenderers[key].material.color = currentColor;
            }
            _transparentObjectRenderers = _transparentObjectRenderers.Where(x => x.Key == hitGameObject).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
