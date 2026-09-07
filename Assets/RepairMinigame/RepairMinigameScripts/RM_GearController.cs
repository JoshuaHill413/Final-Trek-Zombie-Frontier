using UnityEngine;

public class RM_GearController : MonoBehaviour
{
    public float minSpeed = 80f;
    public float maxSpeed = 220f;
    public float boundTop = 3f;
    public float boundBottom = -3f;

    private float _speed;
    private float _direction = 1f;
    private float _changeTimer;
    private float _changeInterval;
    private bool _active = false;

    void Start()
    {
        transform.position = new Vector3(
            transform.position.x,
            Random.Range(boundBottom, boundTop),
            transform.position.z
        );
        PickNewSpeed();
    }

    void Update()
    {
        if (!_active) return;

        _changeTimer += Time.deltaTime;
        if (_changeTimer >= _changeInterval)
        {
            _direction *= -1f;
            PickNewSpeed();
        }

        Vector3 pos = transform.position;
        pos.y += _direction * _speed * Time.deltaTime;

        if (pos.y >= boundTop)
        {
            pos.y = boundTop;
            _direction = -1f;
            PickNewSpeed();
        }
        else if (pos.y <= boundBottom)
        {
            pos.y = boundBottom;
            _direction = 1f;
            PickNewSpeed();
        }

        transform.position = pos;
    }

    void PickNewSpeed()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _changeInterval = Random.Range(0.5f, 2f);
        _changeTimer = 0f;
    }

    public void StartGame() { _active = true; }
    public void StopGame() { _active = false; }
}