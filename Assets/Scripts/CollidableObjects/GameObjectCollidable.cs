using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class GameObjectCollidable : BaseGameObject
{
    protected Transform _playerRef;
    protected float _playerHeight;
    protected float _playerWidth;
    protected static bool _sfxPlayed; //biến này dùng chung cho tất cả các gObj Collidable
    //Để Play CollideSfx 1 lần duy nhất

    protected override void Awake()
    {
        base.Awake();
        _sfxPlayed = false;
    }

    protected override void Start()
    {
        base.Start();
        _playerRef = FindObjectOfType<BirdController>().transform;
        _playerHeight = _playerRef.GetComponent<BirdController>().Height;
        _playerWidth = _playerRef.GetComponent<BirdController>().Width;
    }

    protected override void Update()
    {
        CheckCollision();
    }

    protected virtual void CheckCollision()
    {
        float currentX = transform.position.x;
        float currentY = transform.position.y;
        float playerX = _playerRef.position.x;
        float playerY = _playerRef.position.y;

        float playerTop = playerY + _playerHeight / 2;
        float playerLeft = playerX - _playerWidth / 2;
        float playerRight = playerX + _playerWidth / 2;
        float playerBot = playerY - _playerHeight / 2;

        float gObjTop = currentY + _height / 2;
        float gObjLeft = currentX - _width / 2;
        float gObjRight = currentX + _width / 2;
        float gObjBot = currentY - _height / 2;

        if (playerTop > gObjBot && playerRight > gObjLeft
            && playerBot < gObjTop && playerLeft < gObjRight)
        {
            FireCollisionEvent();
            HandleCollision();
            //Debug.Log("Collided");
        }
        //else
            //Debug.Log("Not Collide");
    }

    //Phát thông báo có va chạm
    protected virtual void FireCollisionEvent()
    {
        float groundTop = transform.position.y + _height / 2;
        EventsManager.Instance.NotifyObservers(GameEvents.PlayerOnCollidedGround, groundTop);
    }

    //Tuỳ object thì sẽ xử lý va chạm ra sao
    protected virtual void HandleCollision() 
    {
        if (_sfxPlayed) return;

        EventsManager.Instance.NotifyObservers(GameEvents.PlayerOnLose, null);
        EventsManager.Instance.NotifyObservers(GameEvents.OnPlaySfx, ESfx.Collided);
        EventsManager.Instance.NotifyObservers(GameEvents.OnPlaySfx, ESfx.Die);
        EventsManager.Instance.NotifyObservers(GameEvents.WhiteFrameOnPopUp, null);
        _sfxPlayed = true;
    }
}
