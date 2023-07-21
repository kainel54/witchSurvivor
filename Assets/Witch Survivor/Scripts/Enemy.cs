using UnityEngine;

// Poolable�� ��ӹ޾� PoolManager�� ����� �� �ְԲ� �Ѵ�.
public class Enemy : Poolable
{
    [SerializeField]
    private float speed = 2f;

    private Rigidbody2D target; // �÷��̾ �ִ� ���� target
    private Rigidbody2D rigid;  // ���ʹ� �����ٵ�2D

    private Vector2 moveDirection;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // ���� ���ӿ�����Ʈ�� Rigidbody2D�� �����´�.        
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

    // override : ���� Ŭ����(Poolable)�� ����(virtual) �Լ��� �����
    public override void Release()
    {
        // ���� Ŭ����(Poolable)�� Release()�� ���
        base.Release();
    }
}
