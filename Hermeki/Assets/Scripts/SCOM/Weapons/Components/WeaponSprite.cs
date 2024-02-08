using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using SCOM.Weapons.Components;
public class WeaponSprite : WeaponComponent<WeaponSpriteData, ActionSprites>
{
    private SpriteRenderer baseSpriteRenderer;
    private SpriteRenderer weaponSpriteRenderer;
    private GameObject baseObject;

    private int currentActionSpriteIndex;
    protected override void HandleEnter()
    {
        base.HandleEnter();
        currentActionSpriteIndex = 0;
    }

    private void HandleBaseSpriteChange(SpriteRenderer sr)
    {
        if (!isAttackActive)
        {
            weaponSpriteRenderer.sprite = null;
            return;
        }

        if (currentActionData != null)
        {
            CheckAttackAction(currentActionData);
        }

        //if (currentActionData !=null && currentAirActionData !=null)
        //{
        //    if (weapon.InAir)
        //    {
        //        CheckAttackAction(currentAirActionData);
        //    }
        //    else
        //    {
        //        CheckAttackAction(currentActionData);
        //    }
        //}
        //else if(currentActionData == null)
        //{
        //    CheckAttackAction(currentAirActionData);
        //}
        //else if(currentAirActionData == null)
        //{
        //    CheckAttackAction(currentActionData);
        //}
        currentActionSpriteIndex++;
    }
    private void CheckAttackAction(ActionSprites actionSprites)
    {
        if (actionSprites == null)
            return;

        Sprite[] currentAttackSprite = new Sprite[0];

        var currSprites = actionSprites.WeaponSprites;
        if (currSprites.Length <= 0)
            return;


        if (currentActionData.WeaponSprites.Length > 0)
            currentAttackSprite = currSprites;

        if (currentAttackSprite.Length < 0)
            return;

        if (currentActionSpriteIndex >= currentAttackSprite.Length)
        {
            Debug.Log($"{Weapon.name} weapon sprite length mismatch");
            return;
        }

        weaponSpriteRenderer.sprite = currentAttackSprite[currentActionSpriteIndex];
    }

    protected override void Start()
    {
        base.Start();
        //baseObject = transform.Find("Base").gameObject;
        //baseSpriteRenderer = transform.Find("Base").GetComponent<SpriteRenderer>();
        //weaponSpriteRenderer = transform.Find("Weapon").GetComponent<SpriteRenderer>();

        baseSpriteRenderer = Weapon.BaseGameObject.GetComponent<SpriteRenderer>();
        weaponSpriteRenderer = Weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();
        baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
    }
}
