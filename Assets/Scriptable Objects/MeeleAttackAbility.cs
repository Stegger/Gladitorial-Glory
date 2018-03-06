using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Meelee Attack")]
public class MeeleAttackAbility : Ability
{
    public float damage;

    private Collider2D attackRange;
    private GameObject owner;

    public override void Initialize(GameObject obj)
    {
        this.owner = obj;
        attackRange = obj.GetComponent<Collider2D>();
    }

    public override void TriggerAbility()
    {
        Debug.Log("Attacking for " + damage + "dmg");
    }
}
