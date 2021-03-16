using Enemy;
using UnityEngine;

namespace Player
{
    public class RaycastShoot : MonoBehaviour
    {
        [SerializeField]
        private float maximumHitDistance;
        [SerializeField]
        private float maximumShootDistance;
        private Camera _myCamera;
        private AudioSource _audio;
        private void Start()
        {
            _myCamera = Camera.main;
            _audio = GetComponent<AudioSource>();
            
        }
        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray chooseTargetRay = _myCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(chooseTargetRay,out RaycastHit hitInfo,Mathf.Infinity,1<<13))
                {
                    print("Ray hit enemy");
                    if (Vector3.Distance(transform.position, hitInfo.transform.position) <= maximumShootDistance)
                    {
                        Ray shootRay = new Ray(transform.position, hitInfo.transform.position - transform.position);
                        if(Physics.Raycast(shootRay,out RaycastHit enemyHitInfo, maximumShootDistance,~(1<<14)))
                        {
                            if(enemyHitInfo.transform.gameObject.layer==13)
                            {
                                BotsAI enemyObject = enemyHitInfo.transform.gameObject.GetComponentInParent<BotsAI>();
                                _audio.Play();
                                if(enemyObject.IsAlive)
                                    enemyObject.TakeDamage(40.0f);
                            }
                            else
                            {
                                print("No enemy hit");
                            }
                        }
                    }
                    else
                    {
                        print("You are too far away");
                    }
                }
            }
        }
    }
}
