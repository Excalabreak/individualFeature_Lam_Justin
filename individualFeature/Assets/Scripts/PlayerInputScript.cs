using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 20f;

    [SerializeField] private GameObject fpHitBox;
    [SerializeField] private GameObject bpHitBox;
    [SerializeField] private GameObject fkHitBox;
    [SerializeField] private GameObject bkHitBox;
 
    private bool noMoveInput = true;
    private bool noAttackInput = true;
    private bool canAttack = true;

    private PlayerInputer pi;
    private Rigidbody rb;

    //newest input is last index
    private List<MoveEnum> moveHistory = new List<MoveEnum>();
    //TODO: change this to attack data list OR add a new list
    private List<AttackEnum> attackHistory = new List<AttackEnum>();

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
            AddToInputHistory(AttackEnum.noAttack);
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
        }


        if(moveHistory.Count == 10)
        {
            moveHistory.RemoveAt(0);
        }
    }

    private void AddToInputHistory(AttackEnum ae)
    {
        if (attackHistory.Count == 0 || attackHistory[attackHistory.Count - 1] != ae)
        {
            attackHistory.Add(ae);
        }


        if (attackHistory.Count == 10)
        {
            attackHistory.RemoveAt(0);
        }
    }
}
