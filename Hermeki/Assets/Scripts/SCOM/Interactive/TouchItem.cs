using Cinemachine;
using System.Linq;
using UnityEngine;


public class TouchItem : TouchObject
{
    [TagField]
    public string[] Ignore_Tag;

    [SerializeField] private StatsItemSO StatsItem;
    [SerializeField] private BuffItemSO BuffItem;
    private SpriteRenderer SR;
    private void Awake()
    {
        this.gameObject.layer = LayerMask.NameToLayer("TouchObject");

        if (SR == null)
        {
            SR = this.GetComponent<SpriteRenderer>();
            if (SR == null)
            {
                SR = this.GetComponentInParent<SpriteRenderer>();
            }
        }

        if (BuffItem != null)
            SR.sprite = BuffItem.itemData.ItemSprite;

        if (StatsItem != null)
            SR.sprite = StatsItem.itemData.ItemSprite;
    }
    public override void Touch(GameObject obj)
    {
        base.Touch(obj);
        if (BuffItem == null)
        {
            Debug.LogWarning("itemData is null");
            return;
        }

        if (Buff.BuffSystemAddBuff(obj.GetComponent<Unit>(), BuffItem) != null)
        {
            //Vfx
            if (BuffItem.InitEffectData.AcquiredEffectPrefab != null)
                gameObject.GetComponent<Unit>().Core.CoreEffectManager.StartEffects(BuffItem.InitEffectData.AcquiredEffectPrefab, this.gameObject.transform.position, Quaternion.identity, Vector3.one);

            //Sfx
            if (BuffItem.InitEffectData.AcquiredSFX.Clip != null)
                gameObject.GetComponent<Unit>().Core.CoreSoundEffect.AudioSpawn(BuffItem.InitEffectData.AcquiredSFX);

            Destroy(SR.gameObject);
        }
    }

    public override void UnTouch(GameObject obj)
    {
        base.UnTouch(obj);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (Ignore_Tag.Contains(collision.tag))
            return;

        base.OnTriggerEnter2D(collision);

        if (!collision.gameObject.GetComponent<BuffSystem>())
            return;

        Touch(collision.gameObject);
    }
}
