using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public bool isOpenDoor = false;
    public void Update()
    {
        if(isOpenDoor)
            OpenDoor();
    }
    private void OpenDoor()
    {
        transform.Translate(Vector3.down * 2.0f * Time.deltaTime);
        Destroy(gameObject, 3.0f);
    }
}
