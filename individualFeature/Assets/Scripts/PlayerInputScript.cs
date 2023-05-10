using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 20f;

    [SerializeField] private GameObject hitBox;
 
    private bool noMoveInput = true;
    private bool noAttackInput = true;
    private bool canAttack = true;

    private PlayerInputer pi;
    private Rigidbody rb;

    //newest input is last index
    private List<MoveEnum> moveHistory = new List<MoveEnum>();
    //TODO: change this to attack data list OR add a new list
    private List<AttackData> attackHistory = new List<AttackData>();

    [SerializeField] private AttackData[] attackList;
    private AttackData nextAttack;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        pi = new PlayerInputer();
        pi.Enable();

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
        GetInputs();
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
        else
        {
            speed = 5f;
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

        if (pi.Player.FrontPunch.ReadValue<float>() == 1)
        {
            noAttackInput = false;
        }
        else if (pi.Player.BackPunch.ReadValue<float>() == 1)
        {
            noAttackInput = false;
        }
        else if(pi.Player.FrontKick.ReadValue<float>() == 1)
        {
            noAttackInput = false;
        }
        else if(pi.Player.BackPunch.ReadValue<float>() == 1)
        {
            noAttackInput = false;
        }

        if (noAttackInput)
        {
            AddToInputHistory(GetAttackData("NoAttack"));
        }
    }

    private void RegularAttack()
    {

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

    private void AddToInputHistory(MoveEnum me)
    {
        if (moveHistory.Count == 0 || moveHistory[moveHistory.Count-1] != me)
        {
            moveHistory.Add(me);
            Debug.Log(me);
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
            Debug.Log(ad.AttackName);
        }


        if (attackHistory.Count == 10)
        {
            attackHistory.RemoveAt(0);
        }
    }
}
