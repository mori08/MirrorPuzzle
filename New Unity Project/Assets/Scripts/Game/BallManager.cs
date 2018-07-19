using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class BallManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> _ballPrefabList;

    [SerializeField] private List<Ball> _ballList;

    [SerializeField] private Mirror _mirror;

    [SerializeField] private Game _game;

    private const int MaxBallNum = 40;

    private int _mirrorState;

    private Vector2 _firstPoint;

    private Vector2 _secondPoint;

    private const int WaitState = 0;

    private const int SetMirrorState = 1;

    private const int ActionMirrorState = 2;

    private const int BallBanishedNum = 4;

    private int _time;



    /// <summary>
    /// マウスで指示した座標をワールド座標に変換してかえします。
    /// </summary>
    /// <returns> ワールド座標 </returns>
    private static Vector2 GetWorldMousePosition()
    {
        var position = Input.mousePosition;

        var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

        return new Vector2(screenToWorldPointPosition.x, screenToWorldPointPosition.y);
    }



    /// <summary>
    /// ボールを生成する座標を生成します。
    /// </summary>
    /// <returns> 座標 </returns>
    private static Vector3 GetGeneratePos()
    {
        var x = Random.Range(-2.5f, 2.5f);

        return new Vector3(x, 5.5f, 0);
    }



    /// <summary>
    /// ボールを生成します。
    /// </summary>
    /// <returns> 生成したGameObject </returns>
    private void GenerateBall()
    {

        var ballId = new List<List<int>>(6);
        
        for (var i = 0; i < 6; ++i)
        {
            ballId.Add(new List<int>(6));
            for (var j = 0; j < 6; ++j)
            {
                ballId[i].Add(-1);
            }
        }
        
        for (var x = 0; x < 6; ++x)
        {
            for (var y = 0; y < 6; ++y)
            {
                var used1 = x == 0 ? -1 : ballId[x - 1][y];
                var used2 = y == 0 ? -1 : ballId[x][y - 1];
                var used3 = x == 0 || y == 0 ? -1 : ballId[x - 1][y - 1];
                var used4 = x == 0 || y == 5 ? -1 : ballId[x - 1][y + 1];

                var id = Random.Range(0, _ballPrefabList.Count);

                while (id == used1 || id == used2 || id == used3 || id == used4)
                {
                    id = Random.Range(0, _ballPrefabList.Count);
                }

                ballId[x][y] = id;
            }
        }

        for (var x = 0; x < 6; ++x)
        {
            for (var y = 0; y < 6; ++y)
            {
                var ball = Instantiate(_ballPrefabList[ballId[x][y]],
                    new Vector3(-2.5f + 1f * x, 3.5f + 1f * y, 0), Quaternion.identity);

                var bc = ball.GetComponent<Ball>();
                bc.SetGame(_game);
                _ballList.Add(bc);
            }
        }
        
    }



    /// <summary>
    /// _ballListを削除します。
    /// </summary>
    private void EraseBrokenObject()
    {
        while (_ballList.Remove(null))
        {
        }

        _ballList.RemoveAll(ball => ball.IsBanished);
    }



    /// <summary>
    /// 鏡を使ってボールを変化させる。
    /// </summary>
    private void MirrorAction()
    {
        var subVector = _secondPoint - _firstPoint;

        var n = new Vector2(subVector.y, -subVector.x);

        var addList = new List<Ball>();

        foreach (var ball in _ballList)
        {
            var subBallVector = ball.GetPosition() - _firstPoint;

            if (Mirror.CrossVector2(subVector, subBallVector) > 0)
            {
                ball.DestroyThisObject();
            }
            else
            {
                var b = ball.MirrorCopyThisBall(_firstPoint, _firstPoint + n);

                addList.Add(b);
            }
        }

        foreach (var ball in addList)
        {
            _ballList.Add(ball);
        }
        
    }



    /// <summary>
    /// 鏡を管理します。
    /// </summary>
    private void ControlMirror()
    {
        if (_mirrorState == WaitState)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _firstPoint = GetWorldMousePosition();
                _mirrorState = SetMirrorState;
                _secondPoint = _firstPoint + Vector2.up;
            }
        }
        else if (_mirrorState == SetMirrorState)
        {
            if ((_firstPoint - GetWorldMousePosition()).magnitude > 0.5f)
            {
                _secondPoint = GetWorldMousePosition();
            }

            _mirror.Setting(_firstPoint, _secondPoint);

            if (Input.GetMouseButtonUp(0))
            {
                _mirrorState = ActionMirrorState;
                _time = 0;
            }
        }
        else if (_mirrorState == ActionMirrorState)
        {
            ++_time;

            if (_time < 30)
            {
                _mirror.SetAlpha((30 + _time) / 60f);
            }
            else if (_time == 30)
            {
                MirrorAction();
            }
            else if (_time < 90)
            {
                _mirror.SetAlpha((90 - _time) / 60f);
            }
            else
            {
                _mirror.SetAlpha(0);
                _mirrorState = WaitState;
            }
        }
    }



    private void Start()
    {
        _ballList = new List<Ball>();

        _mirrorState = WaitState;

        _time = 30;
    }



    private void Update()
    {
        if (_game.GetTime() == 0)
        {
            while (_ballList.Remove(null))
            {
            }
            foreach (var ball in _ballList)
            {
                ball.DestroyThisObject();
            }
            return;
        }

        EraseBrokenObject();

        BanishBall();

        ControlMirror();

        if (_ballList.Count == 0)
        {
            GenerateBall();
        }

        ++_time;
    }



    /// <summary>
    /// ボールのunion-find-tree用のメンバを初期化する。
    /// </summary>
    /// <param name="ball"> 初期化したいボール </param>
    private static void make_set(Ball ball)
    {
        ball.Parent = ball;

        ball.Num = 1;

        ball.IsBanished = false;
    }



    /// <summary>
    /// ルートノードを探します。
    /// </summary>
    /// <param name="ball"> ボール </param>
    /// <returns> ルートノード </returns>
    private static Ball Find(Ball ball)
    {
        while (ball.Parent != ball.Parent.Parent)
        {
            ball.Parent = ball.Parent.Parent;
        }

        return ball.Parent;
    }



    /// <summary>
    /// 和集合を作ります。
    /// </summary>
    /// <param name="ball1"> ボール１ </param>
    /// <param name="ball2"> ボール２ </param>
    private static void Union(Ball ball1,Ball ball2)
    {
        ball1 = Find(ball1);

        ball2 = Find(ball2);

        if (ball1 == ball2)
        {
            return;
        }

        ball1.Parent = ball2;

        ball2.Num = ball1.Num + ball2.Num;
    }



    private void BanishBall()
    {
        foreach (var ball in _ballList)
        {
            make_set(ball);
        }

        foreach (var ball in _ballList)
        {
            foreach (var colBall in ball.GetColBallList())
            {
                Union(ball, colBall);
            }
        }

        foreach (var ball in _ballList)
        {
            if (Find(ball).Num >= BallBanishedNum)
            {
                ball.IsBanished = true;
            }
        }
    }

}
