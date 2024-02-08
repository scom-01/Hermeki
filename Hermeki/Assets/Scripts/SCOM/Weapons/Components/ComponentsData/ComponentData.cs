using System;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class ComponentData
    {
        [SerializeField, HideInInspector] private string name;

        public Type ComponentDependency { get; protected set; }
        public ComponentData()
        {
            SetComponentName();
        }
        public void SetComponentName() => name = GetType().Name;

        public virtual void SetActionDataNames() { }

        public virtual void InitializeActionData(int numberOfActions) { }
    }

    [Serializable]
    public class ComponentData<T> : ComponentData where T : ActionData
    {
        [SerializeField] private T[] actionData;
        //[SerializeField] private T[] airActionData;
        public T[] ActionData { get => actionData; private set => actionData = value; }
        //public T[] InAirActionData { get => airActionData; private set => airActionData = value; }
        public override void SetActionDataNames()
        {
            base.SetActionDataNames();
            for (var i = 0; i < ActionData.Length; i++)
            {
                ActionData[i].SetAttackName(i + 1);
            }
            //for (var i = 0; i < InAirActionData.Length; i++)
            //{
            //    InAirActionData[i].SetAttackName("InAir",i + 1);
            //}
        }

        public override void InitializeActionData(int numberOfActions)
        {
            base.InitializeActionData(numberOfActions);

            var oldLen = ActionData != null ? actionData.Length : 0;
            //var oldLenAir = InAirActionData != null ? airActionData.Length : 0;

            if (oldLen != numberOfActions)
            {
                Array.Resize(ref actionData, numberOfActions);

                if (oldLen < numberOfActions)
                {
                    for (var i = oldLen; i < actionData.Length; i++)
                    {
                        var newObj = Activator.CreateInstance(typeof(T)) as T;
                        actionData[i] = newObj;
                    }
                }
                SetActionDataNames();
            }

            //if (oldLenAir != numberOfActions)
            //{
            //    Array.Resize(ref airActionData, numberOfActions);

            //    if (oldLenAir < numberOfActions)
            //    {
            //        for (var i = oldLenAir; i < airActionData.Length; i++)
            //        {
            //            var newObj = Activator.CreateInstance(typeof(T)) as T;
            //            airActionData[i] = newObj;
            //        }
            //    }
            //    SetActionDataNames();
            //}
        }
    }
}

