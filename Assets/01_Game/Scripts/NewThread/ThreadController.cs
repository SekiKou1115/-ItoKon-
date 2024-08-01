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
    [Header("アタッチ")]
    [SerializeField] private GameObject _groom;
    [SerializeField] private GameObject _bride;
    [SerializeField] private ObiParticleAttachment _groomOpa;
    [SerializeField] private ObiParticleAttachment _brideOpa;

    [Header("ステータス")]
    [SerializeField, Tooltip("最大距離")] private float _maxDist;
    [SerializeField, Tooltip("通常時質量")] private float _normalMass = 0.01f;
    [SerializeField, Tooltip("最大距離時質量")] private float _pullMass = 10f;

    // -------------------------------- PrivateField
    // 最低距離
    private const float _minDist = 1f;

    // 参照用変数
    private ObiRope _rope;
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
        _rope = GetComponent<ObiRope>();
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
        if (_minDist < dist && dist < _maxDist)
        {
            _cursor.ChangeLength(dist);
        }

        // 最大距離時の移動の制限
        if (dist >= _maxDist)
        {
            _rope.SetMass(_normalMass);
        }
        else
        {
            _rope.SetMass(_pullMass);
        }

        // 糸を張る
        if(PlayerManager.Instance.IsGrab)
        {
            _rope.stretchingScale = 0.1f;
        }
        else
        {
            _rope.stretchingScale = 1.05f;
        }
    }
}
