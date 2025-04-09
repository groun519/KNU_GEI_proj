using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Spawnner : MonoBehaviour
{
    [Header("Spawn Trigger")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Vector3 spawnTriggerVec;
    [SerializeField] private Vector3 spawnTriggerSize;

    [Header("Spawn Objects")]
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private Vector3[] spawnOffset;
    public int[] openningDoorsPerObj; // 오브젝트당 열 문의 수
    private int allDoors;
    public GameObject[] openningDoors; // 문 들.

    private bool isClosed = false;
    
    void Start()
    {
        //spawnPosition = new Vector3[spawnObjects.Length];   

        int sum = 0;
        for(int i = 0; i < openningDoorsPerObj.Length; i++)
        {
            sum += openningDoorsPerObj[i];
        }
        allDoors = sum;
    }

    void Update()
    {
        CheckSectorHitBox();

        for (int i = 0; i < spawnObjects.Length; i++)
        {
            //spawnPosition[i] = spawnOffset[i] + transform.position;
        }
    }

    public void CheckSectorHitBox()
    {
        Collider[] hits = Physics.OverlapBox(spawnTriggerVec, spawnTriggerSize, Quaternion.identity, targetLayer);

        foreach (Collider hit in hits)
        {
            if(hit.gameObject.tag == "Player") // 플레이어를 발견
            {
                //Debug.Log(hit.gameObject.name);
                int cnt = 0; // 문을 체크할 cnt를 선언

                for (int i = 0; i < spawnObjects.Length; i++) // 생성할 적 개수만큼 i 반복
                {
                    GameObject obj = Instantiate(spawnObjects[i], spawnOffset[i], Quaternion.identity); // i번째 적 생성

                    for (int j = 0; j < openningDoorsPerObj[i]; j++) // 해당 적의 openningDoorsPerObj를 받아와, 문을 몇 개 할당할건지 알아내고, 그만큼 반복
                    {
                        //Debug.Log(cnt + " " + j);

                        Openner openner = obj.GetComponent<Openner>(); // i번째 적의 Openner 스크립트를 받아옴

                        if (cnt < openningDoors.Length) // 만약 열 문의 수를 넘어섰다면 (안전장치)
                        {
                            Door door = openningDoors[cnt].GetComponent<Door>(); // cnt번째의 문 오브젝트의 Door을 추출해 옴.

                            openner.doors[j] = door; // i번째 적의 doors 배열 j번째에 cnt번째 문 오브젝트의 Door을 할당함.

                            cnt++;
                        }
                    }
                }

                //Debug.Log(hit.gameObject.name);
                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3[] positions = spawnOffset;

        foreach(Vector3 vec in positions)
        {
            Gizmos.DrawCube(vec, Vector3.one);
        }

        Gizmos.DrawCube(spawnTriggerVec, spawnTriggerSize*2);
    }
}
