using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class GameOverController : MonoBehaviour
{
    [SerializeField] float _popUpSpeed;
    [SerializeField] float _maxYOffset;
    [SerializeField] float _minYOffset;
    [SerializeField] float _delayPopup;

    float _maxY;
    float _minY;
    bool _changeSpeed;
    bool _canPopUp;
    Vector3 _initPos;

    private void Awake()
    {
        _initPos = transform.position;
    }

    private void OnEnable()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEvents.GameOverOnAllowPopUp, AllowPopUp);
        //Debug.Log("Subbed");
    }

    private void OnDisable()
    {
        ResetValues();
        EventsManager.Instance.UnSubcribeToAnEvent(GameEvents.GameOverOnAllowPopUp, AllowPopUp);
        //Debug.Log("UnSub");
    }

    // Start is called before the first frame update
    void Start()
    {
        _maxY = transform.position.y + _maxYOffset;
        _minY = transform.position.y - _minYOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canPopUp) return;

        //Debug.Log("Updd");
        transform.Translate(new Vector3(0f, _popUpSpeed, 0f) * Time.deltaTime);
        if (transform.position.y >= _maxY && !_changeSpeed)
        {
            _changeSpeed = true;
            _popUpSpeed *= -1;
        }

        if (transform.position.y <= _minY)
            transform.position = new Vector3(transform.position.x, _minY, transform.position.z);
    }

    private void AllowPopUp(object obj)
    {
        //Debug.Log("Popp");
        StartCoroutine(DelayPopUp());
    }

    private IEnumerator DelayPopUp()
    {
        yield return new WaitForSeconds(_delayPopup);

        _canPopUp = true;
    }

    private void ResetValues()
    {
        //Vì obj này là con của 1 gameObj dontdestroy nên vì thế nó tồn tại suốt game
        //=>Phải Sử dụng OnEnable, OnDisable

        _changeSpeed = false;
        _canPopUp = false;
        _popUpSpeed = Mathf.Abs(_popUpSpeed);
        transform.position = _initPos;
    }
}
