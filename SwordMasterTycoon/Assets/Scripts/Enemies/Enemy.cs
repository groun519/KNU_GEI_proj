using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("HP Settings")]
    public float HP = 100;
    public float maxHP;
    public float damage;

    [SerializeField] private GameObject skeleton;

    [HideInInspector] public GameObject playerObj;
    [HideInInspector] public NavMeshAgent agent;

    [HideInInspector] public bool didDeadEvent = false;

    [SerializeField] private float distance = 25.0f;
    public float targetDist;
    [HideInInspector] public bool isTargetInDist = false;

    protected virtual void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        setRigidbodyState(true);
        setColliderState(false);
    }

    public void SetDist(float _coe)
    {
        if (!didDeadEvent)
        {
            if (IsDead())
            {
                TurnRagdoll();
            }
            else
            {
                Vector3 targetVec = playerObj.transform.position + (gameObject.transform.position - playerObj.transform.position).normalized * _coe;

                targetDist = GetTargetDist(targetVec);
                isTargetInDist = targetDist <= distance;

                if (isTargetInDist)
                {
                    SetNewDest(targetVec);
                }
                else
                {
                    SetNewDest(gameObject.transform.position);
                }
            }
        }
    }

    public void LookPlayer()
    {
        gameObject.transform.rotation = Quaternion.LookRotation(playerObj.transform.position - gameObject.transform.position);
    }


    public float GetTargetDist(Vector3 _targetVec)
    {
        Vector3 dist = _targetVec - transform.position;
        return dist.magnitude;
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

        foreach(Collider collider in colliders)
        {
            collider.enabled = _state;
        }
    }


    public void CalculateDamage(float _damage)
    {
        HP -= _damage;
    }

    public bool IsDead()
    {
        return HP <= 0;
    }

    private void TurnRagdoll() // Die
    {
        //Debug.Log(gameObject.name + " : WTF");
        HP = 0;

        gameObject.GetComponent<Animator>().enabled = false;
        Destroy(gameObject, 3.0f);
        setRigidbodyState(false);
        setColliderState(true);

        Collider mainCol = GetComponent<Collider>();
        mainCol.enabled = false;

        AddForceToOppdir(100.0f);
        agent.enabled = false;

        didDeadEvent = true;
    }

    private void AddForceToOppdir(float _force) // 반대방향으로 addforce. AddForceToOppositeDirection은 너무 길어서 그만,,
    {
        Rigidbody rd = GetComponent<Rigidbody>();

        Vector3 playerXYZ = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, playerObj.transform.position.z);
        Vector3 thisXZ = new Vector3(transform.position.x, playerObj.transform.position.y + 0.5f, transform.position.z);

        Vector3 forceDir = thisXZ - playerXYZ;

        rd.AddForce(forceDir * _force, ForceMode.Impulse);
    }

    private void SetNewDest(Vector3 _vec)
    {
        agent.SetDestination(_vec);
    }
}
