using System.Collections;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using Color = UnityEngine.Color;

public class Enemy_Common : EnemyUI
{
    private Animator anim;

    private bool usedGimmick = false;
    private bool usedAttack = false;
    private bool isDropItem = false;

    [SerializeField] private LayerMask targetLayer;
    

    [Header("Drops")]
    [SerializeField] private GameObject[] dropItems;

    [Header("Defult Attack")]
    [SerializeField] private float damageCoe_punch;
    [SerializeField] private float range_punch;
    [SerializeField] private GameObject trail;

    [Header("Gimmick 1")]
    [SerializeField] private float damageCoe; // 딜계수
    [SerializeField] private float speedCoe; // 속도계수
    [SerializeField] private float findCoe; // 감지범위 계수
    [SerializeField] private float range; // 범위

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Debug.Log(base.isTargetInDist);

        base.Update();
        SetDist(0);

        if (IsDead())
        {
            if (!isDropItem)
            {
                DropItems();
            }
        }

        //Debug.Log(targetDist);

        if (base.isTargetInDist)
        {
            //Debug.Log("find Target");

            if (!usedGimmick)
            {
                if (targetDist < (5.0f)*findCoe)
                {
                    PlayRandGimmick();
                }
            }

            //Debug.Log(usedAttack);

            if (!usedAttack)
            {
                if (targetDist < 1.8f)
                {
                    CommonAttack();
                }
                else if (targetDist < 5.0f)
                {
                    PlayRandGimmick();
                }
            }
        }
    }

    private void CheckSectorHitBox(float _coe)
    {
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        Collider[] hits = Physics.OverlapSphere(position, range_punch, targetLayer);

        foreach (Collider hit in hits)
        {
            Vector3 targetDir = (hit.transform.position - position).normalized;

            if (Vector3.Angle(forward, targetDir) < 180 / 2)
            {
                PlayerController target = hit.gameObject.GetComponent<PlayerController>();
                target.CalculateDamage(damage * _coe);
            }
        }
    }

    private void PlayRandGimmick()
    {
        int randNum = UnityEngine.Random.Range(1, 10);
        //Debug.Log(randNum);

        switch (randNum)
        {
            case 1: // 기믹 1
                Gimmick1();
                break;
            default: // 꽝
                break;
        }

        usedGimmick = true;

        StartCoroutine(CoolTime(5.0f)); // 기믹 텀은 5초
    }

    private void Gimmick1()
    {
        anim.SetTrigger("JAttack");
    }
    private void Gimmick1_hit()
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, range, targetLayer);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.tag == "Player")
            {
                PlayerController player = hit.gameObject.GetComponent<PlayerController>();
                player.CalculateDamage(damage * damageCoe);
            }
        }
    }

    // - - - 기믹 1 - - -

    private void AnimStart(float f)
    {
        agent.speed = f;
        //agent.angularSpeed = 360000.0f;

        usedAttack = true;
    }
    private void JumpStart(float f)
    {
        agent.speed = (((5.0f)*findCoe + targetDist) / 10.0f*findCoe * f) * speedCoe;
        //agent.angularSpeed = 20.0f;
        agent.acceleration = 80.0f;
    }
    private void JumpEnd(float f)
    {
        agent.speed = f;
        //agent.angularSpeed = 120.0f;
        agent.acceleration = 8.0f;
        usedAttack = true;

        Gimmick1_hit();
    }
    private void AnimEnd(float f)
    {
        agent.speed = f;
    }

    // - - - 

    private void CommonAttack()
    {
        anim.SetTrigger("CAttack");

        usedAttack = true;

        StartCoroutine(AttackSpeed(1.2f));
    }

    // - - - 기본공격 - - -

    private void AnimStart_punch(float f)
    {
        agent.speed = f;
    }
    private void Hit_punch(float f)
    {
        agent.speed = f;

        CheckSectorHitBox(damageCoe_punch);
        Instantiate(trail, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.Euler(0, transform.eulerAngles.y, 0));
    }
    private void AnimEnd_punch(float f)
    {
        agent.speed = f;
    }

    private IEnumerator CoolTime(float _time)
    {
        float timer = 0;

        while (timer < _time)
        {
            timer += 1 * Time.smoothDeltaTime;
            //Debug.Log(timer);
            yield return null;
        }

        usedGimmick = false;
        usedAttack = false;
    }

    private IEnumerator AttackSpeed(float _time)
    {
        float timer = 0;

        while (timer < _time)
        {
            timer += 1 * Time.smoothDeltaTime;
            //Debug.Log(timer);
            yield return null;
        }

        usedAttack = false;
    }

    private void DropItems()
    {
        int randNum = UnityEngine.Random.Range(0, dropItems.Length);

        Vector3 dropVec = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
        Instantiate(dropItems[randNum], dropVec, Quaternion.identity);

        isDropItem = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, range);




        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        // 부채꼴의 양 끝 방향 계산
        Vector3 rightBoundary = Quaternion.Euler(0, 180 / 2, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -180 / 2, 0) * forward;

        // 부채꼴을 원호로 그리기
        int segments = 20;
        float segmentAngle = 180 / segments;
        Vector3 previousPoint = position + leftBoundary * range_punch;
        for (int i = 1; i <= segments; i++)
        {
            Vector3 nextPoint = position + (Quaternion.Euler(0, segmentAngle * i - 180 / 2, 0) * forward) * range_punch;
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        // 중심에서 부채꼴의 끝점으로 선 그리기
        Gizmos.DrawLine(position, position + leftBoundary * range_punch);
        Gizmos.DrawLine(position, position + rightBoundary * range_punch);
    }
}
