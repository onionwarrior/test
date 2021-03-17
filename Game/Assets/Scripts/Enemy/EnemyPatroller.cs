using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyPatroller : MonoBehaviour
    {
        private Vector3 _start;
        public bool IsPatrolling { get; set; }= true;
        [SerializeField] private float patrolSpeed;
        private NavMeshAgent _agent;
        private Animator _animator;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int IsArmed = Animator.StringToHash("IsArmed");

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = patrolSpeed;
            _start = transform.position;
            _animator = GetComponent<Animator>();
            _animator.SetBool(IsArmed,true);
            InvokeRepeating(nameof(Patrol), 1.0f, 5.0f);
        }

        private void Patrol()
        {
            float patrolDistance = patrolSpeed * 5.0f;
            Vector3 destination;
            if (IsPatrolling)
            {
                destination = _start + new Vector3(Random.Range(-patrolDistance, patrolDistance), 0,
                    Random.Range(-patrolDistance, patrolDistance));
                ChangeDestination(destination);
            }
            else
            {
                destination = SceneManagement.SceneManager.Instance.player.transform.position;
            }
        }

        public void Stop()
        {
            ChangeDestination(transform.position);
        }
        private void ChangeDestination(Vector3 destination)
        {
            _agent.destination = destination;
        }

        private void Update()
        {
            _animator.SetFloat(Velocity, _agent.velocity.magnitude );
        }
    }
}