using System;
using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectsSpawnerInObjectPool : MonoBehaviour
{
    // HACK:只能作为用法展示，因为示例场景物理运算占用太多了
    
    public TestObj prefab;
    [Tooltip("发射初速")] public float ejectSpeed = 5;
    [Tooltip("物体发射频率")] public float spawnRate = 0.5f;
    [Tooltip("启动物体发射")] public bool enableSpawn;
    [Space]
    
    // 在编辑器窗口实时查看对象池情况
    public int countInactive;
    public int countActive;
    public int countAll;
    public float pressure;

    private float spawnTimer;
    private IObjectPool<TestObj> objPool;    
    private IObjectPoolOptimizer poolOptimizer;
    
    // Start is called before the first frame update
    void Start()
    {
        // 对象池
        objPool = new BaseObjectPool<TestObj>(CreateObj,OnTakeOut,OnRelease,OnDestroyObj,true,100,10000);
        // 对象池自动优化器
        poolOptimizer = new ObjectPoolOptimizer<TestObj>(objPool);
        poolOptimizer.SetOptimizeRate(10);
    }

    #region 创建对象池所需配置

    private TestObj CreateObj()
    {
        var temp = Instantiate(prefab,this.transform);
        temp.gameObject.SetActive(false);
        return temp;
    }

    private void OnTakeOut(TestObj element)
    {
        element.transform.position = transform.position;
        element.gameObject.SetActive(true);
        element.MyRigidbody.velocity = Random.onUnitSphere * ejectSpeed;
        element.SetDestroyAction(() =>
        {
            objPool.Release(element);
        });
    }

    private void OnRelease(TestObj element)
    {
        element.ResetState();
        element.gameObject.SetActive(false);
    }

    private void OnDestroyObj(TestObj element)
    {
        Destroy(element.gameObject);
    }

    #endregion

    private void UpdateData()
    {
        countActive = objPool.CountActive;
        countInactive = objPool.CountInactive;
        countAll = objPool.CountActive + objPool.CountInactive;
        
        pressure = poolOptimizer.PressureUsing;
    }

    private void SpawnObjectsOnce()
    {
        if (spawnTimer < spawnRate)
        {
            spawnTimer += Time.deltaTime;
            return;
        }
        spawnTimer = 0;
        
        for (int i = 0; i < 100; i++)
        {
            objPool.Get();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateData();
        // 持续调用，优化器将按设定频率定期执行优化算法
        poolOptimizer.Update();
        
        if (!enableSpawn) return;
        SpawnObjectsOnce();
    }

    private void OnDestroy()
    {
        objPool.Clear();
    }
}
