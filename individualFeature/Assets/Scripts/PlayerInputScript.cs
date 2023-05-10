using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] private float origSpeed = 5f;
    private float speed;
    [SerializeField] private float jump = 20f;

    [SerializeField] private GameObject hitBox;
    private HitBoxScript hitBoxScript;
 
    private bool noMoveInput = true;
    private bool noAttackInput = true;
    private bool canAttack = true;
    public bool isAttacking = false;

    private PlayerInputer pi;
    private Rigidbody rb;

    //newest input is last index
    private List<MoveEnum> moveHistory = new List<MoveEnum>();
    private List<AttackData> attackHistory = new List<AttackData>();

    [SerializeField] private AttackData[] attackList;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        pi = new PlayerInputer();
        hitBoxScript = hitBox.GetComponent<HitBoxScript>();
        pi.Enable();
        speed = origSpeed;

        for (int i = 1; i < attackList.Length; i++)
        {
            AttackData key = attackList[i];

            for (int j = i - 1; j >= 0; )
            {
                if (key.AttackName.CompareTo(attackList[j].AttackName) < 0)
                {
                    attackList[j + 1] = attackList[j];
                    j--;
                    attackList[j + 1] = key;
                }
                else
                {
                    break;
                }
            }
        }
    }

    private void Update()
    {
        CheckIsAttacking();
        GetInputs();
        CheckCanAttack();
    }

    private void CheckIsAttacking()
    {
        if (hitBoxScript.CurState != AttackStateEnum.NoAttack)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }    

    private void CheckCanAttack()
    {
        if (hitBoxScript.CurState == AttackStateEnum.NoAttack || hitBoxScript.CanCancel)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    private void GetInputs()
    {
        noMoveInput = true;
        noAttackInput = true;

        if (pi.Player.Crouch.ReadValue<float>() == 1)
        {
            speed = 0f;
            AddToInputHistory(MoveEnum.down);
            noMoveInput = false;
        }
        else if (isAttacking)
        {
            speed = 0f;
        }
        else
        {
            speed = origSpeed;
        }

        //very jank jump fix later
        if (pi.Player.Jump.ReadValue<float>() == 1)
        {
            rb.AddForce(Vector3.up * jump);
            AddToInputHistory(MoveEnum.up);
            noMoveInput = false;
        }

        if (pi.Player.Left.ReadValue<float>() == 1)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            AddToInputHistory(HorDirection("left"));
            noMoveInput = false;
        }
        if (pi.Player.Right.ReadValue<float>() == 1)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            AddToInputHistory(HorDirection("right"));
            noMoveInput = false;
        }

        if (noMoveInput)
        {
            AddToInputHistory(MoveEnum.noMove);
        }

        if (canAttack && pi.Player.FrontPunch.ReadValue<float>() == 1)
        {
            if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "StraightPunch") && hitBoxScript.CanCancel)
            {
                RegularAttack("TheDamned");
            }
            else if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "EternalVengeance") && hitBoxScript.CanCancel)
            {
                RegularAttack("Haunted");
            }
            else if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "SpecterStrike") && hitBoxScript.CanCancel)
            {
                RegularAttack("Banished");
            }
            else
            {
                if (CheckMoveHist(0, MoveEnum.back))
                {
                    RegularAttack("CutSlice");
                }
                else if (CheckMoveHist(0, MoveEnum.down))
                {
                    RegularAttack("LowJab");
                }
                else if (CheckMoveHist(0, MoveEnum.forward))
                {
                    if (CheckMoveHist(1, MoveEnum.back) || CheckMoveHist(2, MoveEnum.back) && CheckMoveHist(1, MoveEnum.noMove))
                    {
                        //special
                        RegularAttack("Spear");
                    }
                    else
                    {
                        RegularAttack("LowJab");
                    }
                }
                else
                {
                    RegularAttack("StraightPunch");
                }
            }
            noAttackInput = false;
        }
        else if (canAttack && pi.Player.BackPunch.ReadValue<float>() == 1)
        {
            if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "TheDamned") && hitBoxScript.CanCancel)
            {
                RegularAttack("Torment");
            }
            else if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "Banished") && hitBoxScript.CanCancel)
            {
                RegularAttack("DarkSoul");
            }
            else if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "ShinStrike") && hitBoxScript.CanCancel)
            {
                RegularAttack("InnerDemon");
            }
            else
            {
                if (CheckMoveHist(0, MoveEnum.back))
                {
                    RegularAttack("RisingCut");
                }
                else if (CheckMoveHist(0, MoveEnum.down))
                {
                    RegularAttack("RisingSpear");
                }
                else
                {
                    RegularAttack("SpecterStrike");
                }
            }
            noAttackInput = false;
        }
        else if(canAttack && pi.Player.FrontKick.ReadValue<float>() == 1)
        {
            if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "EternalVengeance") && hitBoxScript.CanCancel)
            {
                RegularAttack("TheKilling");
            }
            else if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "InnerDemon") && hitBoxScript.CanCancel)
            {
                RegularAttack("Soulless");
            }
            else
            {
                if (CheckMoveHist(0, MoveEnum.back))
                {
                    if (CheckMoveHist(1, MoveEnum.down))
                    {
                        //special
                        RegularAttack("HellPort");
                    }
                    else
                    {
                        RegularAttack("FlipKick");
                    }
                }
                else if (CheckMoveHist(0, MoveEnum.down))
                {

                    RegularAttack("SideStrike");
                }
                else if (CheckMoveHist(0, MoveEnum.forward))
                {
                    RegularAttack("FlickKick");
                }
                else
                {
                    RegularAttack("HingeKick");
                }
            }
            noAttackInput = false;
        }
        else if(canAttack && pi.Player.BackKick.ReadValue<float>() == 1)
        {
            if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "CutSlice") && hitBoxScript.CanCancel)
            {
                RegularAttack("EternalVengeance");
            }
            else if (CheckAttackHist(0, "NoAttack") && CheckAttackHist(1, "FlickKick") && hitBoxScript.CanCancel)
            {
                RegularAttack("FallingAshes");
            }
            else
            {
                if (CheckMoveHist(0, MoveEnum.back))
                {
                    RegularAttack("ScorpionSting");
                }
                else if (CheckMoveHist(0, MoveEnum.down))
                {
                    RegularAttack("QuickKick");
                }
                else if (CheckMoveHist(0, MoveEnum.forward))
                {
                    RegularAttack("ShinStrike");
                }
                else
                {
                    RegularAttack("StepKick");
                }
            }
            noAttackInput = false;
        }

        if (noAttackInput)
        {
            AddToInputHistory(GetAttackData("NoAttack"));
        }
    }

    private void RegularAttack(string atk)
    {
        hitBoxScript.StartAttack(GetAttackData(atk));
        AddToInputHistory(GetAttackData(atk));
    }

    private void SpecialAttacks(string key)
    {
        switch (key)
        {
            case ("Spear"):
                break;
            case ("Hell Port"):
                break;
            default:
                break;
        }
    }

    private AttackData GetAttackData(string key)
    {
        int left = 0;
        int right = attackList.Length - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            if (key.CompareTo(attackList[mid].AttackName) == 0)
            {
                return attackList[mid];
            }
            else if (key.CompareTo(attackList[mid].AttackName) < 0)
            {
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }
        Debug.Log("ERROR COULD NOT FIND ACTUAL DATA");
        return attackList[0];
    }

    private MoveEnum HorDirection(string direction)
    {
        if (direction == "left")
        {
            if (GameManagerScript.Instance.IsPlayerLeft)
            {
                return MoveEnum.back;
            }
            else
            {
                return MoveEnum.forward;
            }
        }
        else if (direction == "right")
        {
            if (!GameManagerScript.Instance.IsPlayerLeft)
            {
                return MoveEnum.back;
            }
            else
            {
                return MoveEnum.forward;
            }
        }
        return MoveEnum.noMove;
    }

    // pos == latest input is 0
    private bool CheckAttackHist(int pos, string atk)
    {
        if (attackHistory.Count - 1 - pos >= 0 && attackHistory[attackHistory.Count - 1 - pos].AttackName == atk)
        {
            return true;
        }
        return false;
    }

    private bool CheckMoveHist(int pos, MoveEnum move)
    {
        if (moveHistory.Count - 1 - pos >= 0 && moveHistory[moveHistory.Count - 1 - pos] == move)
        {
            return true;
        }
        return false;
    }

    private void AddToInputHistory(MoveEnum me)
    {
        if (moveHistory.Count == 0 || moveHistory[moveHistory.Count-1] != me)
        {
            moveHistory.Add(me);
            if (me != MoveEnum.noMove)
            {
                Debug.Log(me);
            }
        }


        if(moveHistory.Count == 10)
        {
            moveHistory.RemoveAt(0);
        }
    }

    private void AddToInputHistory(AttackData ad)
    {
        if (attackHistory.Count == 0 || attackHistory[attackHistory.Count - 1].AttackName != ad.AttackName)
        {
            attackHistory.Add(ad);
            if (ad.AttackName != "NoAttack")
            {
                Debug.Log(ad.AttackName);
            }
        }


        if (attackHistory.Count == 10)
        {
            attackHistory.RemoveAt(0);
        }
    }
}
