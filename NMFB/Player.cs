using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //弾を発射するオブジェクトの位置を取得
    //public Transform firePoint;

    //弾をオブジェクトとして取得
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] GameObject exComEffect;
    private float speed;//M：9 F:4 min：1
    private float muscleSpeed = 9.0f;
    private float fatSpeed = 4.0f;
    private float allOutSpeed = 1.0f;
    private int muscleItemGetNum = 0;
    private int changeModeBorder = 3;
    private float bulletInterval = 0.5f;
    private float time;
    public float bulletSpeed = 5.0f;
    private float exerciseCommandLimitTime = 0.1f;
    private bool isInput;
    public  float xLimitMax = 8.4f;
    public float xLimitMin = -8.5f;
    public float yLimitMax = 3.7f;
    public float yLimitMin = -4.3f;
    private Vector3 hyperLaserPos1;
    private Vector3 hyperLaserPos2;
    private String command;
    [SerializeField] public Score scoreScript;
    [SerializeField] public StaminaGauge staminaGaugeScript;
    [SerializeField] ClearScore clearScoreScript;
    private float limitFatTime = 5.0f;
    public CameraMove cameraScipt;
    public int bulletDamage;
    bool isBlinkStaminaGauge;

    public bool isAllOut = false;
    bool isFatforSE = true;
    bool isMuscleforSE = false;
    bool isAllOutforSE = false;
    public bool isHit = false;
    public bool isClear = false;

    [SerializeField] GeneralAudioSource generalAudioSrc;

    [SerializeField] GameObject explosionEffect;
    SpriteRenderer playerSprite;
    [SerializeField] Sprite fatSprite;
    [SerializeField] Sprite muscleSprite;
    public enum Mode
    {
        MUSCLE,
        FAT,
        ALLOUT,
    }
    public Mode currentMode;

    public enum Laser
    {
        DEFAULT,
        HYPER,
    }
    public Laser currentLaserMode;

    public static Player instance;

    /// <summary>
    /// tmp
    /// </summary>
    public float spawnTimeSecond = 0;
    public float tmpTime = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        transform.position = new Vector3(-8.5f, 0, 0);
        muscleSpeed = 9.0f;
        fatSpeed = 4.0f;
        allOutSpeed = 1.0f;
        muscleItemGetNum = 0;
        changeModeBorder = 3;
        bulletInterval = 0.5f;
        bulletSpeed = 5.0f;
        exerciseCommandLimitTime = 0.1f;
        xLimitMax = 8.4f;
        xLimitMin = -8.5f;
        yLimitMax = 3.7f;
        yLimitMin = -4.3f;

        currentMode = Mode.FAT;//.................FAT
        currentLaserMode = Laser.DEFAULT;//...........Defa
        time = bulletInterval;
        bulletInterval = 0.5f;
        limitFatTime = 5.0f;
        speed = fatSpeed;//スタートがFAT
        isAllOut = false;
        isBlinkStaminaGauge = false;
        bulletDamage = 50;

        playerSprite = GetComponent<SpriteRenderer>();
        playerSprite.sprite = fatSprite;
        exComEffect.SetActive(false);
}

    void Update()
    {
        Restart();
        if (!isClear)
        {
            tmpTime += Time.deltaTime;
            spawnTimeSecond = (float)Math.Round(tmpTime * 10) / 10;
            Shot();
            Move();
            //ExerciseCommand();
            StartCoroutine("ExerciseCommand");
            JudgeMode();
        }
        else if(isClear)
        {
            StartCoroutine("ClearFunc");
        }
    }

    //移動の処理
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, xLimitMin, xLimitMax);
        currentPos.y = Mathf.Clamp(currentPos.y, yLimitMin, yLimitMax);

        //追加　positionをcurrentPosにする
        transform.position = currentPos;
    }

    //弾の処理
    void Shot()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                if(currentLaserMode == Laser.DEFAULT)
                {
                    Instantiate(playerBulletPrefab, this.transform.position, transform.rotation);
                }
                else if(currentLaserMode == Laser.HYPER)
                {
                    hyperLaserPos1 = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
                    hyperLaserPos2 = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
                    Instantiate(playerBulletPrefab, hyperLaserPos1, transform.rotation);
                    Instantiate(playerBulletPrefab, hyperLaserPos2, transform.rotation);
                }
                generalAudioSrc.ShotSEFunc();
                time = bulletInterval;
            }
        }
    }

    public void MuscleItemGetCount()
    {
        staminaGaugeScript.currentStamina -= 10;//筋トレアイテムをとってもスタミナゲージを減らす（現状10）
        generalAudioSrc.MuscleItemGetSEFunc();
        if (currentMode == Mode.FAT)
        {
            muscleItemGetNum += 1;
        }
        else if(currentMode == Mode.MUSCLE)
        {
            //もし、レーザーが未強化ならレーザーを強化
            if(currentLaserMode == Laser.DEFAULT)
            {
                //Debug.Log("レーザー強化");
                generalAudioSrc.LaserPowerUpSEFunc();
                currentLaserMode = Laser.HYPER;
            }
            else
            {
                //そうではなくて、レーザーが強化状態なら、スコアを獲得
                scoreScript.GetScoreConversion(200f);//とりあえず300にしてみる　MUSCLE時、1.5倍されるので200にした（変化可能性）
            }
        }
    }

    public void FoodItemGetCount()
    {
        //腹減りゲージを変化させるもの どのモードであっても腹減りゲージは増加させる
        staminaGaugeScript.currentStamina += 30;
        generalAudioSrc.FoodItemGetSEFunc();
    }

    void JudgeMode()
    {
        if(currentMode == Mode.FAT)//F→Mになる
        {
            ModeChange();
            if (muscleItemGetNum >= changeModeBorder)
            {
                muscleItemGetNum = 0;
                currentMode = Mode.MUSCLE;
                ModeChange();
            }
        }
        else if(currentMode == Mode.MUSCLE)
        {
            ModeChange();
        }

        if(isAllOut == true)
        {
            ModeChange();
        }

        /////////////////
        //スタミナゲージの変化によるモードの変化について
        /////////////////
        if (staminaGaugeScript.isStart == false && staminaGaugeScript.currentStamina >= 100)
        {
            //もし腹減りゲージがMAXになったら、即、FATモードに変化させる
            staminaGaugeScript.currentStamina = 100;
            if (isBlinkStaminaGauge == true)
            {
                staminaGaugeScript.StopCoroutine("BlinkStaminaGauge");
                staminaGaugeScript.StaminaGaugeColorReset();
            }
            isBlinkStaminaGauge = false;
            muscleItemGetNum = 0;
            staminaGaugeScript.isStart = true;//１回FATモードになってしまったら、そのあと減らして行った時にすぐBlinkしないようにするため
            currentMode = Mode.FAT;
        }
        else if (staminaGaugeScript.isStart == false && staminaGaugeScript.currentStamina >= staminaGaugeScript.maxStamina * 0.85)
        {
            //そうじゃなくて、腹減りゲージが7割を超えていたら、その後３秒間、腹減りゲージが7割を超えたままなら、FATモードへ以降するようにする（変更）85
            if(!isBlinkStaminaGauge)
            {
                staminaGaugeScript.StartCoroutine("BlinkStaminaGauge");
                isBlinkStaminaGauge = true;
                generalAudioSrc.BuzzerSEFunc();
            }
            limitFatTime -= Time.deltaTime;
            if (limitFatTime < 0)
            {
                staminaGaugeScript.StopCoroutine("BlinkStaminaGauge");
                isBlinkStaminaGauge = false;
                staminaGaugeScript.isStart = true;//１回FATモードになってしまったら、そのあと減らして行った時にすぐBlinkしないようにするため
                muscleItemGetNum = 0;
                staminaGaugeScript.StaminaGaugeColorReset();
                currentMode = Mode.FAT;
            }
        }
        else
        {
            staminaGaugeScript.StopCoroutine("BlinkStaminaGauge");
            isBlinkStaminaGauge = false;
            staminaGaugeScript.StaminaGaugeColorReset();
            limitFatTime = 5.0f;
        }
    }

    public void ModeChange()
    {
        CircleCollider2D cirC2D = GetComponent<CircleCollider2D>();
        if(currentMode == Mode.FAT)
        {
            if(!isFatforSE)
            {
                generalAudioSrc.DamageSSEFunc();
                isFatforSE = true;
                isMuscleforSE = false;
            }
            playerSprite.sprite = fatSprite;
            speed = fatSpeed;
            currentLaserMode = Laser.DEFAULT;
            cirC2D.radius = 0.5f;
            bulletSpeed = 5.0f;
            bulletInterval = 0.5f;
        }
        else if(currentMode == Mode.MUSCLE)
        {
            if(!isMuscleforSE)
            {
                generalAudioSrc.PowerUpSEFunc();
                isMuscleforSE = true;
                isFatforSE = false;
            }
            playerSprite.sprite = muscleSprite;
            speed = muscleSpeed;
            cirC2D.radius = 0.25f;
            bulletSpeed = 9.0f;
            bulletInterval = 0.2f;
        }

        //一番下にあることが大事
        if(isAllOut == true)
        {
            if(!isAllOutforSE)
            {
                generalAudioSrc.AlloutSEFunc();
                isAllOutforSE = true;
            }
            speed = allOutSpeed;
        }
        else
        {
            isAllOutforSE = false;
        }
    }

    public IEnumerator DamageBlink()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float interval = 0.2f;// 点滅周期
        int blinkCount = 10;//5回点滅させたいので（非表示＋表示　で、5回分　2*5=10）

        //cirC2D.enabled = false;
        isHit = true;
        scoreScript.AddScore(-1500);
        generalAudioSrc.DamageLSEFunc();
        if(staminaGaugeScript.currentStamina <= 0)
        {
            staminaGaugeScript.currentStamina += 10;
        }
        for (int i=0; i< blinkCount; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(interval);
        }
        isHit = false;
    }

    IEnumerator ExerciseCommand()
    {
        exerciseCommandLimitTime -= Time.deltaTime;
        if (exerciseCommandLimitTime < 0)
        {
            isInput = false;
            command = "";
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            exerciseCommandLimitTime = 0.8f;
            isInput = true;
            command = "u";
        }
        if (isInput)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                command = command + "r";
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                command = command + "d";
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                command = command + "l";
            }
        }
        if (command == "urdl" && staminaGaugeScript.currentStamina > 0)
        {
            //腹減りゲージを能動的に減らすことができる
            //Debug.Log("コマンド入力成功");
            command = "";
            staminaGaugeScript.currentStamina -= 2;
            //コマンド入力に成功すると、能動的にスタミナゲージを減らすことができる
            //かつ、MUSCLEモード時に、コマンド入力によってスタミナを減少させられた場合、スコアが50ポイント加算される（変化可能性）
            exComEffect.SetActive(true);
            if (currentMode == Mode.MUSCLE)
            {
                //Debug.Log("MUSCLEモードで、コマンド入力成功");
                generalAudioSrc.ExerciseSEFunc();
                scoreScript.AddScore(100f);
            }
            yield return new WaitForSeconds(0.2f);
            exComEffect.SetActive(false);
        }
    }

    public void DestroyEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(explosionEffect, pos, Quaternion.identity);
        Destroy(effect, 1.0f);
    }

    IEnumerator ClearFunc()
    {
        CircleCollider2D cirC2D = GetComponent<CircleCollider2D>();
        cirC2D.enabled = false;
        yield return new WaitForSeconds(2.0f);
        AfterClearFunc();
    }

    void AfterClearFunc()
    {
        float speed = 5.0f;
        if (transform.position.x <= 10f)
        {
            transform.position += new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        }
    }

    void Restart()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            tmpTime = 0;
            isClear = false;
            isAllOut = false;
            isFatforSE = true;
            isMuscleforSE = false;
            isAllOutforSE = false;
            isHit = false;
            currentMode = Mode.FAT;
            currentLaserMode = Laser.DEFAULT;
            CircleCollider2D cirC2D = GetComponent<CircleCollider2D>();
            cirC2D.enabled = true;
            scoreScript.scoreText.SetText("SCORE:0");
            clearScoreScript.scoreText.SetText("0");
            exComEffect.SetActive(false);
            scoreScript.Sumscore = 0;
            clearScoreScript.tmpScore = 0;
            StopCoroutine("ClearFunc");
            StopCoroutine("clearScoreScript.CountUP");
            clearScoreScript.scoreText.enabled = false;
            clearScoreScript.restartText.enabled = false;
            clearScoreScript.isSE = false;
            clearScoreScript.medalSprite.sprite = default;
            staminaGaugeScript.staminaGaugeSlider.value = 100;
            staminaGaugeScript.currentStamina = staminaGaugeScript.maxStamina;
            JsonLoader.instance.index = 0;
            transform.position = new Vector3(-8.5f, 0, 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
