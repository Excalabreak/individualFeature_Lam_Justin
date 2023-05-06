using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private PlayerInputer pi;
    private Rigidbody rb;

    private List<InputData> inputs;

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
        
        if (pi.Player.Crouch.ReadValue<float>() == 1)
        {
            speed = 0f;
        }
        else
        {
            speed = 5f;
        }

        //add jump later
        if (pi.Player.Left.ReadValue<float>() == 1)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (pi.Player.Right.ReadValue<float>() == 1)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }
}
