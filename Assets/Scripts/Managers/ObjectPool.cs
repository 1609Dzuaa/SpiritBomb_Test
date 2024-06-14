using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

[System.Serializable]
public struct PoolableObject
{
    public EPoolable _ePoolable;
    public GameObject _GObjPoolable;
    public int _ammount;
}

public class ObjectPool : BaseSingleton<ObjectPool>
{
    [SerializeField] List<PoolableObject> _listPoolableObj = new();
    private Dictionary<EPoolable, List<GameObject>> _dictPool = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        FillInDictionary();
        InstantiateGameObjects();
    }

    private void FillInDictionary()
    {
        for (int i = 0; i < _listPoolableObj.Count; i++)
            if (!_dictPool.ContainsKey(_listPoolableObj[i]._ePoolable))
                _dictPool.Add(_listPoolableObj[i]._ePoolable, new());
    }

    private void InstantiateGameObjects()
    {
        for (int i = 0; i < _listPoolableObj.Count; i++)
            for (int j = 0; j < _listPoolableObj[i]._ammount; j++)
            {
                GameObject gObj = Instantiate(_listPoolableObj[i]._GObjPoolable);
                gObj.SetActive(false);
                _dictPool[_listPoolableObj[i]._ePoolable].Add(gObj);
            }
    }

    public GameObject GetObjectInPool(EPoolable objType)
    {
        for (int i = 0; i < _dictPool[objType].Count; i++)
            if (!_dictPool[objType][i].activeInHierarchy)
                return _dictPool[objType][i];

        Debug.Log("out of " + objType);
        return null;
    }

}