using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ender : MonoBehaviour
{
    [SerializeField] private Canvas can;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        UIManager umg = can.GetComponent<UIManager>();

        umg.OnLastPanel();
    }
}
