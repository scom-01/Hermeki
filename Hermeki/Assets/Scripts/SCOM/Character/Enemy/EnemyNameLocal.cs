using TMPro;
using UnityEngine;

public class EnemyNameLocal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Nametext;
    //[SerializeField] private LocalizeStringEvent LocalStringName;
    private Unit unit;
    // Start is called before the first frame update
    void Start()
    {
        unit = this.GetComponentInParent<Unit>();
        if(Nametext==null)
            Nametext = this.GetComponent<TextMeshProUGUI>();
        //LocalStringName = Nametext.gameObject.GetComponent<LocalizeStringEvent>();
        Rendering(unit);
    }

    private void Rendering(Unit unit)
    {
        if (unit == null)
            return;

        Nametext.text = unit.UnitData.UnitName;
        //LocalStringName.StringReference = unit.UnitData.UnitNameLocal;
    }
}
