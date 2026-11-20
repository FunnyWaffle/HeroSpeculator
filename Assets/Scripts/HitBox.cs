using Assets.Scripts.Projectiles;
using UnityEngine;

namespace Assets.Scripts
{
    public class HitBox : MonoBehaviour
    {
        private IDamageable _damageable;

        public Allie _allie { get; private set; }

        public void Initialize(
            IDamageable damageable,
            Allie allie)
        {
            _damageable = damageable;
            _allie = allie;
        }

        public bool TryTakeHit(float damage, Allie allie)
        {
            if (_allie == allie)
                return false;

            _damageable.TakeDamage(damage);
            return true;
        }
    }
}
