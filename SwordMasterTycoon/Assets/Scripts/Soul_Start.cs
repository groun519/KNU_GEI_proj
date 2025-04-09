using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul_Start : Soul
{
    [SerializeField] private GameObject[] otherSouls;

    protected override void Update()
    {
        base.Update();

        if (isOvelap)
        {
            DestroyOtherSouls();
        }
    }

    private void DestroyOtherSouls()
    {
        Destroy(otherSouls[0], .1f);
        Destroy(otherSouls[1], .1f);
    }
}
