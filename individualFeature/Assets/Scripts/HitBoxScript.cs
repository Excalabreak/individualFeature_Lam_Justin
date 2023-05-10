using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour
{
    [SerializeField] private Material startUpMat;
    [SerializeField] private Material activeMat;
    [SerializeField] private Material recoveryMat;

    private AttackData currentAttack;
    private AttackStateEnum curState = AttackStateEnum.NoAttack;
    private int totalFrameCount = 0;
    private int frameCount = 0;

    private bool hit = false;
    private bool canCancel = false;

    private void Update()
    {
        SetHitBoxDirection();
        if (curState != AttackStateEnum.NoAttack)
        {
            Attack();
        }
    }

    public void StartAttack(AttackData attack)
    {
        if (curState == AttackStateEnum.NoAttack || canCancel)
        {
            currentAttack = attack;
            curState = AttackStateEnum.StartUp;
            totalFrameCount = 0;
            frameCount = 0;
            hit = false;
            canCancel = false;
            RenderHitBox();
        }
    }

    private void Attack()
    {
        switch (curState)
        {
            case AttackStateEnum.StartUp:
                totalFrameCount++;
                frameCount++;
                if (frameCount == currentAttack.StartUpFrames)
                {
                    frameCount = 0;
                    curState++;
                    RenderHitBox();
                }
                break;

            case AttackStateEnum.Active:
                totalFrameCount++;
                frameCount++;
                if (!canCancel)
                {
                    CheckCanCancel();
                }
                if (frameCount == currentAttack.ActiveFrames)
                {
                    frameCount = 0;
                    curState++;
                    RenderHitBox();
                }
                break;

            case AttackStateEnum.Recovery:
                totalFrameCount++;
                frameCount++;
                if (!canCancel)
                {
                    CheckCanCancel();
                }
                if (frameCount == currentAttack.RecoveryFrames)
                {
                    totalFrameCount = 0;
                    frameCount = 0;
                    curState = AttackStateEnum.NoAttack;
                    canCancel = false;
                    RenderHitBox();
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (curState == AttackStateEnum.Active && other.gameObject.tag == "Enemy")
        {
            hit = true;
        }

        if (curState != AttackStateEnum.Active && curState != AttackStateEnum.Recovery)
        {
            hit = false;
        }
    }

    private void CheckCanCancel()
    {
        if (hit && totalFrameCount >= currentAttack.CancelAdv)
        {
            canCancel = true;
        }
    }

    private void RenderHitBox()
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        switch (curState)
        {
            case AttackStateEnum.NoAttack:
                renderer.enabled = false;
                break;
            case AttackStateEnum.StartUp:
                renderer.enabled = true;
                renderer.material = startUpMat;
                break;
            case AttackStateEnum.Active:
                renderer.enabled = true;
                renderer.material = activeMat;
                break;
            case AttackStateEnum.Recovery:
                renderer.enabled = true;
                renderer.material = recoveryMat;
                break;
            default:
                break;
        }
    }

    private void SetHitBoxDirection()
    {
        if (GameManagerScript.Instance.IsPlayerLeft)
        {
            gameObject.transform.localPosition = new Vector3(1.5f, 0.25f, 0f);
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(-1.5f, 0.25f, 0f);
        }
    }

    public bool CanCancel
    {
        get { return canCancel; }
    }

    public AttackStateEnum CurState
    {
        get { return curState; }
    }
}
