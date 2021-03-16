using System;
using System.Collections;
using Code;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [RequireComponent(typeof(BotsAI))]
    public class BotsAI : MonoBehaviour, IReactToDamage<float>
    {
        private Transform _player;
        private float _healthPoints = 100;
        public bool IsAlive { get; set; } = true;
        [SerializeField] private float visionAngle;
        [SerializeField] private float visionDistance;
        [SerializeField] private GameObject shot;
        [SerializeField] private AudioClip hurtSoundEffect;
        private AudioSource _thisAudioSource;
        private EnemyPatroller _patroller;
        private int _questId = 1;
        public int questId { get; set; }
        [SerializeField] private GameObject rayCastTarget;
        
        private void Start()
        {
            _thisAudioSource = GetComponent<AudioSource>();
            _patroller = GetComponent<EnemyPatroller>();
        }

        public void Die()
        {
            StartCoroutine(DeathAnimation());
            //Play Death animation
        }

        private IEnumerator DeathAnimation()
        {
            this.transform.Rotate(-75, 0, 0);
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        public void TakeDamage(float damage)
        {
            _healthPoints -= damage;
            if (_healthPoints > 0)
                _thisAudioSource.Play();
            else
                Die();
        }

        public void ReactToDamage(float damage)
        {
            TakeDamage(damage);
            //Text react
            //Gibs?
            //Limbs?
        }

        private bool IsVisible()
        {
            Vector3 origin = transform.position;
            var transform1 = SceneManage.SceneManager.Instance.player.gameObject.transform;
            origin.y = transform1.position.y;
            float angleBetweenAIAndPlayer = Vector3.Angle(transform.forward, transform1.position-origin);
            if (angleBetweenAIAndPlayer < visionAngle / 2.0f &&
                Vector3.Distance(origin, rayCastTarget.transform.position) <= visionDistance)
            {
               print("pedik");
                Ray visionRay = new Ray(origin, transform1.position - origin);
                if (Physics.Raycast(visionRay, out RaycastHit hitObject, visionDistance))
                {
                    print(hitObject.transform.gameObject.layer);
                    if (hitObject.transform.gameObject.layer == 10)
                    {
                        print("Vizhu pedika");
                        return true;
                    }
                }
                Debug.DrawRay(visionRay.origin,visionRay.direction*100,Color.blue,20.0f);
                
                
                
            }

            return false;
        }

        void Update()
        {
            if (IsAlive)
            {
                _patroller.IsPatrolling = !IsVisible();
                if(!_patroller.IsPatrolling)
                    _patroller.CancelInvoke();
            }
        }

        //void OnGUI() 
        //{
        //    int size = 32;
        //    float posX = _camera.pixelWidth / 3 - size / 4;
        //    float posY = _camera.pixelHeight / 2 - size / 2;
        //    GUI.Label(new Rect(posX, posY, size, size), "Aaauauau");
        //}
    }
}