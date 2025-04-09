using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy_Range : EnemyUI
{
    private Animator anim;

    private bool usedGimmick = false;
    //private bool usedAttack = false;
    private bool isDropItem = false;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float attackRange = 5.0f;
    [SerializeField] private Transform handTrans;
    [SerializeField] private GameObject preSphere;

    [Header("Drops")]
    [SerializeField] private GameObject[] dropItems;

    [Header("One Hand")]
    [SerializeField] private float damageCoe; // 딜계수
    [SerializeField] private float speedCoe; // 속도계수
    [SerializeField] private float findCoe; // 감지범위 계수
    [SerializeField] private float range; // 범위
    [SerializeField] private GameObject projectile;

    [Header("Two Hand")]
    [SerializeField] private GameObject projectile2;

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
        SetDist(attackRange);
        LookPlayer();

        //Debug.Log(targetDist);

        if (base.isTargetInDist)
        {
            //Debug.Log("find Target");

            if (!usedGimmick)
            {
                if (targetDist < (5.0f) * findCoe)
                {
                    PlayRandGimmick();
                }
            }

            //Debug.Log(usedAttack);

            /*if (!usedAttack)
            {
                if (targetDist < 1.8f)
                {
                    CommonAttack();
                }
                else if (targetDist < 5.0f)
                {
                    PlayRandGimmick();
                }
            }*/
        }

        if (IsDead())
        {
            if (!isDropItem)
            {
                DropItems();
            }
        }
    }

    private void PlayRandGimmick()
    {
        int randNum = UnityEngine.Random.Range(1, 5);
        //Debug.Log(randNum);

        switch (randNum)
        {
            case 1: // 기믹 1
                OneHandAttack();
                break;
            case 2:
                TwoHandAttack();
                break;
            default: // 꽝
                break;
        }

        usedGimmick = true;

        StartCoroutine(CoolTime(2.0f)); // 기믹 텀은 5초
    }

    private void OneHandAttack()
    {
        anim.SetTrigger("1HandA");
    }

    // - - - 원핸드 - - -

    private void AnimStart_1hand(float f)
    {
        agent.speed = f;

        preSphere.SetActive(true);
    }
    private void Throw_1hand(float f)
    {
        agent.speed = f;
        agent.acceleration = 80.0f;

        Vector3 vec = playerObj.transform.position - gameObject.transform.position;
        vec.y += 1;

        SpawnProjectile(projectile);

        preSphere.SetActive(false);
    }
    private void AnimEnd_1hand(float f)
    {
        agent.speed = f;
        agent.acceleration = 8.0f;
        //usedAttack = true;

        //Gimmick1_hit();
    }

    // - - - 

    private void TwoHandAttack()
    {
        anim.SetTrigger("2HandA");

        StartCoroutine(AttackSpeed(1.2f));
    }

    // - - - 투핸드 - - -

    private void AnimStart_2hand(float f)
    {
        agent.speed = f;

        preSphere.SetActive(true);
    }
    private void Throw_2hand(float f)
    {
        agent.speed = f;

        SpawnProjectile(projectile2);

        preSphere.SetActive(false);
    }
    private void AnimEnd_2hand(float f)
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
        //usedAttack = false;
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

        //usedAttack = false;
    }

    private void SpawnProjectile(GameObject _proj)
    {
        Vector3 vec = handTrans.transform.position; // + (playerObj.transform.position - handTrans.transform.position).normalized;
        Vector3 playerVec = playerObj.transform.position;
        playerVec.y += 1.2f;
        Quaternion qut = Quaternion.LookRotation(playerVec - handTrans.transform.position);
        //qut.x = 0;
        GameObject projObj = Instantiate(_proj, vec, qut);

        Projectile projectile = projObj.GetComponent<Projectile>();

        projectile.SetDamageCoe(damageCoe);
    }

    private void DropItems()
    {
        int randNum = UnityEngine.Random.Range(0, dropItems.Length);

        Vector3 dropVec = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Instantiate(dropItems[randNum], dropVec, Quaternion.identity);

        isDropItem = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, range);
    }
}
