using UnityEngine;

namespace SCOM.CoreSystem
{
    public class Death : CoreComponent
    {
        [SerializeField]
        protected GameObject[] deathChunk;
        protected EffectManager EffectManager
        {
            get
            {
                if(effectManager == null)
                {
                    core.GetCoreComponent(ref effectManager);
                }
                return effectManager;
            }
        }
            
        private EffectManager effectManager;


        protected UnitStats Stats => stats ? stats : core.GetCoreComponent(ref stats);
        private UnitStats stats;

        [HideInInspector] public bool isDead = false;
        
        public virtual void Die()
        {
            if (isDead)
                return;
            core.Unit.IsAlive = false;
            isDead = true;
            foreach (var effect in deathChunk)
            {
                var particleObject = EffectManager.StartEffectsWithRandomPos(effect, 0.5f, Vector3.one, core.Unit.transform.position);
                particleObject.GetComponent<Animator>().speed = Random.Range(0.3f, 1f);
            }
            
            core.Unit.DieEffect();
        }

        protected void OnEnable()
        {
            Stats.OnHealthZero += Die;
        }

        protected void OnDisable()
        {
            Stats.OnHealthZero -= Die;
        }
    }
}
