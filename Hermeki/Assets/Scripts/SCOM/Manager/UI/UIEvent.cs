using UnityEngine;

namespace SCOM
{
    public class UIEvent : MonoBehaviour
    {
        protected UI.UIManager manager;
        protected virtual void Awake()
        {
            manager = this.GetComponentInParent<UI.UIManager>();
        }
    }
}
