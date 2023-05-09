using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlay : MonoBehaviour
{

    public void AuchDamage()
    {
        AudioManager.instance.PlaySFX(1);
    }
    public void SwordSwoosh()
    {
        AudioManager.instance.PlaySFX(3);
    }
}
