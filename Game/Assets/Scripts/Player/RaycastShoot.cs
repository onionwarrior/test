using System.Collections;
using Enemy;
using UnityEngine;
namespace Player
{
    public class Combat : MonoBehaviour
    {
        private bool _isShooting = false;
        private bool _isArmed = false;
        private  IEnumerator ShootCoroutine(Animator animator,AudioSource audio,BotsAI enemy)
        {
            if (!_isArmed)
            {
                _isArmed = true;
                animator.Play("Unsheathe");
                yield return new WaitForSeconds(2f);
            }
            _isShooting = true; 
            animator.Play("Shoot");
            yield return new WaitForSeconds(1.5f);
            audio.Play();
            yield return new WaitForSeconds(.5f);
            enemy.TakeDamage(40);
            animator.Play("Stop");
            _isShooting = false;
        }

        public void RaycastShoot(AudioSource audio, Camera camera, float maximumShootDistance,
            float maximumHitDisctance, Transform attacker, Animator animator,GameObject target)
        {
            if (Vector3.Distance(attacker.position, target.transform.position) <= maximumShootDistance)
            {
                var position = attacker.position;
                Ray shootRay = new Ray(position, target.transform.position - position);
                if(Physics.Raycast(shootRay,out RaycastHit enemyHitInfo, maximumShootDistance,~(1<<14)))
                {
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
