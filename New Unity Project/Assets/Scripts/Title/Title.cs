using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private List<GameObject> _ballPrefabList;

    private List<GameObject> _ballList;

    /// <summary>
    /// ゲームシーンをロードします。
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
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
    /// ボールを追加します。
    /// </summary>
    private void GenerateBall()
    {
        var ball = Instantiate(_ballPrefabList[Random.Range(0, _ballPrefabList.Count)], GetGeneratePos(),
            Quaternion.identity);
        ball.transform.parent = transform;

        _ballList.Add(ball);
    }

    private void Start()
    {
        _ballList = new List<GameObject>();
    }

    private void Update()
    {
        if (Random.Range(0, 30) == 0)
        {
            GenerateBall();
        }

        while (_ballList.Remove(null)) { }

        foreach (var ball in _ballList)
        {
            if (ball.transform.position.y < -5.5f)
            {
                Destroy(ball);
            }   
        }
    }
}
