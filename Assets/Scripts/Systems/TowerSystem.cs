using Netologia.Behaviours;
using Netologia.TowerDefence;
using Netologia.TowerDefence.Behaviors;
using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

namespace Netologia.Systems
{
	public class TowerSystem : GameObjectPoolContainer<Tower>, Director.IManualUpdate
	{
		private UnitSystem _units;				//injected
		private ProjectileSystem _projectiles;	//injected

		public void ManualUpdate()
		{
            foreach (var pair in this)
                foreach (var tower in pair)
				{
					if (tower.DecrementAttackReload(Time.deltaTime))
                        continue;

                    if (!tower.HasTarget || !IsTargetValid(tower, tower.Target, tower.Range))
                    {
                        var target = _units.FindTarget(tower.transform.position, tower.Range);
                        if (target != null)
                        {
                            tower.Target = target;
                            Debug.Log($"{tower.name} выбрал новую цель: {target.name}");
                        }
                        else
                        {
                            // Если цель не найдена, пропускаем атаку
                            continue;
                        }
                    }

                    // Проверяем, что цель всё ещё валидна (в зоне досягаемости и жива)
                    if (!IsTargetValid(tower, tower.Target, tower.Range))
                    {
                        // Если цель стала некорректной, сбрасываем её и пропускаем атаку
                        tower.Target = null;
                        continue;
                    }

                    // Подготавливаем снаряд только если цель валидна
                    var projectile = _projectiles[tower.Projectile].Get;
                    projectile.PrepareData(tower.transform.position, tower.Target, tower.Damage, tower.AttackElemental);

                    // Выполняем атаку
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