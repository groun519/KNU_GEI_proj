using System.Collections;
using UnityEngine;

public class PlayerCharacter : Player
{
    [Header("UserSettings")]
    [SerializeField] private float speed;
    [SerializeField] private GameObject trail;
    private SwordTrail st;
    private SwordHit swordHit;
    [SerializeField] private GameObject skeleton;

    private Vector3 dir;
    public bool canUseBaseAttack = true;
    public bool isRun = false;

    protected virtual void Start()
    {
        st = trail.GetComponent<SwordTrail>();
        swordHit = GetComponent<SwordHit>();

        setRigidbodyState(true);
        setColliderState(false);
    }

    public void MovePlayer(Rigidbody _rb)
    {
        float horizon = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        dir = new Vector3(horizon, 0.0f, vertical) * speed;

        if (isMoving())
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(horizon, vertical) * Mathf.Rad2Deg, 0);
        }

        Vector3 move = new Vector3(dir.x, _rb.velocity.y, dir.z);
        _rb.velocity = move;
    }

    
    public void LookMouseRot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f; // Y축 방향은 제외.

            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    public bool isMoving()
    {
        return dir.x != 0.0f || dir.z != 0.0f;
    }

    public void SetStaminaPerSec(float _speed)
    {
        if(stamina < maxStamina)
        stamina += _speed * Time.deltaTime;
    }
    public void AddStamina(float _val)
    {
        stamina += _val;
    }

    public float GetRunSpeed()
    {
        return GetWalkSpeed() * 3 / 2;
    }
    public float GetWalkSpeed()
    {
        return 4.0f + (Bpoint / 50.0f);
    }
    public void SetSpeed(float _speed) { speed = _speed; }

    public void SetMaxHp(float _mhp) 
    {
        maxHP = _mhp;
    }
    public void RecoveryHP(float _val)
    {
        HP += _val;
    }

    public void SetPlayerScale(float coe)
    {
        gameObject.transform.localScale = Vector3.one * coe;
    }

    public bool CanAct()
    {
        return stamina > 0;
    }

    private void InstantiateTrail()
    {
        Instantiate(trail, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z) , Quaternion.Euler(0, transform.eulerAngles.y, 0));
    }

    private void HitTrail(float _coe)
    {
        swordHit.SetRadius((2.5f + Rpoint / 20.0f) * _coe);
        swordHit.CheckSectorHitBox(_coe);
    }

    public void BaseAttackCoolTime(float _time)
    {
        StartCoroutine(CoolTime(_time));
    }

    public void CalculateDamage(float _f)
    {
        HP -= _f;
    }

    public void Die(Rigidbody rd)
    {
        HP = 0;

        gameObject.GetComponent<Animator>().enabled = false;

        Rigidbody srd = skeleton.GetComponent<Rigidbody>();

        srd.AddForce(Vector3.up * 750.0f, ForceMode.Impulse);
        rd.isKinematic = true;

        setRigidbodyState(false);
        setColliderState(true);

        Collider mainCol = GetComponent<Collider>();
        mainCol.enabled = false;

        PlayerCharacter playerCharacter = GetComponent<PlayerCharacter>();
        Destroy(playerCharacter, 0.022f);
    }

    void setRigidbodyState(bool _state)
    {
        Rigidbody[] rigidbodies = skeleton.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigid in rigidbodies)
        {
            rigid.isKinematic = _state;
        }
    }

    void setColliderState(bool _state)
    {
        Collider[] colliders = skeleton.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = _state;
        }
    }

    // - - - 1ta - - -
    private void Hit_1ta()
    {
        st.reverse = true;
        st.widthCoe = 1.0f;
        InstantiateTrail();
        HitTrail(1.0f);
    }

    // - - - 2ta - - - 
    private void Hit_2ta()
    {
        st.reverse = true;
        st.widthCoe = 1.1f;
        InstantiateTrail();
        HitTrail(1.2f);
    }

    // - - - special 2ta - - -
    private void Hit_special_1()
    {
        st.reverse = true;
        st.widthCoe = 1.1f;
        InstantiateTrail();
        HitTrail(1.2f);
    }
    private void Hit_special_2()
    {
        st.reverse = false;
        st.widthCoe = 1.25f;
        InstantiateTrail();
        HitTrail(1.5f);
    }

    private IEnumerator CoolTime(float _time)
    {
        float timer = 0;

        while(timer < _time)
        {
            timer += 1 * Time.smoothDeltaTime;
            yield return null;
            //Debug.Log(timer);
        }

        canUseBaseAttack = true;
    }
}
