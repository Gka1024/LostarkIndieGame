using UnityEngine;

public class VFXProjectile : MonoBehaviour
{
    private Vector3 _direction;
    private float _speed;
    private bool _isInitialized = false;

    // 프로젝타일 초기화 (방향과 속도 설정)
    public void Initialize(Vector3 direction, float speed)
    {
        _direction = direction.normalized;
        _speed = speed;
        
        // 이동 방향을 바라보게 회전
        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_direction);
        }
        
        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized) return;

        // 프레임마다 이동
        transform.position += _direction * _speed * Time.deltaTime;
    }
}