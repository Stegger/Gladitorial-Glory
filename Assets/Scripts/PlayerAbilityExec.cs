using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAbilityExec : NetworkBehaviour
{
    public Collider2D meeleHurtbox;
    public int damage;

    private Collider2D[] targets;
    private PlayerMovement pmOther;
    private ContactFilter2D contactFilter2D;

    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
    }

    private void Start()
    {
        targets = new Collider2D[4];
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
            CmdAttackMeele();
        }
    }

    [Command]
    private void CmdAttackMeele()
    {
        int c = meeleHurtbox.OverlapCollider(contactFilter2D, targets);
        for (int i = 0; i < c; i++)
        {
            if (targets[i].gameObject != gameObject)
            {
                pmOther = targets[i].gameObject.GetComponent<PlayerMovement>();
                if (pmOther != null)
                    pmOther.CmdTakeDamage(damage);
            }
        }
        GetComponent<NetworkAnimator>().SetTrigger("Attack");
    }
}
