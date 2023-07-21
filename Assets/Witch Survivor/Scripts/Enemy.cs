using UnityEngine;

// Poolable을 상속받아 PoolManager가 사용할 수 있게끔 한다.
public class Enemy : Poolable
{
    [SerializeField]
    private float speed = 2f;

    private Rigidbody2D target; // 플레이어를 넣는 변수 target
    private Rigidbody2D rigid;  // 에너미 리짓바디2D

    private Vector2 moveDirection;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // 현재 게임오브젝트의 Rigidbody2D를 가져온다.        
    }

    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveDirection = target.position - rigid.position;
        rigid.MovePosition(rigid.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        if (moveDirection.x != 0)
        {
            transform.localRotation = moveDirection.x < 0 ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
        }
    }

    // override : 기존 클래스(Poolable)의 가상(virtual) 함수를 덮어쓴다
    public override void Release()
    {
        // 기존 클래스(Poolable)의 Release()를 사용
        base.Release();
    }
}
