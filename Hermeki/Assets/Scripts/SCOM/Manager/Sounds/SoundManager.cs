using SCOM;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst
    {
        get
        {
            if (_Inst == null)
            {
                _Inst = FindObjectOfType(typeof(SoundManager)) as SoundManager;
                if (_Inst == null)
                {
                    Debug.Log("no Singleton obj");
                }
                else
                {
                    DontDestroyOnLoad(_Inst.gameObject);
                }
            }
            return _Inst;
        }
    }
    private static SoundManager _Inst = null;


    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    [SerializeField] AudioMixerGroup[] MixerGroup;
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (_Inst)
        {
            var managers = Resources.FindObjectsOfTypeAll(typeof(SoundManager));
            for (int i = 0; i < managers.Length; i++)
            {
                Debug.Log($"{managers[i]} = {i}");
                if (i > 0)
                {
                    Destroy(managers[i].GameObject());
                }
            }
            return;
        }

        _Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                _audioSources[i].outputAudioMixerGroup = MixerGroup[i];
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Sound.BGM].loop = true;
        }

    }
    public void Play(AudioData data, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (data.Clip == null)
            return;

        if (type == Sound.BGM) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = data.Clip;
            audioSource.volume = data.Volume;
            audioSource.Play();
        }
        else // Effect 효과음 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.volume = data.Volume;
            audioSource.PlayOneShot(data.Clip);
        }
    }
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Sound.BGM) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // Effect 효과음 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }
}
