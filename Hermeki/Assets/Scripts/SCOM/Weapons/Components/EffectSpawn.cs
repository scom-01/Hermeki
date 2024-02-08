using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class EffectSpawn : WeaponComponent<EffectSpawnData, ActionEffect>
    {

        private int currentEffectSpawnIndex;
        protected override void HandleEnter()
        {
            base.HandleEnter();
            currentEffectSpawnIndex = 0;
        }

        private void HandleEffectSpawn()
        {
            if (currentActionData != null)
            {
                CheckEffectAction(currentActionData);
            }

            currentEffectSpawnIndex++;
        }

        private void CheckEffectAction(ActionEffect actionParticle)
        {
            if (actionParticle == null)
                return;

            var currParticles = actionParticle.EffectParticles;
            if (currParticles.Length <= 0)
                return;


            if (currentEffectSpawnIndex >= currParticles.Length)
            {
                Debug.Log($"{Weapon.name} Particle Prefabs length mismatch");
                return;
            }
            currParticles[currentEffectSpawnIndex].SpawnObject(unit);
        }
        protected override void Start()
        {
            base.Start();

            eventHandler.OnEffectSpawn -= HandleEffectSpawn;
            eventHandler.OnEffectSpawn += HandleEffectSpawn;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            eventHandler.OnEffectSpawn -= HandleEffectSpawn;
        }

    }
}
