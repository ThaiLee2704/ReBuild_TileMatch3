using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_VFXManager : Tile_Singleton<Tile_VFXManager>
{
    [Header("Click Particles")]
    [SerializeField] private GameObject clickEmptyParticle;
    [SerializeField] private GameObject clickTileParticle;

    [Header("Win Particles")]
    [SerializeField] private GameObject fireWork;
    private GameObject parObject;
    [SerializeField] private GameObject winPopupParticle;

    [Header("Pools")]
    [SerializeField] private Transform clickEmptyPoolTrans;
    [SerializeField] private Transform fireWorkPoolTrans;

    public void PlayEmptyClickPar(Vector2 pos)
    {
        var parObject = Tile_ObjectPooling.Instance.GetObject(clickEmptyParticle, clickEmptyPoolTrans);
        parObject.transform.position = pos;
        parObject.SetActive(true);
    }

    public void PlayFireWork()
    {
        parObject = Tile_ObjectPooling.Instance.GetObject(fireWork, fireWorkPoolTrans);
        parObject.transform.position = fireWorkPoolTrans.position;
        parObject.SetActive(true);
    }

    public void StopFireWork()
    {
        parObject.SetActive(false);
    }
}
