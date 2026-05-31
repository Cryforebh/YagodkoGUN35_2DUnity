using Netologia.Behaviours;
using Netologia.TowerDefence;
using Netologia.TowerDefence.Behaviors;
using UnityEngine;
using Zenject;

namespace Netologia.Systems
{
    public class ProjectileSystem : GameObjectPoolContainer<Projectile>, Director.IManualUpdate
    {
        private EffectSystem _effects;      //injected

        [SerializeField, Min(0.01f)]
        private float _hitDistance = 0.3f;

        public void ManualUpdate()
        {
            foreach (var pool in this)
            {
                foreach (var projectile in pool)
                {
                    //if (projectile.TargetID <= -1)
                    //    continue;

                    Vector3 currentPosition = projectile.transform.position;
                    Vector3 targetPosition = projectile.TargetPosition;

                    Vector3 direction = (targetPosition - currentPosition).normalized;
                    float distanceSquared = (targetPosition - currentPosition).sqrMagnitude;

                    if (distanceSquared <= _hitDistance)
                    {
                        projectile.DealDamage();
                        pool.ReturnElement(projectile);
                        continue;
                    }

                    Vector3 newPosition = currentPosition + direction * projectile.MoveSpeed * Time.deltaTime;
                    projectile.transform.position = newPosition;
                }
                
            }
        }

        public void OnDespawnUnit(int unitID)
        {
            foreach (var pool in this)
                foreach (var projectile in pool)
                    if (projectile.TargetID == unitID)
                        projectile.ResetTarget();
        }

        [Inject]
        private void Construct(EffectSystem effects)
        {
            (_effects) = (effects);
            //SqrtMagnitude optimization
            _hitDistance *= _hitDistance;
        }
    }
}