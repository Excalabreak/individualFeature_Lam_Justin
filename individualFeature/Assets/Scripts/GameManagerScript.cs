using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript instance;

    [SerializeField] private GameObject PlayerGameObject;
    [SerializeField] private GameObject EnemyGameObject;
    private bool isPlayerLeft = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        CheckPlayerLeft();
        //Debug.Log(isPlayerLeft);
    }

    private void CheckPlayerLeft()
    {
        if (PlayerGameObject.transform.position.x < EnemyGameObject.transform.position.x)
        {
            isPlayerLeft = true;
        }
        else
        {
            isPlayerLeft = false;
        }
    }

    public static GameManagerScript Instance
    {
        get { return instance; }
    }

    public bool IsPlayerLeft
    {
        get { return isPlayerLeft; }
    }
}
