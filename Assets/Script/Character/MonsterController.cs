using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterController : MonoBehaviour
{
    [Header("怪物基本信息")]
    public int health = 3;
    public float repelDistance = 8f; // 怪物击退距离
    public float startWaitTime = .5f;
    [Header("特效")]
    public GameObject bloodEffect;
    // 初始化击退方向
    private Vector2 direction;
    private Color originalColor;
    private float flashTime = .2f;
    // 获取当前动画播放进度
    [HideInInspector] public AnimatorStateInfo info;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool isAttack;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public bool startAI = false;

    // Start is called before the first frame update
    public void Start()
    {
        animator = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody2D>();
        sr = transform.GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    // Update is called once per frame
    public void Update()
    {
        if (isAlive)
        {
            Invoke("StartAI", startWaitTime);
            if (isHit)
            {
                // 获取当前动画播放进度
                info = animator.GetCurrentAnimatorStateInfo(0);
                // 转向
                transform.localScale = new Vector3(-direction.x, 1, 1);
                // 受击击退
                rb.MovePosition(rb.position + direction * repelDistance * Time.deltaTime);
                if (info.normalizedTime >= .6f)
                {
                    isHit = false;
                    if (isAttack)
                        isAttack = false;    // 防止攻击被打断而无法再次攻击
                }
            }
        }
        else
        {
            Destroy(gameObject, 5f);
        }
    }
    private void StartAI()
    {
        startAI = true;
    }
    public void TakeDamage(Vector2 direction)
    {
        if (isAlive)
        {
            health--;
            isHit = true;
            sr.color = Color.red;
            // 流血特效
            Instantiate(bloodEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Invoke("ResetColor", flashTime);
            this.direction = direction;
            animator.SetTrigger("isHit");
        }
    }
    private void ResetColor()
    {
        sr.color = originalColor;
        if (health <= 0)
        {
            if (startAI)
                Room.JudgmenDone();
            startAI = false;
            animator.SetTrigger("die");
            isAlive = false;
        }
    }
    public void AttackEnd()
    {
        isAttack = false;
    }
}