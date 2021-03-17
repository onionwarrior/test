using System;
using System.Collections;
using Animations;
using Code;
using Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class Combat : MonoBehaviour
    {
        private bool _isShooting = false;
        private bool _isArmed = false;
        private static readonly int Armed = Animator.StringToHash("IsArmed");
        public bool IsArmed { get => _isArmed; set => _isArmed=value; }
        private NavMeshAgent _agent;
        private PlayerScript _player;
        private void Start()
        {
            
            _agent = GetComponent<NavMeshAgent>();
            _player = SceneManagement.SceneManager.Instance.player;
        }

        private IEnumerator Track(BotsAI enemy)
        {
            var t = 0f;
            while (transform.forward!=enemy.transform.position-transform.position)
            {
                var startRotation = transform.rotation;
                var direction = enemy.transform.position - transform.position;
                direction.y = 0;
                var finalRotation = Quaternion.LookRotation(direction);
                t += Time.deltaTime / (Vector3.Angle(transform.forward,direction )/120.0f);
                transform.rotation = Quaternion.Lerp(startRotation, finalRotation, t);
                yield return null;
            }
        }
        private  IEnumerator ShootCoroutine(Animator animator,AudioSource audio,BotsAI enemy)
        {
            _player.canMove = false;
            _agent.SetDestination(transform.position);
            if (!_isArmed)
            {
                _isArmed = true;
                animator.Play("Unsheathe");
                yield return new WaitForSeconds(1.5f);
                SceneManagement.SceneManager.Instance.player.ikActive = true;
                animator.SetBool(Armed,true);
                yield return new WaitForSeconds(2f);
            }
            _isShooting = true; 
            animator.Play("Shoot");
            Coroutine tracker = StartCoroutine(Track(enemy));
            yield return new WaitForSeconds(1.5f);
            audio.Play();
            yield return new WaitForSeconds(.5f);
            StopCoroutine(tracker);
            enemy.TakeDamage(40);
            animator.Play("Stop");
            _player.canMove = true;
            _isShooting = false;
        }

        public void RaycastShoot(AudioSource audio, Camera camera, float maximumShootDistance,
            float maximumHitDisctance, Transform attacker, Animator animator,GameObject target)
        {
            if (Vector3.Distance(attacker.position, target.transform.position) <= maximumShootDistance)
            {
                var position = attacker.position;
                Ray shootRay = new Ray(position, target.transform.position - position);
                if(Physics.Raycast(shootRay,out RaycastHit enemyHitInfo, maximumShootDistance,~(1<<14|1<<(int)Layers.Floor)))
                {
                    print(enemyHitInfo.transform.gameObject);
                    if(enemyHitInfo.transform.gameObject.layer==(int)Layers.Enemies)
                    {
                        BotsAI enemyObject = enemyHitInfo.transform.gameObject.GetComponentInParent<BotsAI>();
                        if (enemyObject.IsAlive&&!_isShooting)
                            StartCoroutine(ShootCoroutine(animator,audio,enemyObject));
                    }
                }
            }
        }
    }
}
