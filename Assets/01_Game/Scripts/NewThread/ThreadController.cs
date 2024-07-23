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
        if ((_minDist < dist && dist < _maxDist)
            && !PlayerManager.Instance.IsGrab)
        {
            _cursor.ChangeLength(dist);
        }

        Debug.Log(gameObject.GetComponent<ObiRope>().restLength);

        // ����������̈ړ��̐���
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
