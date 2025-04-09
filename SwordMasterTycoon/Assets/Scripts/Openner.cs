using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openner : MonoBehaviour
{
    public Door[] doors;

    private void OnDestroy()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].isOpenDoor = true;
        }
    }
}
