using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ball : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;

    [SerializeField] private List<Ball> _colBallList;

    [SerializeField] private Sprite     _normalSprite;
    [SerializeField] private Sprite     _deadSprite;

    private Game         _game;

    private Rigidbody2D _rigidbody2D;

    private AudioSource _audioSource;

    private SpriteRenderer _spriteRenderer;

    private float _deadTime;

    public Ball Parent;

    public int  Num;

    public bool IsBanished;


    /// <summary>
    /// Gameを設定
    /// </summary>
    /// <param name="g"> ゲーム </param>
    public void SetGame(Game g)
    {
        _game = g;
    }

    /// <summary>
    /// ２次元ベクトルの内積を求めます。
    /// </summary>
    /// <param name="v1"> ベクトル１ </param>
    /// <param name="v2"> ベクトル２ </param>
    /// <returns>  </returns>
    private static float CrossVector2(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }



    /// <summary>
    /// ボールが生成可能な座標か示します。
    /// </summary>
    /// <param name="v"> 座標 </param>
    /// <returns> 可能なとき true , 不可能なとき false </returns>
    private static bool IsGenerateAble(Vector2 v)
    {
        return Mathf.Abs(v.x) <= 2.5 && Mathf.Abs(v.y) <= 4.0;
    }



    /// <summary>
    /// 鏡でボールをコピーします。
    /// </summary>
    /// <param name="firstPoint"> 一つ目の座標 </param>
    /// <param name="secondPoint"> 二つ目の座標 </param>
    /// <returns> 生成されたボール 生成できないときnullを返します。 </returns>
    public Ball MirrorCopyThisBall(Vector2 firstPoint, Vector2 secondPoint)
    {
        var p = new Vector2(transform.position.x, transform.position.y);

        var l = (firstPoint - secondPoint).normalized;

        var n = new Vector2(l.y, -l.x).normalized;

        var mirrorPos = 2 * CrossVector2(firstPoint - p, n) * n + p;

        if (!IsGenerateAble(mirrorPos))
        {
            return null;
        }

        var generatedObject =
            Instantiate(_ballPrefab, new Vector3(mirrorPos.x, mirrorPos.y, 0), Quaternion.identity);

        generatedObject.transform.parent = transform.parent;

        var a = new Vector2(Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z),
            Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z));

        var b = 2 * CrossVector2(a, l) * l - a;

        generatedObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(b.y, b.x) + 180);

        var bc = generatedObject.GetComponent<Ball>();

        bc.SetGame(_game);

        return bc;
    }



    /// <summary>
    /// 座標を取得します。
    /// </summary>
    /// <returns> 座標 </returns>
    public Vector2 GetPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }



    /// <summary>
    /// 衝突しているボールのリスト
    /// </summary>
    /// <returns></returns>
    public List<Ball> GetColBallList()
    {
        return _colBallList;
    }



    /// <summary>
    /// このオブジェクトを破棄します。
    /// </summary>
    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }



    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        _colBallList = new List<Ball>();

        _audioSource = GetComponent<AudioSource>();

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _normalSprite;

        IsBanished = false;

        _deadTime = 0;
    }



    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        if (IsBanished)
        {
            _spriteRenderer.sprite = _deadSprite;

            _deadTime += Time.deltaTime;

            _spriteRenderer.sprite = Mathf.FloorToInt(_deadTime * 4f) % 2 == 0
                ? _deadSprite
                : _normalSprite;

            tag = "Banished";

            if (_deadTime > 1f)
            {
                _game.AddScore(100);

                DestroyThisObject();
            }
        }

        if (_rigidbody2D.velocity.magnitude > 3.0f&&!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }



    /// <summary>
    /// 衝突時の処理
    /// </summary>
    /// <param name="other"> 衝突した物体 </param>
    public void OnTriggerEnter2D(Collider2D other)
    { 
        if (!CompareTag(other.gameObject.tag))
        {
            return;
        }

        var colBall = other.gameObject.GetComponent<Ball>();

        if (_colBallList.Contains(colBall))
        {
            return;
        }

        _colBallList.Add(colBall);
    }


    /// <summary>
    /// 衝突時の処理
    /// </summary>
    /// <param name="other"> 衝突した物体 </param>
    public void OnTriggerStay2D(Collider2D other)
    {
        if (!CompareTag(other.gameObject.tag))
        {
            return;
        }

        var colBall = other.gameObject.GetComponent<Ball>();

        if (_colBallList.Contains(colBall))
        {
            return;
        }

        _colBallList.Add(colBall);
    }


    /// <summary>
    /// 衝突時の処理
    /// </summary>
    /// <param name="other"> 衝突した物体 </param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!CompareTag(other.gameObject.tag))
        {
            return;
        }

        var colBall = other.gameObject.GetComponent<Ball>();

        if (!_colBallList.Contains(colBall))
        {
            return;
        }

        _colBallList.Remove(colBall);
    }
}
