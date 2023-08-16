using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;

    protected bool triggerCalled;
    private string animBoolname;
    protected float stateTimer;


    public EnemyState (Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolname = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolname, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolname, false);

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }


}
