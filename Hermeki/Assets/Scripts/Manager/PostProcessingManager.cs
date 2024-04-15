using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    Volume volume;
    VolumeProfile volumeProfile;
    private void Awake()
    {
        volume = this.GetComponent<Volume>();
        volumeProfile = volume.profile;
    }

    public void SetVignette(float _value, float goal)
    {
        Vignette vignette;
        volumeProfile.TryGet(out vignette);
        if (vignette != null)
        {
            vignette.active = true;
            StartCoroutine(SetVignetteValue(_value, goal));
        }

    }

    IEnumerator SetVignetteValue(float _value, float goal)
    {
        Vignette vignette;
        volumeProfile.TryGet(out vignette);
        if (vignette != null)
        {
            vignette.intensity.value = _value;
        }
        while (vignette.intensity.value != goal)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, goal, Time.deltaTime);
            if (Mathf.Abs(vignette.intensity.value - goal) < 0.05f)
            {
                vignette.intensity.value = goal;
            }
            yield return null;
        }
        vignette.active = false;
    }
}
