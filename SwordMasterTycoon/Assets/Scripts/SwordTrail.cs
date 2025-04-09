using System.Collections;
using UnityEngine;

public class SwordTrail : MonoBehaviour
{

    [Header("Trail Setttings")]
    [SerializeField] private GameObject trail;

    [SerializeField] private float width = 2.0f;

    [SerializeField] private float speed = 700.0f;
    private float rotTime;

    public bool reverse = false;
    public float widthCoe = 1.0f;

    private float speedMulti; // 속도 배수

    [SerializeField] private float height = 10.0f;

    private bool isStop = false;
    private float randY;

    private PlayerController player;
    private SwordHit sHit; // shit 아님. Sword Hit zz

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<PlayerController>();
        sHit = player.GetComponent<SwordHit>();

        SetWidth();
        SetSpeed();

        randY = Random.Range(-.5f, .5f);

        if (!reverse)
        {
            trail.transform.localPosition = new Vector3(trail.transform.localPosition.x - width*widthCoe, trail.transform.localPosition.y + randY, trail.transform.localPosition.z);
        }
        else
        {
            trail.transform.localPosition = new Vector3(trail.transform.localPosition.x + width*widthCoe, trail.transform.localPosition.y + randY, trail.transform.localPosition.z);
        }
        
        
        rotTime = (sHit.angle / 180 * 0.25f) / (speed / 700.0f);
        //Debug.Log(rotTime);
    }

    void Update()
    {
        StartCoroutine(WaitForIt(rotTime));
        Destroy(gameObject, 3.0f);

        if (!isStop)
        {
            Rot();
        }
    }
    
    private void Rot()
    {
        if (!reverse)
        {
            transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(transform.position, Vector3.up, -speed * Time.deltaTime);
        }
    }

    public void SetWidth()
    {
        width = 2.5f + player.Rpoint / 20.0f;
        //Debug.Log(width);
    }

    private void SetSpeed()
    {
        speed = 750.0f + player.Bpoint / 300.0f * 750.0f;
    }

    IEnumerator WaitForIt(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        isStop = true;
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
