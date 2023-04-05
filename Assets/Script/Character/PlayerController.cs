using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("角色信息")]
    public int health = 5;
    [Header("技能信息")]
    public float interval = 2f;
    public float maxSpeed;
    [Header("特效")]
    public GameObject bloodEffect;
    [Header("按键")]
    public Joystick moveStick; // 遥感
    private float timer;
    private bool isAlive = true;
    // 获取当前动画播放进度
    private AnimatorStateInfo info;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    // 控制位移
    private Vector2 movement;
    private int comboStep;
    private bool isAttack = false;
    private bool isAttackButton = false;
    private bool isHit;
    private Color originalColor;
    private float flashTime = .2f;
    // 击退方向
    private Vector2 direction;
    private float repelDistance = 8f; // 玩家击退距离

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            // movement.x = moveStick.Horizontal;
            // movement.y = moveStick.Vertical;
            movement.x = movement.x == 0 ? moveStick.Horizontal : movement.x;
            movement.y = movement.y == 0 ? moveStick.Vertical : movement.y;
            SwitchAnim();
            Attack();
        }
        else
        {
            Destroy(gameObject, 5f);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * maxSpeed * Time.fixedDeltaTime);    
    }

    void SwitchAnim()
    {
        // 将角色当前速度的值赋值给动画的speed，来控制角色idle与run之间的转换
        animator.SetFloat("speed", movement.magnitude);
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
        // 通过控制缩放的x来控制角色的转向
        else if (movement.x != 0 && !isAttack)
        {
            if (movement.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (movement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

        }
    }
    public void GetAttackButtonDown()
    {
        isAttackButton = true;
        StartCoroutine("GetAttackButtonUp");
    }
    IEnumerator GetAttackButtonUp()
    {
        yield return new WaitForSeconds(.001f);
        isAttackButton = false;
    }
    // 使用小刀技能
    private void Attack()
    {
        if ((Input.GetButtonDown("Attack") || isAttackButton) && !isAttack && !isHit)
        {
            isAttack = true;
            comboStep++;
            if (comboStep > 3)
                comboStep = 1;
            timer = interval;
            animator.SetTrigger("isAttack");
            animator.SetInteger("ComboStep", comboStep);
        }
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                comboStep = 0;
            }
        }
    }

    private void SkillEnd()
    {
        isAttack = false;
    }
    public void TakeDamage(Vector2 directionGet)
    {
        if (isAlive)
        {
            health--;
            HealthController.GetHit();
            isHit = true;
            sr.color = Color.red;
            Instantiate(bloodEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Invoke("ResetColor", flashTime);
            direction = directionGet;
            animator.SetTrigger("isHit");
        }
    }
    public void ResetColor()
    {
        sr.color = originalColor;
        if (health <= 0)
        {
            animator.SetTrigger("die");
            isAlive = false;
        }
    }
}