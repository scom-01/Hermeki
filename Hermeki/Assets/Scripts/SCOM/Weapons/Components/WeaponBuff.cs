namespace SCOM.Weapons.Components
{
    public class WeaponBuff : WeaponComponent<WeaponBuffData, ActionWeaponBuff>
    {
        public int currentWeaponBuffIndex = 0;
        protected override void HandleEnter()
        {
            base.HandleEnter();
            currentWeaponBuffIndex = 0;
        }
        private void HandleWeaponBuff()
        {
            if (currentActionData == null)
            {
                return;
            };            

            if (Buff.BuffSystemAddBuff(unit, currentActionData.BuffDataList[currentWeaponBuffIndex].Buff) == null)
                return;

            core.CoreSoundEffect.AudioSpawn(currentActionData.BuffDataList[currentWeaponBuffIndex].EquipAudio);
            currentWeaponBuffIndex++;
        }

        protected override void Start()
        {
            base.Start();

            eventHandler.OnWeaponBuff -= HandleWeaponBuff;
            eventHandler.OnWeaponBuff += HandleWeaponBuff;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            eventHandler.OnWeaponBuff -= HandleWeaponBuff;
        }
    }
}
