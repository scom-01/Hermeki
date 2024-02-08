using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class SoundClip : WeaponComponent<SoundClipData, ActionSoundClip>
    {
        private ActionHitBox hitBox;
        private int currentSoundIndex;
        protected override void HandleEnter()
        {
            base.HandleEnter();
            currentSoundIndex = 0;
        }
        public void HandleSoundClip()
        {
            if(currentActionData !=null)
            {
                CheckSoundClipAction(currentActionData);
            }

            currentSoundIndex++;
        }

        public void DetectedSoundClip(Collider2D[] coll)
        {
            if (coll == null)
                return;

            foreach(var item in coll)
            {
                if (item.gameObject.tag == this.gameObject.tag)
                    continue;
                
            }
        }
        private void CheckSoundClipAction(ActionSoundClip actionSoundClip)
        {
            if (actionSoundClip == null)
                return;

            var currSoundClips = actionSoundClip.audioDataList;
            if (currSoundClips == null)
                return;

            if (currSoundClips.Length <= 0)
                return;

            if (currentSoundIndex >= currSoundClips.Length)
            {
                Debug.LogWarning(currSoundClips + "[" + currentSoundIndex + "].audioClips is Null");
                return;
            }

            CoreSoundEffect.AudioSpawn(currSoundClips[currentSoundIndex]);
        }
        protected override void Awake()
        {
            base.Awake();
            hitBox = GetComponent<ActionHitBox>();
            //hitBox.OnDetectedCollider2D -= DetectedSoundClip;
            //hitBox.OnDetectedCollider2D += DetectedSoundClip;
        }

        protected override void Start()
        {
            base.Start();
            eventHandler.OnSoundClip -= HandleSoundClip;
            eventHandler.OnSoundClip += HandleSoundClip;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            //hitBox.OnDetectedCollider2D -= DetectedSoundClip;
            eventHandler.OnSoundClip -= HandleSoundClip;
        }
    }
}
