using TMPro;
using UnityEngine;


public class DmgTxtPooling : ObjectPooling
{
    public TextMeshProUGUI HitTextMeshPro
    {
        get
        {
            if (hitTextMeshPro == null)
            {
                hitTextMeshPro = this.GetComponent<TextMeshProUGUI>();
            }
            return hitTextMeshPro;
        }
        set => hitTextMeshPro = value;
    }

    private TextMeshProUGUI hitTextMeshPro;

    public float DamageAmount
    {
        get
        {
            return damageAmount;
        }
        set
        {
            damageAmount = value;
            if (HitTextMeshPro != null)
            {
                HitTextMeshPro.text = string.Format("{0:#,###}", damageAmount);
            }
            else
            {
                Debug.LogWarning("HitTextMeshPro is null");
            }
        }
    }
    public float FontSize
    {
        get
        {
            return fontSize;
        }
        set
        {
            fontSize = value;
            if (HitTextMeshPro != null)
            {
                HitTextMeshPro.fontSize = fontSize;
            }
            else
            {
                Debug.LogWarning("HitTextMeshPro is null");
            }
        }
    }
    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            if (HitTextMeshPro != null)
            {
                HitTextMeshPro.color = color;
            }
            else
            {
                Debug.LogWarning("HitTextMeshPro is null");
            }
        }
    }

    private float damageAmount;
    private float fontSize;
    private Color color;

    public void SetText(float damage, float fontsize, Color color)
    {
        this.DamageAmount = damage;
        this.FontSize = fontsize;
        this.Color = color;
    }

    public GameObject GetObejct(Vector3 pos, Quaternion quaternion,float damage, float fontSize, DAMAGE_ATT damageAttiribute)
    {
        if (ObjectQueue.Count > 0)
        {
            var obj = ObjectQueue.Dequeue();
            if (obj != null)
            {
                obj.GetComponent<RectTransform>().anchoredPosition = pos;
                switch (damageAttiribute)
                {
                    case DAMAGE_ATT.Magic:
                        obj.GetComponentInChildren<DamageText>().SetText(damage, fontSize, Color.magenta);
                        break;
                    case DAMAGE_ATT.Physics:
                        obj.GetComponentInChildren<DamageText>().SetText(damage, fontSize, Color.yellow);
                        break;
                    case DAMAGE_ATT.Fixed:
                        obj.GetComponentInChildren<DamageText>().SetText(damage, fontSize, Color.white);
                        break;
                    case DAMAGE_ATT.Heal:
                        obj.GetComponentInChildren<DamageText>().SetText(damage, fontSize, Color.green);
                        break;
                }
                obj.gameObject.SetActive(true);
            }
            return obj;
        }
        else
        {
            var newobj = CreateObject(Vector3.one);
            newobj.GetComponent<RectTransform>().anchoredPosition = pos;
            switch (damageAttiribute)
            {
                case DAMAGE_ATT.Magic:
                    newobj.GetComponentInChildren<DamageText>().SetText(damage, fontSize, Color.magenta);
                    break;
                case DAMAGE_ATT.Physics:
                    newobj.GetComponentInChildren<DamageText>().SetText(damage, fontSize, Color.yellow);
                    break;
                case DAMAGE_ATT.Fixed:
                    newobj.GetComponentInChildren<DamageText>().SetText(damage, fontSize, Color.white);
                    break;
            }
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }
}
