using System.Collections;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    public GameObject platformPrefab;
    public float respawnTime = 5f; // Time after which the platform respawns

    public void RespawnPlatform(Vector3 position)
    {
        StartCoroutine(Respawn(position));
    }

    private IEnumerator Respawn(Vector3 position)
    {
        yield return new WaitForSeconds(respawnTime);
        Instantiate(platformPrefab, position, Quaternion.identity);
    }
}
