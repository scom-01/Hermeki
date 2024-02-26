using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SCOM
{
    public class SoundPooling : MonoBehaviour
    {
        public AudioData SoundObject;
        [HideInInspector]
        public AudioSource SoundSource
        {
            get
            {
                if (audio == null)
                {
                    audio = this.GetComponent<AudioSource>();
                    if (audio == null)
                    {
                        audio = this.AddComponent<AudioSource>();
                    }
                }
                return audio;
            }
        }
        private AudioSource audio = new();

        public AudioSource CreateObject(AudioData _SFX)
        {
            var source = this.AddComponent<AudioSource>();
            source.clip = _SFX.Clip;
            source.volume = _SFX.Volume;
            source.playOnAwake = false;
            //source.outputAudioMixerGroup = DataManager.Inst.SFX;
            source.loop = false;
            source.Stop();
            return source;
        }

        public void Init(AudioData _SFX)
        {
            if (_SFX.Clip == null)
                return;

            SoundObject = _SFX;
            //SoundSource.outputAudioMixerGroup = DataManager.Inst.SFX;
            SoundSource.clip = _SFX.Clip;
            SoundSource.volume= _SFX.Volume;
            SoundSource.playOnAwake = false;
            SoundSource.loop = false;
        }

        public AudioSource GetObejct(float volume)
        {
            SoundSource.PlayOneShot(SoundObject.Clip, volume);
            return SoundSource;
            //if (ObjectQueue.Count > 0)
            //{
            //    var obj = ObjectQueue.Dequeue();
            //    if (obj != null)
            //    {
            //        obj.volume = volume;
            //        obj.Play();
            //        StartCoroutine(ReturnObject(obj));
            //    }
            //    return obj;
            //}
            //else
            //{
            //    var newobj = CreateObject(SoundObject);
            //    newobj.volume = volume;
            //    newobj.Play();
            //    StartCoroutine(ReturnObject(newobj));
            //    return newobj;
            //}
        }

        //public IEnumerator ReturnObject(AudioSource source)
        //{
        //    yield return new WaitForSeconds(source.clip.length);

        //    if (ObjectQueue.Count >= MaxPoolAmount)
        //    {
        //        Destroy(source);
        //    }
        //    else
        //    {
        //        source.Stop();
        //        source.enabled = false;
        //        ObjectQueue.Enqueue(source);
        //    }
        //    yield return null;
        //}
    }
}
