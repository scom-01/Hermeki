using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FixedCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject MainChar;
    // Start is called before the first frame update
    void Start()
    {

    }
        
    // Update is called once per frame
    void Update()
    {
        if(MainChar != null)
        {
            this.transform.position = new Vector3(MainChar.transform.position.x, MainChar.transform.position.y,-10);  
        }
    }
}
