using UnityEngine;

public class LeverController : MonoBehaviour
{
    // -------------------------------- SerializeField
    [SerializeField] private GameObject _activedPos;

    // -------------------------------- PrivateField
    private bool[] _barFlag = new bool[2];
    private GameObject _point;
    private bool _isActived = false;
    private LeverHitCheck[] _pointsArray = new LeverHitCheck[2];

    // -------------------------------- UnityMassege
    private void Start()
    {
        _point = transform.parent.gameObject;

        for (int i = 0; i <= 1; i++)
        {
            _pointsArray[i] = transform.GetChild(i).GetComponent<LeverHitCheck>();
            _pointsArray[i].ID = i;
        }
    }

    // -------------------------------- PbulicMethod
    public void CheckPoint(int value, bool isInPoint)
    {
        if(isInPoint)
        {
            if (_barFlag[0] && !_barFlag[1])
            {
                TwitchOn();
            }
            else
            {
                _barFlag[value] = true;
            }
        }
        else
        {
            _barFlag[value] = false;
        }
    }

    // -------------------------------- PrivateMethod
    /// <summary>
    /// アクティブ処理
    /// </summary>
    private void TwitchOn()
    {
        if (!_isActived)
        {
            _point.transform.rotation = _activedPos.transform.rotation;
            // 行いたい関数
            _isActived = true;
        }
    }
}
