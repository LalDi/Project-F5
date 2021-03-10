using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//where 키워드 : https://m.blog.naver.com/beaqon/221301092125

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);//현재프레임작업이 완료될 때까지 기다리지 않고 즉시 삭제
            return;
        }
        Instance = (T)FindObjectOfType(typeof(T));//이 싱글톤 제네릭을 사용한 타입 T를 가진 오브젝트를 찾아 인스턴스를 할당한다

        DontDestroyOnLoad(gameObject);//이 오브젝트가 씬이 바뀔때에도 삭제되지 않게 함
    }
}
