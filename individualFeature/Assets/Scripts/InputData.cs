using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputData : MonoBehaviour
{
    private MoveEnum horMoveInput;
    private MoveEnum verMoveInput;
    private AttackEnum attackInput;
    private int framesHeld = 0;

    public InputData(MoveEnum hMove, MoveEnum vMove, AttackEnum attack)
    {
        horMoveInput = hMove;
        verMoveInput = vMove;
        attackInput = attack;
    }

    public void stillHeld()
    {
        framesHeld++;
    }  
    
    public int FramesHeld
    {
        get { return framesHeld; }
    }

    public MoveEnum HorMoveInput
    {
        get { return horMoveInput; }
    }

    public MoveEnum VerMoveInput
    {
        get { return verMoveInput; }
    }

    public AttackEnum AttackInput
    {
        get { return attackInput; }
    }    
}
