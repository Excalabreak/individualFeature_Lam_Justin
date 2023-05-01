using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private PlayerInputer pi;

    private void Awake()
    {
        pi = new PlayerInputer();
        pi.Enable();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {

        Vector2 moveVec = pi.Player.Move.ReadValue<Vector2>();
        transform.position += new Vector3(moveVec.x * speed * Time.deltaTime, 0, 0);
    }
}
