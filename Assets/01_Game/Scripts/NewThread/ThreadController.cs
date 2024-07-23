using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using Unity.VisualScripting;

public class ThreadController : MonoBehaviour
{
    // -------------------------------- Singleton
    public static ThreadController Instance;

    // -------------------------------- SerializeField
    [SerializeField] private GameObject _groom;
    [SerializeField] private GameObject _bride;
    [SerializeField] private ObiParticleAttachment _groomOpa;
    [SerializeField] private ObiParticleAttachment _brideOpa;
    [SerializeField] private float _maxDist;

    // -------------------------------- PrivateField
    private const float _minDist = 1f;

    private ObiRopeCursor _cursor;

    // -------------------------------- Property
    public float GetSetMaxDist
    {
        get => _maxDist;
        set => _maxDist = value;
    }

    // -------------------------------- UnityMassege
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _cursor = GetComponent<ObiRopeCursor>();
    }

    private void Update()
    {
        // プレイヤー2人の距離を取得
        var dist = Vector3.Distance(
            _groom.transform.position,
            _bride.transform.position) - 1f;

        // キャラクター変更しても長さの調節を可能にするため
        if (PlayerManager.Instance.MovePlayerName == PlayerManager.Name.GROOM)
        {
            _groomOpa.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
            _brideOpa.attachmentType = ObiParticleAttachment.AttachmentType.Static;
        }
        else
        {
            _groomOpa.attachmentType = ObiParticleAttachment.AttachmentType.Static;
            _brideOpa.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
        }

        // 長さの変更
        if ((_minDist < dist && dist < _maxDist)
            && !PlayerManager.Instance.IsGrab)
        {
            _cursor.ChangeLength(dist);
        }

        Debug.Log(gameObject.GetComponent<ObiRope>().restLength);

        // 長さ上限時の移動の制限
        if (dist >= _maxDist || PlayerManager.Instance.IsGrab)
        {
            _groom.GetComponent<Rigidbody>().mass = 1f;
            _bride.GetComponent<Rigidbody>().mass = 1f;
        }
        else
        {
            _groom.GetComponent<Rigidbody>().mass = 100f;
            _bride.GetComponent<Rigidbody>().mass = 100f;
        }
    }
}
