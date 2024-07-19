using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Platforming : MonoBehaviour
{
    public GameObject plat = null;
    public GameObject center = null;
    public bool m_isGround = false;
    [SerializeField] Vector3 origin;
    [SerializeField] float radius;
    [SerializeField] float footPos;
    [SerializeField] float groundCheckDistance_onPlat;
    [SerializeField] float groundCheckDistance_up;
    [SerializeField] float groundCheckDistance_down;
    Quaternion pastRot;
    Vector3 pastPos;
    Vector3 pastSca;
    Quaternion nowRot;
    Vector3 nowPos;
    Vector3 nowSca;
    public LayerMask m_layerMask;
    Rigidbody m_rigidbody;
    void Start()
    {
        m_rigidbody = transform.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (plat != null)
        {
            if (pastSca.x <= 0 || pastSca.y <= 0 || pastSca.z <= 0 || nowSca.x <= 0 || nowSca.y <= 0 || nowSca.z <= 0)
            {   //plat��Scale��0�ȉ��̏ꍇ��MoveWith���Ȃ�
                plat = null;
                center = null;
                return;
            }
            if (center != null)
            {   //Center����
                nowRot = center.transform.rotation;
                nowPos = center.transform.position;
                nowSca = center.transform.lossyScale;
                MoveWith();
                pastRot = center.transform.rotation;
                pastPos = center.transform.position;
                pastSca = center.transform.lossyScale;
            }
            else
            {   //Center�Ȃ�
                nowRot = plat.transform.rotation;
                nowPos = plat.transform.position;
                nowSca = plat.transform.lossyScale;
                MoveWith();
                pastRot = plat.transform.rotation;
                pastPos = plat.transform.position;
                pastSca = plat.transform.lossyScale;
            }
        }
        GroundCheck();
        //�v���C���[�𐂒��ɖ߂�
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
    }
    public void MoveWith()
    {
        Vector3 player_pastPos = transform.position;
        //Rotate
        Quaternion deltaRot = nowRot * Quaternion.Inverse(pastRot);
        transform.RotateAround(nowPos, new Vector3(1, 0, 0), deltaRot.eulerAngles.x);
        transform.RotateAround(nowPos, new Vector3(0, 1, 0), deltaRot.eulerAngles.y);
        transform.RotateAround(nowPos, new Vector3(0, 0, 1), deltaRot.eulerAngles.z);
        //Move
        Vector3 deltaMove = (nowPos - pastPos);
        //Scale
        Vector3 deltaSca = new Vector3((nowSca.x - pastSca.x) / pastSca.x, (nowSca.y - pastSca.y) / pastSca.y, (nowSca.z - pastSca.z) / pastSca.z);
        Vector3 playerDis = Quaternion.Inverse(nowRot) * (transform.position - nowPos);
        deltaSca = nowRot * new Vector3(playerDis.x * deltaSca.x, playerDis.y * deltaSca.y, playerDis.z * deltaSca.z);
        //�v���C���[�̈ʒu��Move��Scale�̌v�Z���ʁi�ړ��ʁj�������āA�ړ�
        transform.position += deltaMove + deltaSca;
        //Y��������ւ͏��ɉ������͂ŏオ��̂ŁA�����ł͈ړ������Ȃ�
        if (transform.position.y - player_pastPos.y > 0)
        {
            transform.position = new Vector3(transform.position.x, player_pastPos.y, transform.position.z);
        }
    }
    public void GroundCheck()
    {
        //�v���C���[�̏�Ԃɍ��킹��Ray�̒�����ݒ�
        float rayDistance = origin.y - footPos - radius;
        if (plat != null)
        {
            rayDistance += groundCheckDistance_onPlat;
        }
        else if (m_rigidbody.velocity.y > 0)
        {
            rayDistance += groundCheckDistance_up;
        }
        else
        {
            rayDistance += groundCheckDistance_down;
        }
        //������Ray�𔭎˂��ď����m�F
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + origin, radius, Vector3.down, out hit, rayDistance, m_layerMask))
        {
            m_isGround = true;
            if ((plat != null && hit.collider.name == plat.name))
            {
                //Debug.Log("������: " + hit.collider.name);
                return;
            }
            else if (hit.collider.transform.CompareTag("Ground") /*|| hit.collider.transform.CompareTag("Center")*/)
            {
                //Debug.Log("�V������: " + hit.collider.name);
                plat = hit.collider.gameObject;
                pastRot = plat.transform.rotation;
                pastPos = plat.transform.position;
                pastSca = plat.transform.lossyScale;
                nowSca = plat.transform.lossyScale;
                //�e��center�����邩���ׂ�
                GameObject p = plat;
                while (p.CompareTag("Ground") && p.transform.parent != null)
                {
                    //if (p.transform.parent.CompareTag("Center"))
                    //{
                    //    center = p.transform.parent.gameObject;
                    //    //Debug.Log("center: " + center.name);
                    //    pastRot = center.transform.rotation;
                    //    pastPos = center.transform.position;
                    //    pastSca = center.transform.lossyScale;
                    //    nowSca = center.transform.lossyScale;
                    //    return;
                    //}
                    //else
                    //{
                    //    p = p.transform.parent.gameObject;
                    //}
                    p = p.transform.parent.gameObject;
                }
            }
            else
            {
                //Debug.Log("����������Ȃ�: " + hit.collider.name);
                plat = null;
                center = null;
            }
        }
        else
        {
            //Debug.Log("��");
            plat = null;
            center = null;
            m_isGround = false;
        }
    }
}