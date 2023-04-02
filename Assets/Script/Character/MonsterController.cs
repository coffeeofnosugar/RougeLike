using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterController : MonoBehaviour
{
    // 初始化击退方向
    private Vector2 direction;
    private Color originalColor;
    private float flashTime = .2f;
    private float startWaitTime = .5f;
    private float timer;
    public bool startAI = false;
    [Header("怪物基本信息")]
    public int health = 3;
    public float repelDistance = 8f; // 怪物击退距离
    [SerializeField] public MonsterAttribute MA;

    // Start is called before the first frame update
    public void Start()
    {
        MA.animator = transform.GetComponent<Animator>();
        MA.rb = transform.GetComponent<Rigidbody2D>();
        MA.sr = transform.GetComponent<SpriteRenderer>();
        originalColor = MA.sr.color;
    }

    // Update is called once per frame
    public void Update()
    {
        if (MA.isHit)
        {
            // 获取当前动画播放进度
            MA.info = MA.animator.GetCurrentAnimatorStateInfo(0);
            // 转向
            transform.localScale = new Vector3(-direction.x, 1, 1);
            // 受击击退
            MA.rb.MovePosition(MA.rb.position + direction * repelDistance * Time.deltaTime);
            if (MA.info.normalizedTime >= .6f)
            {
                MA.isHit = false;
                if (MA.isAttack)
                    MA.isAttack = false;    // 防止攻击被打断而无法再次攻击
            }
        }
        if (timer >= startWaitTime)
            startAI = true;
        if (timer <= startWaitTime + 1)
            timer += Time.deltaTime;
    }
    public void TakeDamage(Vector2 direction)
    {
        health--;
        MA.isHit = true;
        MA.sr.color = Color.red;
        Invoke("ResetColor", flashTime);
        this.direction = direction;
        MA.animator.SetTrigger("isHit");
    }
    private void ResetColor()
    {
        MA.sr.color = originalColor;
        if (health <= 0)
            Destroy(gameObject);
    }
    public void AttackEnd()
    {
        MA.isAttack = false;
    }
}

[System.Serializable]
public class MonsterAttribute
{
    // 获取当前动画播放进度
    public AnimatorStateInfo info;
    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool isHit;
    public bool isAttack;
}