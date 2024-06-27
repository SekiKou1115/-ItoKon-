using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToSpawn;
    [SerializeField] Transform spawnPosition; 
    [SerializeField] float spawnInterval; // インターバル
    [SerializeField] float objectLifetime; // 生成したオブジェクトの生存時間

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        GameObject spawnedObject = Instantiate(objectsToSpawn[randomIndex], spawnPosition.position, spawnPosition.rotation);
        Destroy(spawnedObject, objectLifetime);
    }
}
