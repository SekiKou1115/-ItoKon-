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
    [Header("�A�^�b�`")]
    [SerializeField] private GameObject _groom;
    [SerializeField] private GameObject _bride;
    [SerializeField] private ObiParticleAttachment _groomOpa;
    [SerializeField] private ObiParticleAttachment _brideOpa;

    [Header("�X�e�[�^�X")]
    [SerializeField, Tooltip("�ő勗��")] private float _maxDist;
    [SerializeField, Tooltip("�ʏ펞����")] private float _normalMass = 0.01f;
    [SerializeField, Tooltip("�ő勗��������")] private float _pullMass = 10f;

    // -------------------------------- PrivateField
    // �Œ዗��
    private const float _minDist = 1f;

    // �Q�Ɨp�ϐ�
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
        // �v���C���[2�l�̋������擾
        var dist = Vector3.Distance(
            _groom.transform.position,
            _bride.transform.position) - 1f;

        // �L�����N�^�[�ύX���Ă������̒��߂��\�ɂ��邽��
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

        // �����̕ύX
        if (_minDist < dist && dist < _maxDist)
        {
            _cursor.ChangeLength(dist);
        }

        // �ő勗�����̈ړ��̐���
        if (dist >= _maxDist)
        {
            _rope.SetMass(_normalMass);
        }
        else
        {
            _rope.SetMass(_pullMass);
        }

        // ���𒣂�
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
