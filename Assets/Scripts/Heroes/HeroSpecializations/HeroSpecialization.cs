using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.Heroes.HeroSpecializations
{
    public abstract class HeroSpecialization : MonoBehaviour
    {
        protected abstract Ability Ability { get; }
        protected abstract Transform Target { get; }

        private void Update()
        {
            Ability.UpdateCooldown(Time.deltaTime);
            UpdateAnimations();
        }

        public bool TryGetTarget(out Transform target)
        {
            target = Target;

            if (target == null)
                return false;

            return true;
        }

        public abstract void Use();
        protected abstract void UpdateAnimations();
    }
}
