using UnityEngine;

public class StableAspect : MonoBehaviour {

    private Camera _cam;

    // 画像のサイズ
    private const float Width = 640f;
    private const float Height = 960f;

    // 画像のPixel Per Unit
    private const float PixelPerUnit = 100f;

    private void Awake()
    {
        var aspect = (float)Screen.height / Screen.width;
        const float bgAcpect = Height / Width;

        // カメラコンポーネントを取得します
        _cam = GetComponent<Camera>();

        // カメラのorthographicSizeを設定
        _cam.orthographicSize = Height / 2f / PixelPerUnit;


        if (bgAcpect > aspect)
        {
            // 倍率
            var bgScale = Height / Screen.height;

            // viewport rectの幅
            var camWidth = Width / (Screen.width * bgScale);

            // viewportRectを設定
            _cam.rect = new Rect((1f - camWidth) / 2f, 0f, camWidth, 1f);
        }
        else
        {
            // 倍率
            var bgScale = Width / Screen.width;

            // viewport rectの幅
            var camHeight = Height / (Screen.height * bgScale);

            // viewportRectを設定
            _cam.rect = new Rect(0f, (1f - camHeight) / 2f, 1f, camHeight);
        }
    }
}
