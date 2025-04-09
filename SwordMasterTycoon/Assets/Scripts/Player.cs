using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Status")]
    public float HP = 1000;
    public float maxHP = 1000;
    // 방어막 기능도 추가해서 체력바 위에 파랗게 나타내면 어떨까. 파랑소울 가치도 높일겸.
    public float stamina = 250;
    public float maxStamina = 250;
    [SerializeField] private float damage;

    [Header("RGBpoint")]
    public int Rpoint;
    public int Gpoint;
    public int Bpoint;

    protected virtual void Update()
    {
        if(HP > maxHP)
            HP = maxHP;
        else if(HP < 0)
            HP = 0;
    }
}
