using UnityEngine;

public class Soul : MonoBehaviour
{
    [Header("BoxSettings")]
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;
    [SerializeField] private Quaternion rot;

    [Header("R or G or B")]
    [SerializeField] private int RGB;
    [SerializeField] private int point = 1;

    [SerializeField] private float recovery = 0;

    private PlayerController player;
    [HideInInspector] public bool isOvelap = false;

    protected virtual void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + center, size / 2f, rot);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            if(obj.tag == "Player")
            {
                //Debug.Log(obj.name);
                player = obj.GetComponent<PlayerController>();

                SendRGBpoint(RGB);

                isOvelap = true;
                Destroy(gameObject);
            }
        }
    }

    private void SendRGBpoint(int _color)
    {
        switch (_color)
        {
            case 0:
                player.Rpoint += point;
                break;
            case 1:
                player.Gpoint += point;
                player.maxHP += point * 30;
                break;
            case 2: 
                player.Bpoint += point;  
                break;
        }

        player.RecoveryHP(recovery);
    }

    void OnDrawGizmos()
    {
        // 디버깅용으로 박스 영역을 시각적으로 표시
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(center, size);
    }
}
