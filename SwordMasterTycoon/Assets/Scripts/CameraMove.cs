using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("TargetSetting")]
    [SerializeField] private GameObject Target;

    [SerializeField] private float offX = 0.0f;
    [SerializeField] private float offY = 10.0f;
    [SerializeField] private float offZ = -5.0f;

    [SerializeField] private float speed = 10.0f;

    private Vector3 TargetPos;

    [Header("Player")]
    [SerializeField] private PlayerController player;
    


    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TargetPos = new Vector3(
            Target.transform.position.x + offX,
            Target.transform.position.y + offY,
            Target.transform.position.z + offZ);

        transform.position = Vector3.Lerp(transform.position, TargetPos, speed * Time.deltaTime);

        if (player.Rpoint > 50)
        {
            offY = 10.0f + (player.Rpoint-50) / 25.0f;
            offZ = -5.0f - (player.Rpoint-50) / 50.0f;
        }
    }
}
