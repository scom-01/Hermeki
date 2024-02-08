using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCOM.CoreSystem
{
    public class Core : MonoBehaviour
    {
        private readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();

        public CollisionSenses CoreCollisionSenses
        {
            get
            {
                if (collisionSenses == null)
                {
                    collisionSenses = GetCoreComponent<CollisionSenses>();
                }
                return collisionSenses;
            }
        }
        private CollisionSenses collisionSenses;

        public DamageReceiver CoreDamageReceiver
        {
            get
            {
                if (damageReceiver == null)
                {
                    damageReceiver = GetCoreComponent<DamageReceiver>();
                }
                return damageReceiver;
            }
        }
        private DamageReceiver damageReceiver;
        public DamageTransmitter CoreDamageTransmitter
        {
            get
            {
                if (damageTransmitter == null)
                {
                    damageTransmitter = GetCoreComponent<DamageTransmitter>();
                }
                return damageTransmitter;
            }
        }
        private DamageTransmitter damageTransmitter;

        public Death CoreDeath
        {
            get
            {
                if (death == null)
                {
                    death = GetCoreComponent<Death>();
                }
                return death;
            }
        }
        private Death death;

        public EffectManager CoreEffectManager
        {
            get
            {
                if (effectManager == null)
                {
                    effectManager = GetCoreComponent<EffectManager>();
                }
                return effectManager;
            }
        }
        private EffectManager effectManager;

        public KnockBackReceiver CoreKnockBackReceiver
        {
            get
            {
                if (knockBackReceiver == null)
                {
                    knockBackReceiver = GetCoreComponent<KnockBackReceiver>();
                }
                return knockBackReceiver;
            }
        }
        private KnockBackReceiver knockBackReceiver;

        public Movement CoreMovement
        {
            get
            {
                if (movement == null)
                {
                    movement = GetCoreComponent<Movement>();
                }
                return movement;
            }
        }
        private Movement movement;

        public SoundEffect CoreSoundEffect
        {
            get
            {
                if (soundEffect == null)
                {
                    soundEffect = GetCoreComponent<SoundEffect>();
                }
                return soundEffect;
            }
        }
        private SoundEffect soundEffect;

        public UnitStats CoreUnitStats
        {
            get
            {
                if (unitStats == null)
                {
                    unitStats = GetCoreComponent<UnitStats>();
                }
                return unitStats;
            }
        }
        private UnitStats unitStats;


        public void LogicUpdate()
        {
            foreach (CoreComponent component in CoreComponents)
            {
                component.LogicUpdate();
            }
        }

        public void AddComponent(CoreComponent component)
        {
            if (!CoreComponents.Contains(component))
            {
                CoreComponents.Add(component);
            }
        }

        public T GetCoreComponent<T>() where T : CoreComponent
        {
            var comp = CoreComponents.OfType<T>().FirstOrDefault();

            if (comp)
                return comp;

            comp = GetComponentInChildren<T>();

            if (comp)
                return comp;

            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
            return null;
        }

        public T GetCoreComponent<T>(ref T value) where T : CoreComponent
        {
            value = GetCoreComponent<T>();
            return value;
        }

        public Unit Unit
        {
            get
            {
                if (unit == null)
                {
                    unit = GetComponentInParent<Unit>();
                }
                return unit;
            }
            //=> GenericNotImplementedError<Unit>.TryGet(unit, transform.parent.name);
            private set => unit = value;
        }
        private Unit unit;
        public virtual void Awake()
        {
            unit = GetComponentInParent<Unit>();
        }
    }
}