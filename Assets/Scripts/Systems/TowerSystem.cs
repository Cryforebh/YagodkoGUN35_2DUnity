using Netologia.Behaviours;
using Netologia.TowerDefence;
using Netologia.TowerDefence.Behaviors;
using UnityEngine;
using Zenject;

namespace Netologia.Systems
{
    public class TowerSystem : GameObjectPoolContainer<Tower>, Director.IManualUpdate
    {
        private UnitSystem _units;              //injected
        private ProjectileSystem _projectiles;  //injected

        public void ManualUpdate()
        {
            var time = TimeManager.DeltaTime;
            foreach (var pair in this)
                foreach (var tower in pair)
                {
                    if (tower.DecrementAttackReload(time))
                        continue;

                    // Поиск цели
                    if (!tower.HasTarget || !IsTargetValid(tower, tower.Target, tower.Range))
                    {
                        var target = _units.FindTarget(tower.transform.position, tower.Range);
                        if (target != null)
                        {
                            tower.Target = target;
                            Debug.Log($"{tower.name} выбрал новую цель: {target.name}");
                        }
                        // Если цель не найдена, пропускаем атаку
                        else
                            continue;
                    }

                    // Проверяем, что цель в зоне досягаемости и жива
                    if (!IsTargetValid(tower, tower.Target, tower.Range))
                    {
                        // Если цель стала некорректной, сбрасываем ее и пропускаем атаку
                        tower.Target = null;
                        continue;
                    }

                    // Подготавливаем снаряд только если цель валидна
                    var projectile = _projectiles[tower.Projectile].Get;
                    projectile.PrepareData(tower.transform.position, tower.Target, tower.Damage, tower.AttackElemental);

                    tower.Attack();
                    Debug.Log($"{tower.name} атакует {tower.Target.name}");
                }
        }

        private bool IsTargetValid(Tower tower, Unit target, float range)
        {
            if (target == null || target.CurrentHealth <= 0)
                return false;

            float distance = Vector3.Distance(tower.transform.position, target.transform.position);
            return distance <= range;
        }

        public void OnDespawnUnit(int unitID)
        {
            foreach (var pair in this)
                foreach (var tower in pair)
                    if (tower.TargetID == unitID)
                        tower.Target = null;
        }

        [Inject]
        private void Construct(UnitSystem units, ProjectileSystem projectiles)
        {
            _units = units;
            _projectiles = projectiles;
        }
    }
}