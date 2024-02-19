using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLayerMaskSO", menuName = "Data/LayerMask/LayerMaskSO Data")]
public class LayerMaskSO : ScriptableObject
{
    public LayerMask WhatIsGround = LayerMask.NameToLayer("Ground");
    public LayerMask WhatIsWall;
    public LayerMask WhatIsPlatform;
    [Tooltip("적으로 인지하는 Object LayerMask")]
    public LayerMask WhatIsEnemyUnit;
}
