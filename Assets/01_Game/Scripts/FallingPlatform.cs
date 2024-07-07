using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f; // Delay 時間
    [SerializeField] private float destroyDelay = 2f; // Destroy 時間
    private Rigidbody rb;
    private bool hasFallen = false;
    private ObjectRespawn _respawner;
    private Vector3 spawnPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        _respawner = FindObjectOfType<ObjectRespawn>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !hasFallen)
        {
            spawnPosition = transform.position; // 現在のPosを保持
            Invoke("Fall", fallDelay);
            Invoke("DestroyPlatform", fallDelay + destroyDelay);
            hasFallen = true;
        }
    }

    void Fall()
    {
        rb.isKinematic = false; 
    }

    void DestroyPlatform()
    {
       
        
        if (_respawner != null)
        {
            _respawner.RespawnPlatform(spawnPosition); // respawn managerの関数を呼び出す
        }
        Destroy(gameObject); // Destroy 
    }
}
