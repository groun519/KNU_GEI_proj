using UnityEngine;

public class SwordColor : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material[] m;

    [Header("RGB(1~100)")]
    [SerializeField] private int Red;
    [SerializeField] private int Green;
    [SerializeField] private int Blue;

    [Header("Scale")]
    [SerializeField] private float width; // 가로축(1~4)
    [SerializeField] private float height; // 세로축(1~2)
    // 검날의 x축은 4배까지 확장되게, y축과 z축은 2배까지 확장되게 할 것.

    [Header("SwordTrail")]
    [SerializeField] private Material trailMat;

    [Header("Player")]
    [SerializeField] private PlayerController player;

    private new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        m = renderer.materials;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetColor();
        SetScale();

        SetRGB();
    }

    private void SetRGB()
    {
        Red = player.Rpoint;
        Green = player.Gpoint;
        Blue = player.Bpoint;
    }

    private void SetScale()
    {
        width = 1 + Red / 50.0f;
        height = 1 + Green / 100.0f;

        float newScaleX = 1 * width;
        float newScaleY = 17 * height;
        float newScaleZ = 3 * height;

        gameObject.transform.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);
    }

    private void SetColor()
    {
        float emissin = Mathf.PingPong(Time.time, 1.0f);
        Color baseColor = new Color(Red * 2.55f / 255, Green * 2.55f / 255, Blue * 2.55f / 255, 1f);
        Color emissionColor = baseColor * Mathf.LinearToGammaSpace(emissin);

        // m1의 emissive 색상 설정
        m[0].SetColor("_EmissiveColor", emissionColor * 1.0f);
        m[0].EnableKeyword("_EMISSION");

        // m2의 emissive 색상 설정
        m[1].SetColor("_EmissiveColor", emissionColor * 250.0f);
        m[1].EnableKeyword("_EMISSION");

        trailMat.SetColor("_EmissiveColor", emissionColor * 3000.0f);
        trailMat.EnableKeyword("_EMISSION");
    }
}
