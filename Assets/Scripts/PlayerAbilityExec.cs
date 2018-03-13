using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAbilityExec : NetworkBehaviour
{
    public MeeleAttackAbility meeleAbility;

    public Collider2D meeleHurtbox;

    private Collider2D[] targets;
    private PlayerMovement pmOther;
    private ContactFilter2D contactFilter2D;

    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
        meeleAbility.Initialize(this.gameObject);
        targets = new Collider2D[meeleAbility.targetsToHit + 1];
        contactFilter2D = new ContactFilter2D();
        LayerMask lm = new LayerMask
        {
            value = 8
        };
        contactFilter2D.layerMask = lm;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int c = meeleHurtbox.OverlapCollider(contactFilter2D, targets);
            for (int i = 0; i < c; i++)
            {
                if (targets[i].gameObject != gameObject)
                {
                    pmOther = targets[i].gameObject.GetComponent<PlayerMovement>();
                    if (pmOther != null)
                        pmOther.CmdTakeDamage(meeleAbility.damage);
                }
            }
            GetComponent<NetworkAnimator>().SetTrigger("Attack");
        }
    }
 
}
