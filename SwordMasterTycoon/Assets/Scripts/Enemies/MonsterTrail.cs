using System.Collections;
using UnityEngine;

public class MonsterTrail : MonoBehaviour
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

    public bool isStop = false;
    private float randY;

    void Start()
    {
        randY = Random.Range(-.5f, .5f);

        if (!reverse)
        {
            trail.transform.localPosition = new Vector3(trail.transform.localPosition.x - width * widthCoe, trail.transform.localPosition.y + randY, trail.transform.localPosition.z);
        }
        else
        {
            trail.transform.localPosition = new Vector3(trail.transform.localPosition.x + width * widthCoe, trail.transform.localPosition.y + randY, trail.transform.localPosition.z);
        }


        rotTime = width * 2 * Mathf.PI / speed * 20.0f;  // 0.25f / speed / 700.0f;
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

    IEnumerator WaitForIt(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        isStop = true;
    }
}
