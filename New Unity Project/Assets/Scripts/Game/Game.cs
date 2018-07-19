using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private float _timeLimit;

    [SerializeField] private Number _scorer;

    [SerializeField] private Number _timeNum;

    [SerializeField] private GameObject _resultBoard;

    [SerializeField] private Text _text;

    [SerializeField] private AudioClip _pon;

    [SerializeField] private Mirror _mirror;

    private AudioSource _auioSource;

    private float _time;

    private int _score;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        _time = _timeLimit;

        _auioSource = GetComponent<AudioSource>();

        _score = 0;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        _time -= Time.deltaTime;

        _timeNum.Set(GetTime());

        _scorer.Set(_score);

        if (GetTime() == 0)
        {
            _resultBoard.SetActive(true);
            _text.text = "" + _score;
            _mirror.SetAlpha(0);
        }
    }

    /// <summary>
    /// スコアを追加します。
    /// </summary>
    /// <param name="score"> 追加スコア </param>
    /// <returns> 追加後のスコア </returns>
    public int AddScore(int score)
    {
        _auioSource.PlayOneShot(_pon);

        _score += score;

        return _score;
    }

    /// <summary>
    /// ゲーム時間を取得します。
    /// </summary>
    /// <returns> ゲーム時間 </returns>
    public int GetTime()
    {
        var t = Mathf.FloorToInt(_time);

        return t < 0 ? 0 : t;
    }

    /// <summary>
    /// ゲームシーンをロードします。
    /// </summary>
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
}
