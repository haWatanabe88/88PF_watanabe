using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{

    [Header("保存先の設定")]
    [SerializeField]
    string folderName = "Screenshots";
    bool isCreatingScreenShot = false;
    string path;
    GameObject clickObject;
    //private int trimScreenSize = 600;

    bool isCreatingCaptureShot = false;
    [SerializeField] float minPosX;
    [SerializeField] float minPosY;
    [SerializeField] float maxPosX;
    [SerializeField] float maxPosY;
    public FreeHand lineManagerScript;
    [SerializeField] float diffWidth;
    [SerializeField] float diffHeight;
    Camera cam;
    [SerializeField] int adjSize = 20;//切り取りサイズの調整のための変数___より広く切り取りたい場合、これを調整すると良き

    //public bool isBtn = false;




    //処理実行は、関数を呼ぶことでしか実行できない（まず、main関数が呼ばれる必要があるから）ので、代入する時は、関数で代入する必要がある
    //class直下においては、宣言と初期化しかできない（代入ができない）
    public void SetMinMaxPos()
    {
        this.minPosX = lineManagerScript.GetMinPosX;
        this.minPosY = lineManagerScript.GetMinPosY;
        this.maxPosX = lineManagerScript.GetMaxPosX;
        this.maxPosY = lineManagerScript.GetMaxPosY;
    }
    ///minPosとmaxPosをWorld→Screenに変換するときに、Vector3型である必要があるので、変換する必要がある。
    ///LineManagerから受け取ってから変換するようにしてみるため
    public void ConvertWorldToScreen()
    {
       
        Vector3 tmpMin = new Vector3(this.minPosX, this.minPosY, 0);
        Vector3 tmpMax = new Vector3(this.maxPosX, this.maxPosY, 0);
        Vector3 tmpMinScreen = cam.WorldToScreenPoint(tmpMin);
        Vector3 tmpMaxScreen = cam.WorldToScreenPoint(tmpMax);
        this.minPosX = tmpMinScreen.x;
        this.minPosY = tmpMinScreen.y;
        this.maxPosX = tmpMaxScreen.x;
        this.maxPosY = tmpMaxScreen.y;
    }


    void Start()
    {
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
        lineManagerScript = GameObject.Find("FreeHand").GetComponent<FreeHand>();
        //path = Application.dataPath + "/" + folderName + "/";
        path = Application.persistentDataPath + "/" + folderName + "/";
        //Debug.Log(Application.persistentDataPath);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {

        //SetMinMaxPos();//ここで変数に代入する
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("押された");
            PrintScreen();
        }
        /*
        else if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("spaceおした");
            clickObject = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitTgt = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if(hitTgt)
            {
                clickObject = hitTgt.transform.gameObject;
                if(clickObject.tag == "ScreenShotbtn")
                {
                    //Debug.Log(Application.dataPath);
                    clickObject = null;
                    PrintScreen();
                }
                else if(clickObject.tag == "Clearbtn")//これはスクショとは関係ないけど、とりあえず
                {
                    AllDEstroy();
                }
            }
        }
        */

    }
    public void PrintScreen()
    {
        //isBtn = true;//ボタンが押されたことを示す
        //Debug.Log("isBtnがtrueになった");
        SetMinMaxPos();//ここで変数に代入する
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);//フォルダの作成
            string filename = "MYTEST.txt";
            FileInfo fileInfo = new FileInfo(path+filename);
            FileStream fileStream = fileInfo.Create();//ファイルの作成
        }
        //StartCoroutine("PrintScreenInternal");
        //StartCoroutine(CaptureCoroutine(trimScreenSize, trimScreenSize));
        StartCoroutine("CaptureCoroutine");
    }

    IEnumerator PrintScreenInternal()
    {
        if (isCreatingScreenShot)
        {
            yield break;
        }
        //SetMinMaxPos();//ここで変数に代入する
        isCreatingScreenShot = true;

        yield return null;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }

        string date = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss");
        string fileName = path + date + ".png";

        ScreenCapture.CaptureScreenshot(fileName);//第２引数に数字を指定すると、サイズを大きくして撮影することができる

        yield return new WaitUntil(() => File.Exists(fileName));

        isCreatingScreenShot = false;
    }

    //////////////////////////////////////////
    //以下、スクショで指定範囲（以下のソースの場合は、画面中央付近）のみ切り抜く
    //////////////////////////////////////////
    protected virtual IEnumerator CaptureCoroutine()
    {
        if (isCreatingCaptureShot)
        {
            yield break;
        }
        //SetMinMaxPos();//ここで変数に代入する

        ConvertWorldToScreen();//ここで、w→sに変換する
        isCreatingCaptureShot = true;

        //Debug.Log("以下,,PrintScreen実行直後の、minX,minY, maxX, maxY");//setMinMaxPosでW→Sにしたものを書いてある
        //Debug.Log(minPosX);
        //Debug.Log(minPosY);
        //Debug.Log(maxPosX);
        //Debug.Log(maxPosY);
        string date = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss");
        string fileName = path + date + ".png";
        // カメラのレンダリング待ち
        yield return new WaitForEndOfFrame();
        Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
        // 切り取る画像の左下位置を求める
        //int x = (tex.width - width) / 2;
        //int y = (tex.height - height) / 2;
        diffWidth = (maxPosX - minPosX) + adjSize * 2;//adjSize = 20
        diffHeight = (maxPosY - minPosY) + adjSize * 2;
        //Debug.Log("以下minx_miny_diffwid_diffhei");//これは切り取る範囲を指定している
        //Debug.Log(minPosX);
        //Debug.Log(minPosY);
        //Debug.Log(diffWidth);
        //Debug.Log(diffHeight);
        Color[] colors = tex.GetPixels((int)(minPosX - adjSize), (int)(minPosY - adjSize), (int)diffWidth, (int)diffHeight);
        Texture2D saveTex = new Texture2D((int)diffWidth, (int)diffHeight, TextureFormat.ARGB32, false);//defo：ARGB32/RHalf
        saveTex.SetPixels(colors);
        File.WriteAllBytes(fileName, saveTex.EncodeToJPG());//ファイル生成の形式を変更できる
    }
    /*
    private void AllDEstroy()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("line");

        foreach (GameObject line in lines)
        {
            lineManagerScript.ResetFunc();
            isCreatingCaptureShot = false;//再度撮影できるようにするために、Ckearボタンを押したらここのfalseに変更する
            Destroy(line);
        }
    }
    */
}