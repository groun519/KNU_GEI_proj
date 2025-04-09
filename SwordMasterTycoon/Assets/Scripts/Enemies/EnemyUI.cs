using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : Enemy
{
    [Header("UI Object Settings")]
    [SerializeField] private Slider HPBar;
    [SerializeField] private GameObject HPObj;

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void Update()
    {
        SetHPBar();

        if(didDeadEvent)
        {
            Destroy(HPObj);
        }
        else
        {
            HPBar.transform.forward = Camera.main.transform.forward;
        }
    }

    public void SetHPBar()
    {
        HPBar.value = HP / maxHP;
    }
}
