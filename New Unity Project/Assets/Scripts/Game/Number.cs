using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime.DynamicDispatching;
using UnityEngine;

public class Number : MonoBehaviour
{
    [SerializeField] private int _digit;

    [SerializeField] private GameObject _partOfNumberPrefab;

    [SerializeField] private int _value;

    private List<PartOfNumber> _numberObjectList;

    public void Start()
    {
        _numberObjectList = new List<PartOfNumber>();

        var rest = _value;

        for (var i = 0; i < _digit; ++i)
        {
            var numObject
                = Instantiate(_partOfNumberPrefab, transform.position, Quaternion.identity);

            numObject.transform.parent = transform;

            var num = numObject.GetComponent<PartOfNumber>();

            var r = rest % 10;
            rest /= 10;

            if (i != 0 && r == 0)
            {
                num.Hide();
            }
            else
            {
                num.SetNumber(r);
            }

            _numberObjectList.Add(num);
        }
    }

    /// <summary>
    /// 表示したい値を設定します。
    /// </summary>
    /// <param name="v"> 表示したい値 </param>
    public void Set(int v)
    {
        if (v == _value) return;

        var plus = (v - _value) / Mathf.Abs(v - _value) + (v - _value) / 30;

        _value += plus;

        if (_value == 0)
        {
            foreach (var num in _numberObjectList)
            {
                num.Hide();
            }

            _numberObjectList[0].SetNumber(0);

            _numberObjectList[0].SetPos(new Vector3(0, 0, 0));

            return;
        }

        var pos = 0f;
        var rest = _value;

        foreach (var num in _numberObjectList)
        {
            if (rest == 0)
            {
                num.Hide();
                continue;
            }

            var r = rest % 10;
            rest /= 10;

            var width = num.SetNumber(r);

            pos -= width / 200f;

            num.SetPos(new Vector3(pos,0));

            pos -= width / 200f;
        }
    }
}
