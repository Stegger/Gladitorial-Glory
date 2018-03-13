using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Meelee Attack")]
public class MeeleAttackAbility : Ability
{
    public int damage;
    public int targetsToHit;
    
    private GameObject owner;

    public override void Initialize(GameObject obj)
    {
        owner = obj;
    }

    public override void TriggerAbility()
    {
        Debug.Log("Meele ability triggered.");
    }
}
