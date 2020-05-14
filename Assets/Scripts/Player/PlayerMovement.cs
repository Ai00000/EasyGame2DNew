using System;
using System.Collections;
using DG.Tweening;
using FastyTools.EventCenter;
using NaughtyAttributes;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 玩家角色移动
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [BoxGroup("Move Info")] [SerializeField]
        private float moveSpeed;

        [ReadOnly] public bool canInput = true;


        [BoxGroup("Jump Info")] [SerializeField]
        private float jumpSpeed;

        [BoxGroup("Jump Info")] [SerializeField, Tooltip("落地检测框")]
        private Vector2 boxWh;

        //跳跃相关
        [BoxGroup("Jump Info")] public int maxJumpCount = 2;
        [BoxGroup("Jump Info")] [ReadOnly] public int currentJumpCount;
        [BoxGroup("Jump Info")] public float quickDown = 2.5f;
        [BoxGroup("Jump Info")] public float slowDown = 2f;

        [BoxGroup("Jump Info"), MinMaxSlider(0, 0.16f), Tooltip("能下一次跳跃的时间")]
        public Vector2 nextJumpTimer;

        [BoxGroup("Other")] [SerializeField] private Transform checkP;
        [BoxGroup("Other")] [ReadOnly] public bool isGround;

        [BoxGroup("Other")] [SerializeField, Tooltip("碰撞层")]
        private LayerMask panelMask;

        //攻击相关
        [BoxGroup("Attack Info")] public int attack;
        [BoxGroup("Attack Info"), Tooltip("攻击间隔")]
        public float attackTimer = 1f;
        [BoxGroup("Attack Info")] public Collider2D attackBox;
        [BoxGroup("Attack Info"), Tooltip("攻击前进的距离")]
        public float attackStep;
        
        
        //受击相关
        [ReadOnly,Tooltip("是否无敌")] public bool isInvincible;
        public float invincibleTime = 1f;
        
        private bool canAttack = true;
        private Vector3 lAttackBoxPos=new Vector3(-0.52f,-0.66f);
        private Vector3 rAttackBoxPos=new Vector3(-1.07f,-0.66f);



        //组件
        private Rigidbody2D rb;
        private Animator ani;
        private SpriteRenderer sr;

        //临时变量
        private float hSpeed;
        [SerializeField] private bool jumpPressed;
        private bool startJump;
        private float jumpTime;

        //动画参数
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Jump1 = Animator.StringToHash("Jump");
        private static readonly int IsWin = Animator.StringToHash("isWin");
        private static readonly int Hit = Animator.StringToHash("Hit");

        private void Start()
        {
            canInput = true;
            currentJumpCount = 0;
            rb = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();

            //设置监听
            EventCenterManager.Instance.AddEventLister(PlayerEvent.跳跃落地 + "", OnGroundEnter);
            EventCenterManager.Instance.AddEventLister(PlayerEvent.开始攻击 + "", OnAttackEnter);
            EventCenterManager.Instance.AddEventLister(PlayerEvent.结束攻击 + "", OnAttackExit);
            EventCenterManager.Instance.AddEventLister(PlayerEvent.胜利 + "", OnWin);

        }


        private void Update()
        {
            jumpPressed = Input.GetButtonDown("Jump");
            Jump();
            JumpTime();
            Attack();
        }


        private void FixedUpdate()
        {
            isGround = IsGround();
            Move();
            AutoGravityScale();
        }


        #region 功能

        //移动
        private void Move()
        {
            if (!canInput)
            {
                return;
            }

            hSpeed = Input.GetAxisRaw("Horizontal"); //获取匀速轴

            //翻转
            if (hSpeed == -1)
            {
                sr.flipX = true;
            }

            if (hSpeed == 1)
            {
                sr.flipX = false;
            }

            ani.SetBool(IsRunning, Mathf.Abs(hSpeed) > 0.1f);


            var dir = hSpeed * moveSpeed;
            rb.velocity = new Vector2(dir, rb.velocity.y);
        }


        //跳跃
        private void Jump()
        {
            var currTime = jumpTime;
            if (jumpPressed && isGround)
            {
                jumpTime = 0f;
                print("跳");
                currentJumpCount++;
                rb.velocity += Vector2.up *jumpSpeed;

                ani.SetTrigger(Jump1);
                jumpPressed = false;
                startJump = true;

            }
            //TODO 时间限制
            // else if (jumpPressed && currentJumpCount + 1 <= maxJumpCount && currTime >= nextJumpTimer.x &&
            //          currTime <= nextJumpTimer.y)
            else if(jumpPressed && currentJumpCount + 1 <= maxJumpCount)
            {
                print("连跳");
                currentJumpCount++;
                rb.velocity=new Vector2(rb.velocity.x,jumpSpeed);
                // rb.velocity = Vector2.up *jumpSpeed;

                ani.SetTrigger(Jump1);
                jumpPressed = false;
            }
        }

        private void JumpTime()
        {
            if (startJump)
            {
                jumpTime += Time.deltaTime;
            }
        }
        

        //当进入地面
        private void OnGroundEnter()
        {
            currentJumpCount = 0;
        }


        //攻击
        private void Attack()
        {
            if (Input.GetButtonDown("Attack") && canAttack)
            {
                ani.SetTrigger(Attack1);
                canAttack = false;
                rb.AddForce(Vector2.right * attackStep);
            }
        }

        private void OnAttackEnter()
        {
            attackBox.enabled = true;
            //修正攻击触发框位置
            attackBox.transform.position = sr.flipX? lAttackBoxPos: rAttackBoxPos;
            StartCoroutine(AttackTimeHold());
        }

        private void OnAttackExit()
        {
            attackBox.enabled = false;
        }

        IEnumerator AttackTimeHold()
        {
            yield return new WaitForSeconds(attackTimer);
            canAttack = true;
        }


        /// <summary>
        /// 自动重力
        /// </summary>
        private void AutoGravityScale()
        {
            if (rb.velocity.y < 0 && !isGround)
            {
                rb.gravityScale = quickDown;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = slowDown;
            }
            else
            {
                rb.gravityScale = 1;
            }
        }

        //胜利时
        private void OnWin()
        {
            ani.SetBool(IsWin,true);
        }


        //受伤
        // [Button("受伤")]
        private void OnHit()
        {
            ani.SetTrigger(Hit);
            
            //透明闪烁
            sr.DOColor(new Color(sr.color.r, sr.color.g, sr.color.b, 0f), 0.2f).SetLoops(6, LoopType.Yoyo);
            //无敌时间
            StartCoroutine(InvincibleTime());
        }

        //无敌时间
        IEnumerator InvincibleTime()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibleTime);
            isInvincible = false;
            yield break;
        }
        
        // [Button("死亡")]
        //死亡
        private void OnDie()
        {
            ani.SetTrigger("Die");
        }
        
        #endregion


        #region 业务逻辑

        /// <summary>
        /// 是否处于地面
        /// </summary>
        /// <returns></returns>
        private bool IsGround()
        {
            var hit = Physics2D.OverlapBox(checkP.position, boxWh, 0, panelMask);
            if (hit != null)
            {
                EventCenterManager.Instance.EventTrigger(PlayerEvent.跳跃落地 + "");
            }

            return hit != null;
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(checkP.position, boxWh);
        }
    }
}