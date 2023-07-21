using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    // ObjectInfo : 풀링을 할 오브젝트 선언용 class
    // System.Serializable : 직렬화(유니티에서 알아볼 수 있는 형태이다)
    [Serializable]
    private class ObjectInfo
    {
        // objectName : 오브젝트 이름
        public string objectName;
        // prefab : 풀링할 프리팹
        public GameObject prefab;
        // count : 미리 만들어둘 갯수
        public int count;
    }

    [Header("풀링 오브젝트 설정")]
    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // IsReady : 사용 준비가 되었는지 알려주는 변수
    public bool IsReady { get; private set; }

    // objectName : 임시로 objectName을 보관하기 위한 변수
    private string objectName = "";
    // objectPoolDictionary : 각각 새로운 보관통을 정의함
    // string : 이름, IObjectPool<GameObject>> : 이름표가 붙은 보관통
    private Dictionary<string, IObjectPool<GameObject>> objectPoolDictionary = new Dictionary<string, IObjectPool<GameObject>>();
    // gameObjectDictionary : 보관통 안의 통
    private Dictionary<string, GameObject> gameObjectDictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        // Init: 초기화를 시켜주는 함수
        Init();
    }

    private void Init()
    {
        // 준비가 안 되서 먼저 IsReady를 false로 세팅
        IsReady = false;

        // objectInfos에서 설정한 만큼 반복
        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            // ObjectPool : 유니티 내장 Pool 기능
            // ObjectPool<만들 클래스>(새로 만들때 쓰는 함수, 꺼내올 때, 넣을 때, 파괴할 때, true, 초기 생성 갯수, 최대 생성 갯수)
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreateNewObject, OnGetPoolObject, 
                OnReleasePoolObject, OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            // objectInfos[idx].objectName : 유니티에서 설정해준 오브젝트의 이름            
            // ContainsKey : 해당 키로 불러올 수 있는 애가 있느냐? 물어보는 것
            // 있을 경우 이미 있어요라고 경고문을 날려준다.
            if (gameObjectDictionary.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} : Already assigned.", objectInfos[idx].objectName);
            }
            // 안 가지고 있을 경우
            else
            {
                // 새로 작은 통을 만들어서 넣어준다.
                gameObjectDictionary.Add(objectInfos[idx].objectName, objectInfos[idx].prefab);
                // 큰 통에다가 이름을 붙여서 작은 통을 넣는다
                objectPoolDictionary.Add(objectInfos[idx].objectName, pool);
            }            

            // 지정 갯수만큼 오브젝트를 생성
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                // Poolable 객체를 불러온다.
                Poolable poolable = CreateNewObject().GetComponent<Poolable>();
                // Release(안 쓸 때 실행하는 함수)를 실행해준다.
                poolable.pool.Release(poolable.gameObject);
            }
        }

        Debug.Log("[PoolManager] Ready to pooling");
        IsReady = true;
    }

    // OnDestroyPoolObject : 객체가 파괴될 때 실행하는 함수
    private void OnDestroyPoolObject(GameObject obj)
    {
        throw new NotImplementedException();
    }

    // OnReleasePoolObject : 객체를 돌려줄 때(그만 쓸 때) 실행하는 함수
    private void OnReleasePoolObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    // OnGetPoolObject : 객체를 가져올 때(쓸 때) 실행하는 함수
    private void OnGetPoolObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    // CreateNewObject : 새로운 객체를 만들 때 실행하는 함수
    private GameObject CreateNewObject()
    {
        GameObject newObject = Instantiate(gameObjectDictionary[objectName]);
        // 내가 어떤 통 소속인지 정의해줌
        newObject.GetComponent<Poolable>().pool = objectPoolDictionary[objectName];
        return newObject;
    }

    public GameObject GetObject(string name)
    {
        objectName = name;

        // 작은 통에서 name이라는 이름을 가진 오브젝트가 없을 경우
        if (!gameObjectDictionary.ContainsKey(name))
        {
            // 에러를 내며 null을 돌려준다.
            Debug.LogFormat("{0} : Key not found in pool", name);
            return null;
        }
        // 있는 경우 이름 가진 애를 꺼내서 사용한다.
        return objectPoolDictionary[name].Get();
    }
}
