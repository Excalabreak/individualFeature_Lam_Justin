using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 20f;

    private bool noInput = true;

    private PlayerInputer pi;
    private Rigidbody rb;

    //newest input is last index
    private List<MoveEnum> inputHistory = new List<MoveEnum>();

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        pi = new PlayerInputer();
        pi.Enable();
    }

    private void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        noInput = true;
        if (pi.Player.Crouch.ReadValue<float>() == 1)
        {
            speed = 0f;
            AddToInputHistory(MoveEnum.down);
            noInput = false;
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
            noInput = false;
        }

        if (pi.Player.Left.ReadValue<float>() == 1)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            AddToInputHistory(HorDirection("left"));
            noInput = false;
        }
        if (pi.Player.Right.ReadValue<float>() == 1)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            AddToInputHistory(HorDirection("right"));
            noInput = false;
        }

        if (noInput)
        {
            AddToInputHistory(MoveEnum.noMove);
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
        if (inputHistory.Count == 0 || inputHistory[inputHistory.Count-1] != me)
        {
            inputHistory.Add(me);
        }


        if(inputHistory.Count == 10)
        {
            inputHistory.RemoveAt(0);
        }
    }
}
