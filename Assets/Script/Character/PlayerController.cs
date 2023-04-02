using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("角色信息")]
    public int health = 10;
    public float maxSpeed;
    [Header("小地图")]
    public GameObject miniMap;
    [Header("技能信息")]
    public float interval = 2f;
    [SerializeField] public PlayerAttribute PA;

    // Start is called before the first frame update
    void Start()
    {
        PA.rb = GetComponent<Rigidbody2D>();
        PA.animator = GetComponent<Animator>();
        PA.sr = GetComponent<SpriteRenderer>();
        PA.originalColor = PA.sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        PA.movement.x = Input.GetAxisRaw("Horizontal");
        PA.movement.y = Input.GetAxisRaw("Vertical");
        SwitchAnim();
        //打开地图
        miniMap.gameObject.SetActive(Input.GetKey(KeyCode.M));
        Attack();
    }

    private void FixedUpdate()
    {
        PA.rb.MovePosition(PA.rb.position + PA.movement * maxSpeed * Time.fixedDeltaTime);    
    }

    void SwitchAnim()
    {
        // 将角色当前速度的值赋值给动画的speed，来控制角色idle与run之间的转换
        PA.animator.SetFloat("speed", PA.movement.magnitude);
        if (PA.isHit)
        {
            
            // 获取当前动画播放进度
            PA.info = PA.animator.GetCurrentAnimatorStateInfo(0);
            // 转向
            transform.localScale = new Vector3(-PA.direction.x, 1, 1);
            // 受击击退
            PA.rb.MovePosition(PA.rb.position + PA.direction * PA.repelDistance * Time.deltaTime);
            if (PA.info.normalizedTime >= .6f)
            {
                PA.isHit = false;
                if (PA.isAttack)
                    PA.isAttack = false;    // 防止攻击被打断而无法再次攻击
            }
        }
        // 通过控制缩放的x来控制角色的转向
        else if (PA.movement.x != 0 && !PA.isAttack)
        {
            transform.localScale = new Vector3(PA.movement.x, 1, 1);
        }
    }
    // 使用小刀技能
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && !PA.isAttack && !PA.isHit)
        {
            PA.isAttack = true;
            PA.comboStep++;
            if (PA.comboStep > 3)
                PA.comboStep = 1;
            PA.timer = interval;
            PA.animator.SetTrigger("skill_knife");
            PA.animator.SetInteger("ComboStep", PA.comboStep);
        }
        if (PA.timer != 0)
        {
            PA.timer -= Time.deltaTime;
            if (PA.timer <= 0)
            {
                PA.timer = 0;
                PA.comboStep = 0;
            }
        }
    }
    private void SkillEnd()
    {
        PA.isAttack = false;
    }
    public void TakeDamage(Vector2 direction)
    {
        health--;
        PA.isHit = true;
        PA.sr.color = Color.red;
        Invoke("ResetColor", PA.flashTime);
        PA.direction = direction;
        PA.animator.SetTrigger("isHit");
    }
    public void ResetColor()
    {
        PA.sr.color = PA.originalColor;
        if (health <= 0)
        {
            Debug.Log("aaaaa");
        }
            Destroy(gameObject);
    }
}

[System.Serializable]
public class PlayerAttribute
{
    // 获取当前动画播放进度
    public AnimatorStateInfo info;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;
    // 控制位移
    public Vector2 movement;
    public int comboStep;
    public float timer;
    public bool isAttack = false;
    public bool isHit;
    public Color originalColor;
    public float flashTime = .2f;
    // 击退方向
    public Vector2 direction;
    public float repelDistance = 8f; // 玩家击退距离
}