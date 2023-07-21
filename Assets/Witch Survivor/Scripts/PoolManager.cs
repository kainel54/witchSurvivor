using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    // ObjectInfo : Ǯ���� �� ������Ʈ ����� class
    // System.Serializable : ����ȭ(����Ƽ���� �˾ƺ� �� �ִ� �����̴�)
    [Serializable]
    private class ObjectInfo
    {
        // objectName : ������Ʈ �̸�
        public string objectName;
        // prefab : Ǯ���� ������
        public GameObject prefab;
        // count : �̸� ������ ����
        public int count;
    }

    [Header("Ǯ�� ������Ʈ ����")]
    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // IsReady : ��� �غ� �Ǿ����� �˷��ִ� ����
    public bool IsReady { get; private set; }

    // objectName : �ӽ÷� objectName�� �����ϱ� ���� ����
    private string objectName = "";
    // objectPoolDictionary : ���� ���ο� �������� ������
    // string : �̸�, IObjectPool<GameObject>> : �̸�ǥ�� ���� ������
    private Dictionary<string, IObjectPool<GameObject>> objectPoolDictionary = new Dictionary<string, IObjectPool<GameObject>>();
    // gameObjectDictionary : ������ ���� ��
    private Dictionary<string, GameObject> gameObjectDictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        // Init: �ʱ�ȭ�� �����ִ� �Լ�
        Init();
    }

    private void Init()
    {
        // �غ� �� �Ǽ� ���� IsReady�� false�� ����
        IsReady = false;

        // objectInfos���� ������ ��ŭ �ݺ�
        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            // ObjectPool : ����Ƽ ���� Pool ���
            // ObjectPool<���� Ŭ����>(���� ���鶧 ���� �Լ�, ������ ��, ���� ��, �ı��� ��, true, �ʱ� ���� ����, �ִ� ���� ����)
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreateNewObject, OnGetPoolObject, 
                OnReleasePoolObject, OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            // objectInfos[idx].objectName : ����Ƽ���� �������� ������Ʈ�� �̸�            
            // ContainsKey : �ش� Ű�� �ҷ��� �� �ִ� �ְ� �ִ���? ����� ��
            // ���� ��� �̹� �־���� ����� �����ش�.
            if (gameObjectDictionary.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} : Already assigned.", objectInfos[idx].objectName);
            }
            // �� ������ ���� ���
            else
            {
                // ���� ���� ���� ���� �־��ش�.
                gameObjectDictionary.Add(objectInfos[idx].objectName, objectInfos[idx].prefab);
                // ū �뿡�ٰ� �̸��� �ٿ��� ���� ���� �ִ´�
                objectPoolDictionary.Add(objectInfos[idx].objectName, pool);
            }            

            // ���� ������ŭ ������Ʈ�� ����
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                // Poolable ��ü�� �ҷ��´�.
                Poolable poolable = CreateNewObject().GetComponent<Poolable>();
                // Release(�� �� �� �����ϴ� �Լ�)�� �������ش�.
                poolable.pool.Release(poolable.gameObject);
            }
        }

        Debug.Log("[PoolManager] Ready to pooling");
        IsReady = true;
    }

    // OnDestroyPoolObject : ��ü�� �ı��� �� �����ϴ� �Լ�
    private void OnDestroyPoolObject(GameObject obj)
    {
        throw new NotImplementedException();
    }

    // OnReleasePoolObject : ��ü�� ������ ��(�׸� �� ��) �����ϴ� �Լ�
    private void OnReleasePoolObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    // OnGetPoolObject : ��ü�� ������ ��(�� ��) �����ϴ� �Լ�
    private void OnGetPoolObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    // CreateNewObject : ���ο� ��ü�� ���� �� �����ϴ� �Լ�
    private GameObject CreateNewObject()
    {
        GameObject newObject = Instantiate(gameObjectDictionary[objectName]);
        // ���� � �� �Ҽ����� ��������
        newObject.GetComponent<Poolable>().pool = objectPoolDictionary[objectName];
        return newObject;
    }

    public GameObject GetObject(string name)
    {
        objectName = name;

        // ���� �뿡�� name�̶�� �̸��� ���� ������Ʈ�� ���� ���
        if (!gameObjectDictionary.ContainsKey(name))
        {
            // ������ ���� null�� �����ش�.
            Debug.LogFormat("{0} : Key not found in pool", name);
            return null;
        }
        // �ִ� ��� �̸� ���� �ָ� ������ ����Ѵ�.
        return objectPoolDictionary[name].Get();
    }
}
