using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfNumber : MonoBehaviour
{
    private SpriteRenderer _spr;

    [SerializeField] private List<Sprite> _numberSpriteList;

    public void Start()
    { 
        _spr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 座標を設定します。
    /// </summary>
    /// <param name="pos"></param>
    public void SetPos(Vector2 pos)
    {
        transform.localPosition = pos;
    }

    /// <summary>
    /// 数字を設定します。
    /// </summary>
    /// <param name="n">  </param>
    /// <returns></returns>
    public float SetNumber(int n)
    {
        if (_spr == null)
        {
            _spr = GetComponent<SpriteRenderer>();
        }

        if (n < 0 || n > 9)
        {
            Debug.LogError("Number");
            return _spr.sprite.rect.width;
        }

        _spr.enabled = true;

        _spr.sprite = _numberSpriteList[n];
        return _spr.sprite.rect.width;
    }

    /// <summary>
    /// 数字を非表示にします。
    /// </summary>
    public void Hide()
    {
        if (_spr == null)
        {
            _spr = GetComponent<SpriteRenderer>();
        }

        _spr.enabled = false;
    }
}
