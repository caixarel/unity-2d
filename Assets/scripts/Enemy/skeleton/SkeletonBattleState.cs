using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;
    private int moveDir;
    private string animBoolName;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
        this.animBoolName = _animBoolName;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance )
            {

                stateTimer = enemy.battleTime;

                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);

                enemyBase.anim.SetBool(this.animBoolName, false);
                return;
            }
            else
            {
                enemyBase.anim.SetBool(this.animBoolName, true);
            }


        }
        else
        {

            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 9)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }


        if (player.position.x > enemy.transform.position.x) moveDir = 1;
        else if (player.position.x < enemy.transform.position.x) moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.velocity.y);
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
