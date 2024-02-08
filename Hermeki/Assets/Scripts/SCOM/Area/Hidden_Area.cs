using UnityEngine;
using UnityEngine.Tilemaps;

public class Hidden_Area : TouchObject
{
    private TilemapRenderer[] TRs;

    private void Awake()
    {
        TRs = this.GetComponentsInChildren<TilemapRenderer>();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        foreach (var TR in TRs)
        {
            TR.enabled = false;
        }
    }
    public override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        foreach (var TR in TRs)
        {
            TR.enabled = true;
        }        
    }
}
