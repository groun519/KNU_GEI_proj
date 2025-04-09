using UnityEngine;

public class PlayerController : PlayerCharacter
{
    [Header("ControllerSettings")]

    private Animator anim;
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        SetMaxHp(700 + Gpoint * 32);
        SetPlayerScale(1 + Gpoint / 300.0f);
        maxStamina = 190 + Bpoint * 1.2f;
        anim.SetFloat("speed", 1.5f + Bpoint / 100.0f);
        anim.SetFloat("movespeed", 1.0f + Bpoint / (150.0f+Gpoint));

        if(HP <= 0)
        {
            Die(rigidBody);
        }

        if (isMoving())
        {
            anim.SetBool("isWalk", true);
            anim.ResetTrigger("exit");

            if (!isRun)
            {
                SetSpeed(GetWalkSpeed());
            }
        }
        else { 
            anim.SetBool("isWalk", false);
        }

        if(CanAct()) // 이동을 제외한 행동을 하려면 스태미나가 있어야 함.
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                anim.SetBool("isRun", true);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRun = true;
                SetSpeed(GetRunSpeed());
                if (stamina > 0 && isMoving())
                    SetStaminaPerSec(-20.0f); // 달리면 초당 20 스태미나 소모
            }
            else
            {
                anim.SetBool("isRun", false);
                SetStaminaPerSec(20.0f * (maxStamina / 190)); // 안달리면 회복
            }



            if (canUseBaseAttack)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    canUseBaseAttack = false;
                    AddStamina(-(50.0f - 45.0f * (Bpoint / 300.0f))); // 공격시 45 소모

                    //InstantiateTrail();
                    //swordHit.SetRadius((2.5f + Rpoint / 20.0f));
                    //swordHit.CheckSectorHitBox();

                    anim.SetTrigger("attack");
                    anim.ResetTrigger("exit");
                    //SetSpeed(0);
                    LookMouseRot();

                    BaseAttackCoolTime(3.0f / (2.0f + (Bpoint / 50.0f)));
                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    anim.SetTrigger("exit");
                    //SetSpeed(GetRunSpeed());
                }
            }
        }
        else
        {
            anim.SetBool("isRun", false);
            SetSpeed(GetWalkSpeed());
            SetStaminaPerSec(20.0f * (maxStamina / 190));
        }

        
    }

    private void FixedUpdate()
    {
        if(HP > 0)
        MovePlayer(rigidBody);
    }
}
