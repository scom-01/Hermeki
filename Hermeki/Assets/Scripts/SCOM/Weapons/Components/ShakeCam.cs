using SCOM.Weapons.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class ShakeCam : WeaponComponent<ShakeCamData, ActionShakeCam>
    {
        private int currentShakeCaIdx;       

        protected override void HandleEnter()
        {
            base.HandleEnter();
            currentShakeCaIdx = 0;
        }
        public void HandleShakeCam()
        {
            if (Camera.main?.GetComponent<CameraShake>() == null)
            {
                Debug.LogWarning("The main camera does not have the CameraShake component.");
                return;
            }

            CheckCamAction(currentActionData);
            currentShakeCaIdx++;
        }

        public bool CheckCamAction(ActionShakeCam actionShakeCam)
        {
            if (actionShakeCam == null)
            {
                Debug.LogWarning("ActionShakeCam is null");
                return false;
            }
            var currCamAction = actionShakeCam.ShakeCamData;
            if (currCamAction.Length <= 0)
                return false;


            if (currentShakeCaIdx >= currCamAction.Length)
            {
                return false;
            }
            Debug.Log("Shake Cam");
            Camera.main.GetComponent<CameraShake>().Shake(
                currCamAction[currentShakeCaIdx].RepeatRate,
                currCamAction[currentShakeCaIdx].Range,
                currCamAction[currentShakeCaIdx].Duration
                );
            return true;
        }
        protected override void Start()
        {
            base.Start();

            eventHandler.OnShakeCam -= HandleShakeCam;
            eventHandler.OnShakeCam += HandleShakeCam;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            eventHandler.OnShakeCam -= HandleShakeCam;
        }
    }
}
