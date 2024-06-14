using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdUIController : MonoBehaviour
{
    [SerializeField] float _upDownSpeed;
    [SerializeField] float _yOffset;
    float _minY;
    float _maxY;

    // Start is called before the first frame update
    void Start()
    {
        _minY = transform.position.y - _yOffset;
        _maxY = transform.position.y + _yOffset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0f, _upDownSpeed, 0f) * Time.deltaTime);

        if (transform.position.y > _maxY)
        {
            transform.position = new Vector3(transform.position.x, _maxY, transform.position.z);
            _upDownSpeed *= -1;
        }
        else if (transform.position.y < _minY)
        {
            transform.position = new Vector3(transform.position.x, _minY, transform.position.z);
            _upDownSpeed *= -1;
        }
    }
}
