using UnityEngine;

namespace SCOM.CoreSystem
{
    public class CoreComponent : MonoBehaviour, ILogicUpdate
    {
        protected Core core
        {
            get
            {
                if (_core == null)
                {
                    _core = transform.parent.GetComponent<Core>();
                }
                return _core;
            }
            set
            {
                _core = value;
            }
        }

        private Core _core;

        protected virtual void Awake()
        {
            core = transform.parent.GetComponent<Core>();

            if (core == null)
            {
                Debug.LogError("There is no Core on the parent");
            }

            core.AddComponent(this);
        }

        public virtual void LogicUpdate() { }
    }
}
