using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class SoundClipData : ComponentData<ActionSoundClip>
    {
        public SoundClipData()
        {
            ComponentDependency = typeof(SoundClip);
        }
    }
}
