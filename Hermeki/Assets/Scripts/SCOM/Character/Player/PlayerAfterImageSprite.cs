using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField]
    [Tooltip("잔상 지속시간")]
    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField]
    private float alphaSet = 0.8f;

    [SerializeField]
    private Vector3 colorRGB;

    [SerializeField]
    private float alphaDecay = 10.0f;

    private Transform player;
    private SpriteRenderer SR;
    private SpriteRenderer playerSR;

    private Color color;

    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        if (GameObject.FindGameObjectWithTag("Player") == null)
            return;

        player = GameObject.FindGameObjectWithTag("Player").transform ?? null;
        if (player == null)
            return;

        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha -= alphaDecay * Time.deltaTime;
        color = new Color(Mathf.Clamp01(colorRGB.x), 
                            Mathf.Clamp01(colorRGB.y), 
                            Mathf.Clamp01(colorRGB.z), alpha);

        SR.color = color;
        if(Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
