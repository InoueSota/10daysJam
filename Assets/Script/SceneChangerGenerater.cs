using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangerGenerater : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool Loaded { get; set; }

    [SerializeField]
    GameObject[] gameManagerPrefabs = null;

    void Awake()
    {
        //すでにロード済みなら、二重に作成しない
        if (Loaded) return;

        Loaded = true;

        //プレハブをインスタンス化して、DontDestroyOnLoad
        foreach (var prefab in gameManagerPrefabs)
        {
            GameObject go = Instantiate(prefab);
            DontDestroyOnLoad(go);
        }
    }
}
