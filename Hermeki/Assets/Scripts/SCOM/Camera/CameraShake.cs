using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// 메소드 실행까지의 시간
    /// </summary>
    [field: SerializeField] public float time;
    /// <summary>
    /// 몇 초마다 실행시킬 것인가
    /// </summary>
    [field: SerializeField] public float repeatRate;
    /// <summary>
    /// 실행할 메소드 명
    /// </summary>
    [field: SerializeField] public float shakeRange = 0.5f;
    [field: SerializeField] public float duration;

    public Camera mainCamera;

    Vector3 cameraPos;
    float startTime;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            Shake(repeatRate, shakeRange, duration);
        }
    }

    public void Shake(float _repeatRate = 0.0f, float _shakeRange = 0.0f, float _duration = 0.1f)
    {
        startTime = GameManager.Inst.PlayTime;
        cameraPos = mainCamera.transform.position;
        shakeRange = _shakeRange;
        duration = _duration;
        InvokeRepeating("StartShake", 0f, _repeatRate);
        Invoke("StopShake", _duration);
    }
    void StartShake()
    {
        //경과시간
        float elapsed_time = (GameManager.Inst.PlayTime - startTime);
        float _Range = Random.value * Mathf.Sin(Mathf.PI * (elapsed_time / duration))+ shakeRange;
        float cameraPosX = _Range;
        float cameraPosY = _Range;
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCamera.transform.position = cameraPos;
    }
    void StopShake()
    {
        CancelInvoke("StartShake");
        mainCamera.transform.position = cameraPos;
    }
}