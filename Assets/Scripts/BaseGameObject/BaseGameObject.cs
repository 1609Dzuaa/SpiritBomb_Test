using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class BaseGameObject : MonoBehaviour
{
    protected float _width;
    protected float _height;
    protected SpriteRenderer _spriteRender;
    protected bool _allowUpdate;

    public float Width { get => _height; }

    public float Height { get => _width; }

    public SpriteRenderer SpriteRender { get => _spriteRender; }

    protected virtual void Awake()
    {
        _spriteRender = GetComponent<SpriteRenderer>();
        _width = _spriteRender.bounds.size.x;
        _height = _spriteRender.bounds.size.y;
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.GameOnStart, OnAllowUpdate);
        //Debug.Log("W: " + _width);
    }

    protected virtual void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.GameOnStart, OnAllowUpdate);
    }

    // Start is called before the first frame update
    protected virtual void Start() { }

    protected virtual void Update() { }

    protected void OnAllowUpdate(object obj)
    {
        _allowUpdate = true;
    }
}
