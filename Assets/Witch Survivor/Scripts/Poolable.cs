using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    // �� ��Ȱ���� ����
    public IObjectPool<GameObject> pool { get; set; }

    // ��Ȱ���뿡 �ִ� Ÿ�̹�
    public virtual void Release()
    {
        pool.Release(gameObject);
    }
}
