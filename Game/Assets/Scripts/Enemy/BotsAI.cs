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
        private Animator _animator;
        private int _questId = 1;
        public int questId { get; set; }
        [SerializeField] private GameObject rayCastTarget;
        private static readonly int Death = Animator.StringToHash("Death");

        private void Start()
        {
            _thisAudioSource = GetComponent<AudioSource>();
            _patroller = GetComponent<EnemyPatroller>();
            _animator = GetComponent<Animator>();
        }

        public void Die()
        {
            IsAlive = false;
            _animator.SetTrigger(Death);
            GetComponentInChildren<CapsuleCollider>().enabled = false;
        }


        public void TakeDamage(float damage)
        {
            _healthPoints -= damage;
            _thisAudioSource.Play();
            if(_healthPoints<=0)
                Die();
        }

        public void ReactToDamage(float damage)
        {
            if (IsAlive)
            {
                TakeDamage(damage);
            }
        }

        private bool IsVisible()
        {
            Vector3 origin = transform.position;
            var transform1 = SceneManage.SceneManager.Instance.player.gameObject.transform;
            origin.y = transform1.position.y+3f;
            float angleBetweenAIAndPlayer = Vector3.Angle(transform.forward, transform1.position-origin);
            if (angleBetweenAIAndPlayer < visionAngle / 2.0f &&
                Vector3.Distance(origin, rayCastTarget.transform.position) <= visionDistance)
            {
                Ray visionRay = new Ray(origin, transform1.position - origin);
                if (Physics.Raycast(visionRay, out RaycastHit hitObject, visionDistance))
                {
                    print(hitObject.transform.gameObject.layer);
                    if (hitObject.transform.gameObject.layer == (int)Layers.Player)
                    {
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