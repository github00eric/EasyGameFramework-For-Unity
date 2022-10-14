using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectsSpawner : MonoBehaviour
{
    public TestObj prefab;
    [Tooltip("发射初速")] public float ejectSpeed = 5;
    [Tooltip("物体发射频率")] public float spawnRate = 0.5f;
    [Tooltip("启动物体发射")] public bool enableSpawn;

    private float spawnTimer;

    private TestObj CreateObj()
    {
        var temp = Instantiate(prefab,this.transform);
        temp.transform.position = transform.position;
        temp.MyRigidbody.velocity = Random.onUnitSphere * ejectSpeed;
        temp.SetDestroyAction(() =>
        {
            Destroy(temp.gameObject);
        });
        temp.ResetState();

        return temp;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!enableSpawn) return;
        
        if (spawnTimer < spawnRate)
        {
            spawnTimer += Time.deltaTime;
            return;
        }
        spawnTimer = 0;
        
        for (int i = 0; i < 100; i++)
        {
            CreateObj();    //objPool.Get();
        }
    }
}
