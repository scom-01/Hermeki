using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace SCOM.CoreSystem
{    
    public class SoundEffect : CoreComponent
    {
        private SoundContainer SFX_soundContainer;
        protected override void Awake()
        {
            base.Awake();
            if (GameManager.Inst?.StageManager?.SFXContainer != null)
                SFX_soundContainer = GameManager.Inst.StageManager.SFXContainer;
        }
        public void AudioSpawn(AudioData audioData)
        {
            if (audioData.Clip == null)
            {
                Debug.LogWarning("Clip is Null");
                return;
            }
            SoundManager.Inst?.Play(audioData, Sound.Effect);

            //SFX_soundContainer?.CheckObject(audioData).GetObejct(audioData.Volume);
        }
        public void AudioSpawn(AudioClip audioClip, float volume = 1f)
        {
            if (audioClip == null)
            {
                Debug.LogWarning("Clip is Null");
                return;
            }
            SoundManager.Inst?.Play(new AudioData(audioClip, volume), Sound.Effect);
            //SFX_soundContainer?.CheckObject(new AudioData(audioClip, volume)).GetObejct(volume);
        }
    }
}
