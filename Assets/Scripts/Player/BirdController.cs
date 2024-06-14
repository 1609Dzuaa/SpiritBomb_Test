using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class BirdController : BaseGameObject
{
    [SerializeField] float _gravity;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _timeEachRotation;
    [SerializeField] float _flapForce;

    private Animator _anim;
    private float _verticalSpeed = 0f;
    private float _entryTime = 0f;
    private float _angle = 0f;
    private bool _enableGravity = true;
    private bool _isDead = false;
    private bool _isFirstTimeTap = true;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnCollidedGround, HandleCollideWithGround);
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.PlayerOnCollidedPipe, HandleCollideWithPipe);
    }

    // Update is called once per frame
    protected override void Update()
    {
        HandleGravity();
        HandleRotation();
        HandleFlap();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnCollidedGround, HandleCollideWithGround);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.PlayerOnCollidedPipe, HandleCollideWithPipe);
    }

    private void HandleGravity()
    {
        if (!_enableGravity || !_allowUpdate) return;

        //PT: v = v0 + a * t, s = v0 * t
        //Xử lý nếu chết thì dù v đang âm cũng cho thành dương rơi xuống cho nhanh

        _verticalSpeed += _gravity * Time.deltaTime;
        float distance = _verticalSpeed * Time.deltaTime;
        transform.position -= new Vector3(0f, distance, 0f);
        //Debug.Log("Distance: " + distance);
    }

    private void HandleRotation()
    {
        //Rotate trục z
        //Nếu verticalSpeed vẫn đang âm thì xoay mặt hướng lên (tối đa 45 độ)
        //Nếu verticalSpeed >= 0 => bắt đầu xoay mặt xuống dưới
        if (!_allowUpdate) return;

        if (_verticalSpeed >= 0)
        {
            if (Time.time - _entryTime >= _timeEachRotation)
            {
                _entryTime = Time.time;
                _angle -= _rotateSpeed;
                _angle = Mathf.Clamp(_angle, -90f, 0f);
                transform.rotation = Quaternion.Euler(0f, 0f, _angle);
            }
        }
        else
        {
            if (!_enableGravity) return;

            if (Time.time - _entryTime >= _timeEachRotation)
            {
                _entryTime = Time.time;
                _angle += _rotateSpeed;
                _angle = Mathf.Clamp(_angle, 0f, 45f);
                transform.rotation = Quaternion.Euler(0f, 0f, _angle);
            }
        }
    }

    private void HandleFlap()
    {
        if (!_isDead)
            if (Input.GetMouseButtonDown(0))
            {
                if (_isFirstTimeTap)
                {
                    _isFirstTimeTap = false;
                    _allowUpdate = true;
                    EventsManager.Instance.NotifyObservers(GameEvents.GameOnStart, null);
                }
                _verticalSpeed = -_flapForce;
                EventsManager.Instance.NotifyObservers(GameEvents.OnPlaySfx, ESfx.Flap);
                //Debug.Log("Flapped");
            }
    }

    private void HandleCollideWithGround(object obj)
    {
        float groundTop = (float)obj;
        float newPosY = groundTop + _height / 2;

        //Cố định vị trí khi va phải Ground
        transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
        _enableGravity = false;
        _isDead = true;
        _anim.SetTrigger("Die");
    }

    private void HandleCollideWithPipe(object obj)
    {
        _isDead = true;
        _anim.SetTrigger("Die");
    }
}
