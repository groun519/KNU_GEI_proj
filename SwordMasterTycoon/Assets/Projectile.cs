using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject sphereObj;
    [SerializeField] private float damage;
    [SerializeField] private float speed = 15.0f;
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private bool destroy = true;
    private float damageCoe = 1.0f;

    private void Start()
    {
        SetRadius();
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        CheckHitBox();

        Destroy(gameObject, 15.0f);
    }

    public void SetDamageCoe(float _coe)
    {
        damageCoe = _coe;
    }

    public void CheckHitBox()
    {
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        Collider[] hits = Physics.OverlapSphere(position, radius/2, targetLayer);

        foreach (Collider hit in hits)
        {
            if(hit.gameObject.tag == "Player")
            {
                PlayerController pc = hit.gameObject.GetComponent<PlayerController>();

                pc.CalculateDamage(damage * damageCoe);

                if (destroy)
                    Destroy(gameObject, 0.1f);
                else
                    Destroy(gameObject, 5.0f);
            }
            else
            {
                if (destroy)
                    Destroy(gameObject, 0.1f);
                else
                    Destroy(gameObject, 5.0f);
            }
        }
    }

    public void SetRadius()
    {
        sphereObj.transform.localScale = new Vector3(radius, radius, radius);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius/2);
    }
}
