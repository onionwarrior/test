using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public interface IKillable
    {
        public void Die();
    }
    public interface IDamageable<T>
    {
        public void TakeDamage(T damage);
    }
    public interface IReactToDamage<T>:IDamageable<T>,IKillable
    {
        public void ReactToDamage(T damage);
    }
}