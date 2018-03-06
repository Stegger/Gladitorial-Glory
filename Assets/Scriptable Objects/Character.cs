using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Add character type")]
public class Character : ScriptableObject {

    public string cName;
    public int health;
    public RuntimeAnimatorController animator;

    [Range(1,3)]
    public int walkSpeed;

    [Range(2,5)]
    public int runSpeed;
    
}