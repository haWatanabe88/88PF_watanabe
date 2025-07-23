using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine.EventSystems;

public class FreeHand : MonoBehaviour
{

    /// <summary>
    /// 描く線のコンポーネントリスト
    /// </summary>
    private List<LineRenderer> lineRendererList;

    /// <summary>
    /// 描く線のマテリアル
    /// </summary>
    public Material lineMaterial;

    /// <summary>
    /// 描く線の色
    /// </summary>
    public Color lineColor;

    /// <summary>
    /// 描く線の太さ
    /// </summary>
    [Range(0.1f, 0.5f)] public float lineWidth;

    /// <summary>
    /// 線の両端の丸みを変更するため
    /// </summary>
    private LineRenderer lr;
    [Header("両端の太さ")]
    [SerializeField] int _myNumCapVertices = 100;

    /// <summary>
    /// mouseのpositionを取得したくて、
    /// </summary>
    [SerializeField] float minPosX;
    [SerializeField] float minPosY;
    [SerializeField] float maxPosX;
    [SerializeField] float maxPosY;
    [SerializeField] bool isMeasure = false;

    public float GetMinPosX => minPosX;
    public float GetMinPosY => minPosY;
    public float GetMaxPosX => maxPosX;
    public float GetMaxPosY => maxPosY;

    Camera cam;
    public Screenshot Screenshot;
    private GameObject clickedGameObject;


    private Vector2 startPos, endPos;//（個人的に加えた部分）
    private float _createLineLength = 0.1f;//0.2fも試したが、0.1fが一番良かったので

    private bool isBtn = false;

    void Awake()
    {
        lineRendererList = new List<LineRenderer>();
    }

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    void Update()
    {
        // ボタンが押された時に線オブジェクトの追加を行う
        if (Input.GetMouseButtonDown(0))
        {
            isBtn = false;
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
            {
                isBtn = true;
                return;
            }
#endif
#if UNITY_IOS
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                isBtn = true;
                return;
            }
#endif
            //Debug.Log("Clicked on the UI");
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//（個人的に加えた部分）
            this.AddLineObject();
        }

        // ボタンが押されている時、LineRendererに位置データの設定を指定していく
        if(!isBtn && Input.GetMouseButton(0))
        {
            //endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.AddPositionDataToLineRendererList();
        }

        if (!isBtn && Input.GetMouseButtonUp(0))
        {
            this.ConvertWorldToScreen();
        }
    }
    /// <summary>
    /// 線オブジェクトの追加を行うメソッド
    /// </summary>
    private void AddLineObject()
    {

        // 追加するオブジェクトをインスタンス
        GameObject lineObject = new GameObject();

        //追加オブジェクトに名前をつける
        lineObject.name = "line";

        // オブジェクトにLineRendererを取り付ける
        lineObject.AddComponent<LineRenderer>();

        //オブジェクトの両端を丸める（追記）
        lr = lineObject.GetComponent<LineRenderer>();
        lr.numCapVertices = _myNumCapVertices;
        // 描く線のコンポーネントリストに追加する
        lineRendererList.Add(lineObject.GetComponent<LineRenderer>());

        // 線と線をつなぐ点の数を0に初期化
        lineRendererList.Last().positionCount = 0;
        lineRendererList.Last().positionCount += 1;//（個人的に加えた部分）

        // マテリアルを初期化
        lineRendererList.Last().material = this.lineMaterial;

        // 線の色を初期化
        lineRendererList.Last().material.color = this.lineColor;

        // 線の太さを初期化
        lineRendererList.Last().startWidth = this.lineWidth;
        lineRendererList.Last().endWidth = this.lineWidth;
    }

    /// <summary>
    /// 描く線のコンポーネントリストに位置情報を登録していく
    /// </summary>
    private void AddPositionDataToLineRendererList()
    {

        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//（個人的に加えた部分）

        // 座標の変換を行いマウス位置を取得
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1.0f);
        var mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);

        /////////////
        if (!isMeasure)//初期化の作業
        {
            //max側の初期化の方法は工夫したが、もしかしたらmin側も工夫する必要が出てくるかもしれない
            minPosX = Screen.width * 2;
            minPosY = Screen.height * 2;
            maxPosX = mousePosition.x;//マウスをドラッグさせた瞬間の座標で初期化するようにした
            maxPosY = mousePosition.y;
            isMeasure = true;
        }
        if (mousePosition.x < minPosX)//各々で更新させないと、「７」とかの時ダメだったので修正した
        {
            minPosX = mousePosition.x;
        }
        else if (maxPosX < mousePosition.x)
        {
            maxPosX = mousePosition.x;
        }
        if (mousePosition.y < minPosY)
        {
            minPosY = mousePosition.y;
        }
        else if (maxPosY < mousePosition.y)
        {
            maxPosY = mousePosition.y;
        }
        /////////////
        //線と線をつなぐ点の数を更新
        if ((endPos - startPos).magnitude > _createLineLength)//（個人的に加えた部分）0.1f
        {
            //Debug.Log("mag");
            //Debug.Log((endPos - startPos).magnitude);
            startPos = endPos;
            //Debug.Log("（足す前）positionCount = " + lineRendererList.Last().positionCount);
            //（個人的に加えた部分）・・・終了
            lineRendererList.Last().positionCount += 1;
        }

        // 描く線のコンポーネントリストを更新
        lineRendererList.Last().SetPosition(lineRendererList.Last().positionCount - 1, mousePosition);
    }

    private void ConvertWorldToScreen()
    {
        ///追記・・・World→Screenに変換するときに、Vector3型である必要があるので、変換する必要がある。
        Vector3 tmpMin = new Vector3(minPosX, minPosY, 0);
        Vector3 tmpMax = new Vector3(maxPosX, maxPosY, 0);
        Vector3 tmpMinScreen = cam.WorldToScreenPoint(tmpMin);
        Vector3 tmpMaxScreen = cam.WorldToScreenPoint(tmpMax);
        //Debug.Log("以下、tmpMinScreenとtmpMaxScreenの値");
        //Debug.Log(tmpMinScreen);
        //Debug.Log(tmpMaxScreen);
    }
}