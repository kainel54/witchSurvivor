using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    // 쓸 재활용통 지정
    public IObjectPool<GameObject> pool { get; set; }

    // 재활용통에 넣는 타이밍
    public virtual void Release()
    {
        pool.Release(gameObject);
    }
}
