using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AttackData", menuName = "Attack Data", order = 51)]
public class AttackData : ScriptableObject
{
    [SerializeField] private string attackName;
    [SerializeField] private int startUpFrames;
    [SerializeField] private int activeFrames;
    [SerializeField] private int recoveryFrames;
    [SerializeField] private int cancelAdv;
    [SerializeField] private int hitAdv;
    [SerializeField] private int damage;
    [SerializeField] private AttackEnum attackHitBox;

    public string AttackName
    {
        get { return attackName; }
    }

    public int StartUpFrames
    {
        get { return startUpFrames; }
    }

    public int ActiveFrames
    {
        get { return activeFrames; }
    }

    public int RecoveryFrames
    {
        get { return recoveryFrames; }
    }

    public int CancelAdv
    {
        get { return cancelAdv; }
    }

    public int HitAdv
    {
        get { return hitAdv; }
    }

    public int Damage
    {
        get { return damage; }
    }

    public AttackEnum AttackHitBox
    {
        get { return attackHitBox; }
    }
}
