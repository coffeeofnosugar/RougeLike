using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : MonsterController
{
    private GameObject target;
    public float maxSpeed = 5;
    private Vector2 movement_;
    public float attackRange = 1f;
    private float interval = 2f;
    private float timerSon;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (isAlive)
        {
            if (startAI)
            {
                StartMove();
                SwitchAnim();
                Attack();
            }
        }
    }
    private void StartMove()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player");
            // if (target.transform.position - transform.position != new Vector3(0, 0, 0))
            movement_ = target.transform.position - transform.position;
            if (movement_.magnitude >= .3f && !isAttack && !isHit)
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, maxSpeed * Time.deltaTime);
        }
    }
    public void SwitchAnim()
    {
        // 将角色当前速度的值赋值给动画的speed，来控制角色idle与run之间的转换
        animator.SetFloat("speed", movement_.magnitude);
        // 通过控制缩放的x来控制角色的转向
        if (movement_.x >= 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
    public void Attack()
    {
        if (movement_.magnitude <= attackRange && !isAttack && !isHit && timerSon <= 0)
        {
            isAttack = true;
            timerSon = interval;
            animator.SetTrigger("isAttack");
        }
        if (timerSon != 0)
        {
            timerSon -= Time.deltaTime;
            if (timerSon <= 0)
                timerSon = 0;
        }
    }
}
