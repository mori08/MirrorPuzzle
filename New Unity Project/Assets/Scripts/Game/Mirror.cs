using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Mirror : MonoBehaviour
{
    [SerializeField] private GameObject _mirrorPanel;

    [SerializeField] private SpriteRenderer _mirrorPanelSpriteRenderer;

    [SerializeField] private float _mirrorPanelSize;

    private SpriteRenderer _sprite;

    private AudioSource _audioSource;



    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();

        _audioSource = GetComponent<AudioSource>();

        _sprite.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// ２次元ベクトルの内積を求めます。
    /// </summary>
    /// <param name="v1"> ベクトル１ </param>
    /// <param name="v2"> ベクトル２ </param>
    /// <returns>  </returns>
    public static float CrossVector2(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }

    /// <summary>
    /// 鏡を表示する
    /// </summary>
    /// <param name="basePos"> 基準点 </param>
    /// <param name="directionPos">  </param>
    public void Setting(Vector2 basePos, Vector2 directionPos)
    {
        _audioSource.Play();

        _sprite.color = new Color(1, 1, 1, 0.8f);

        var l = (directionPos - basePos).normalized;

        var n = new Vector2(l.y, -l.x);

        var direction = CrossVector2(basePos, l) + _mirrorPanelSize;

        transform.position = new Vector3(direction * l.x, direction * l.y, -3);

        transform.eulerAngles = new Vector3(0, 0,
            Mathf.Rad2Deg * Mathf.Atan2(n.y, n.x));
    }

    /// <summary>
    /// 不透明度を設定する
    /// </summary>
    /// <param name="a"> α地 </param>
    public void SetAlpha(float a)
    {
        if (a < 0.3f)
        {
            _audioSource.Stop();   
        }

        var c = _sprite.color;
        c.a = a;
        _sprite.color = c;
    }


    /*
    /// <summary>
    /// 鏡を設置します。
    /// </summary>
    /// <param name="firstPos"> 第一座標 </param>
    /// <param name="secondPos"> 第二座標 </param>
    public void Set(Vector2 firstPos, Vector2 secondPos)
    {
        _audioSource.Play();

        _sprite.enabled = true;

        var cp = (firstPos + secondPos) / 2.0f;

        transform.position = new Vector3(cp.x, cp.y, -1);

        transform.eulerAngles = new Vector3(0, 0,
            Mathf.Rad2Deg * Mathf.Atan2((firstPos - secondPos).y, (firstPos - secondPos).x));
    }

    

    /// <summary>
    /// 鏡のパネルを設置します。
    /// </summary>
    /// <param name="firstPos"> 第一座標 </param>
    /// <param name="secondPos"> 第二座標 </param>
    /// <param name="thirdPos"> 第三座標 </param>
    public void SetMirrorPanel(Vector2 firstPos, Vector2 secondPos, Vector2 thirdPos)
    {
        _mirrorPanelSpriteRenderer.enabled = true;

        var l = (secondPos - firstPos).normalized;

        var n = new Vector2(l.y, -l.x);

        var d = CrossVector2(thirdPos - firstPos, n) / Mathf.Abs(CrossVector2(thirdPos - firstPos, n));

        var distance = CrossVector2(firstPos, n) + _mirrorPanelSize * d;

        _mirrorPanel.transform.position = new Vector3(distance * n.x, distance * n.y, -3);

        _mirrorPanel.transform.eulerAngles = transform.eulerAngles;
    }



    /// <summary>
    /// 鏡を非表示にします。
    /// </summary>
    public void Hide()
    {
        _audioSource.Stop();

        _sprite.enabled = false;

        _mirrorPanelSpriteRenderer.enabled = false;
    }



    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();

        _audioSource = GetComponent<AudioSource>();
    }
    */
}
