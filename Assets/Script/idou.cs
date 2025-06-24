using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idou : MonoBehaviour
{
    // 速度に関するパラメータ
    public float fast_speed;  // 賽銭箱の前まで行く時のスピード
    public float FastPositionSpeed_y;  // 左のポジションまで移動する時のスピード
    public float SecondPositionSpeed_x, SecondPositionSpeed_y;  // 真ん中のポジションまで移動する時のスピード
    public float ThirdPositionSpeed_x, ThirdPositionSpeed_y;  // 右のポジションまで移動する時のスピード
    private int temp;  // オブジェクトがどこに三箇所のどこに移動したかを保持するための変数
    // 現在の状態を示す数値
    private int phase = 10;  // switch文の条件文のとこに入るやつ
    // 一時的な変数
    private int item;  // このオブジェクトがどの運勢と紐づけられているかを保持する変数
    private int recv_;  // TxtFileRead.csから取得したれrecvの値を格納する変数
    private bool isCalledOnce = false;
    //表情の差分
    public Sprite Default;  // 正面
    public Sprite oinori;  // 御祈り
    public Sprite atari;  // あたり
    public Sprite hazure;  //　ハズレ
    public float SpriteChangeTime;
    public float time_sos;
    public float TimeBonus;
    public float resetTime; // タイマー用の変数
    public AudioClip sound_atari;  // 当たりの時の効果音
    public AudioClip sound_hazure;  // ハズレの時の効果音
    //以下2行でcppから受け取った値を十の位と一の位に分ける
    private int ten;
    private int one;   

    score Score;
    God god;
    TxtFileRead txtFileRead;
    prefab_ramdam prefab_Ramdam;
    AudioSource audioSource;

    private GameObject childObj_hukidasi;
    private GameObject childObj_un;
    private GameObject childObj_TextHyaku;
    private GameObject childObj_TextNihyaku;
    private GameObject childObj_TextSanhyaku;
    private float TimeBonus_time;
    public float TimeBonus_timeInterval;
    void Start()
    {
        TimeBonus_time = time_sos;
        resetTime = SpriteChangeTime; // timeとresetTimeの値を同じにする
        // 以下4行でスポーンした時に紐づけられる運勢を取得する
        prefab_ramdam prefab_Ramdam;
        GameObject GMobj = GameObject.Find("GameManager");
        prefab_Ramdam = GMobj.GetComponent<prefab_ramdam>();
        item = prefab_Ramdam.number_item;

        audioSource = GetComponent<AudioSource>();  // AudioSourceのコンポーネントを取得する
    }
    void Update()
    {
        SpriteRenderer MainSpriteRenderer;
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        Vector2 position = transform.position;  // オブジェクトの現在位置を取得

        //以下7行で"GameManager"のオブジェクトの中にあるscore.csとprefab_ramdam.csとTxtFileRead.csを取得する
        // score Score;
        // TxtFileRead txtFileRead;
        // prefab_ramdam prefab_Ramdam;
        GameObject GMobj = GameObject.Find("GameManager");
        Score = GMobj.GetComponent<score>();
        txtFileRead = GMobj.GetComponent<TxtFileRead>();
        prefab_Ramdam = GMobj.GetComponent<prefab_ramdam>();

        recv_ = txtFileRead.recv; // TxtFileRead.csがcppから受け取った値の読み取り // 十の位が列、一の位が運勢（色）

        ten = recv_ / 10;
        one = recv_ % 10;

        Debug.Log("十の位：" + ten + " 一の位：" + one);

        Debug.Log("recvの値を取得してすぐの値：" + recv_);
        // 神様オブジェクトを取得し、God.csを取得する
        GameObject obj_GOD = GameObject.Find("神様");
        god = obj_GOD.GetComponent<God>();
        // 吹き出しのオブジェクトの取得
        GameObject obj_0 = GameObject.Find("吹き出し(Clone)");
        // 以下3行で吹き出しの中に生成するオブジェクトの取得
        GameObject obj_1 = GameObject.Find("恋愛1(Clone)");
        GameObject obj_2 = GameObject.Find("金1(Clone)");
        GameObject obj_3 = GameObject.Find("学1(Clone)");

        // 吹き出しのオブジェクトの取得
        GameObject obj_00 = GameObject.Find("吹き出し(左向き)_0(Clone)");
        // 以下3行で吹き出しの中に生成するオブジェクトの取得
        GameObject obj_11 = GameObject.Find("恋愛1 _半透明(Clone)");
        GameObject obj_22 = GameObject.Find("金1 _半透明(Clone)");
        GameObject obj_33 = GameObject.Find("学1  _半透明(Clone)");

        // phaseの値に基づいて異なる処理を実行
        // 移動してから消えるまでの処理
        // phase10からスタート
        switch (phase)
        {
            // スコアの加算とイラストの差し替え
            //恋愛運（赤）の時に実行
            case 0:
                TimeBonus += Time.deltaTime;
                time_sos -= Time.deltaTime;
                if(time_sos <= 0) 
                {
                    if(temp == 11)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4.5f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-7.2f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                    else if(temp == 12)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-6.6f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                    else if(temp == 13)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4.5f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-7f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                }
                
                //Debug.Log("ケース０で待機");
                if ((Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Q) && temp == 11) || (ten == 1 && one == 1 && temp == 11))  // 左の列でキーボードの「１とq」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 100;
                    god.result = 1;
                }
                else if ((Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.Q) && temp == 12) || (ten == 2 && one == 1 && temp == 12)) // 真ん中の列でキーボードの「2とq」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 200;
                    god.result = 1;
                }
                else if ((Input.GetKey(KeyCode.Alpha3) && Input.GetKey(KeyCode.Q) && temp == 13) || (ten == 3 && one == 1 && temp == 13)) // 右の列でキーボードの「3とq」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 300;
                    god.result = 1;
                }
                //===================================================================================================
                    else if((Input.GetKey(KeyCode.Alpha1) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E)) && temp == 11) || (ten == 1 && (one == 2 || one == 3) && temp == 11))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 100;
                        god.result = 2;
                    }
                    else if((Input.GetKey(KeyCode.Alpha2) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E)) && temp == 12) || (ten == 2 && (one == 2 || one == 3) && temp == 12))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 200;
                        god.result = 2;
                    }
                    else if((Input.GetKey(KeyCode.Alpha3) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E)) && temp == 13) || (ten == 3 && (one == 2 || one == 3) && temp == 13))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 300;
                        god.result = 2;
                    }
                //===================================================================================================
                break;
            // 金運（緑）の時に実行
            case 1:
                TimeBonus += Time.deltaTime;
                time_sos -= Time.deltaTime;
                if(time_sos <= 0) 
                {
                    if(temp == 11)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4.5f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-7.2f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                    else if(temp == 12)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-6.6f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                    else if(temp == 13)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4.5f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-7f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                }
                
                //Debug.Log("ケース１で待機");
                if ((Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.W) && temp == 11) || (ten == 1 && one == 2 && temp == 11))  //  左の列でキーボードの「１とw」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 100;
                    god.result = 1;
                }
                else if ((Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.W) && temp == 12) || (ten == 2 && one == 2 && temp == 12)) //  真ん中の列でキーボードの「2とw」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 200;
                    god.result = 1;
                }
                else if ((Input.GetKey(KeyCode.Alpha3) && Input.GetKey(KeyCode.W) && temp == 13) || (ten == 3 && one == 2 && temp == 13)) //  右の列でキーボードの「3とw」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 300;
                    god.result = 1;
                }
                //=============================================================================
                    else if((Input.GetKey(KeyCode.Alpha1) && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) && temp == 11) || (ten == 1 && (one == 1 || one == 3) && temp == 11))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 100;
                        god.result = 2;
                    }
                    else if((Input.GetKey(KeyCode.Alpha2) && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) && temp == 12) || (ten == 2 && (one == 1 || one == 3) && temp == 12))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 200;
                        god.result = 2;
                    }
                    else if((Input.GetKey(KeyCode.Alpha3) && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) && temp == 13) || (ten == 3 && (one == 1 || one == 3) && temp == 13))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 300;
                        god.result = 2;
                    }
                //==============================================================================
                break;
            // 学業運（青）の時に実行
            case 2:
                TimeBonus += Time.deltaTime;
                time_sos -= Time.deltaTime;
                if(time_sos <= 0) 
                {
                    if(temp == 11)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4.5f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-7.2f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                    else if(temp == 12)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-6.6f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                    else if(temp == 13)
                    {
                        if (!isCalledOnce) 
                        {
                            isCalledOnce = true;
                            childObj_hukidasi = Instantiate(prefab_Ramdam.hukidasi_hantoumei, this.transform);  // 吹き出しの生成
                            childObj_un = Instantiate(prefab_Ramdam.prefab_item_hantoumei[item], this.transform); //吹き出しの中身の生成

                            childObj_hukidasi.transform.localPosition = new Vector3(-4.5f, 2f, 0);
                            childObj_hukidasi.transform.localRotation = Quaternion.identity;

                            childObj_un.transform.localPosition = new Vector3(-7f, -4.2f, 0);
                            childObj_un.transform.localRotation = Quaternion.identity;
                        }
                    }
                }
                
                //Debug.Log("ケース２で待機");
                if ((Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.E) && temp == 11) || (ten == 1 && one == 3 && temp == 11))  //  左の列でキーボードの「１とe」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 100;
                    god.result = 1;
                }
                else if ((Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.E) && temp == 12) || (ten == 2 && one == 3 && temp == 12)) //  真ん中の列でキーボードの「2とe」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 200;
                    god.result = 1;
                }
                else if ((Input.GetKey(KeyCode.Alpha3) && Input.GetKey(KeyCode.E) && temp == 13) || (ten == 3 && one == 3 && temp == 13)) //  右の列でキーボードの「3とe」で実行
                {
                    //Debug.Log("削除");
                    audioSource.PlayOneShot(sound_atari);
                    if(TimeBonus <= TimeBonus_time)
                    {
                        Score.can += 300;
                        childObj_TextSanhyaku = Instantiate(prefab_Ramdam.TextSanhyaku, this.transform);  // 吹き出しの生成
                        childObj_TextSanhyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextSanhyaku.transform.localRotation = Quaternion.identity;
                    }
                    else if(TimeBonus > TimeBonus_time && TimeBonus <= TimeBonus_time + TimeBonus_timeInterval)
                    {
                        Score.can += 200;
                        childObj_TextNihyaku = Instantiate(prefab_Ramdam.TextNihyaku, this.transform);  // 吹き出しの生成
                        childObj_TextNihyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextNihyaku.transform.localRotation = Quaternion.identity;
                    } 
                    else 
                    {
                        Score.can += 100;
                        childObj_TextHyaku = Instantiate(prefab_Ramdam.TextHyaku, this.transform);  // 吹き出しの生成
                        childObj_TextHyaku.transform.localPosition = new Vector3(0, 4.5f, 0);
                        childObj_TextHyaku.transform.localRotation = Quaternion.identity;
                    } 
                    MainSpriteRenderer.sprite = atari;
                    phase = 300;
                    god.result = 1;
                }
                //=====================================================================
                    else if((Input.GetKey(KeyCode.Alpha1) && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W)) && temp == 11) || (ten == 1 && (one == 1 || one == 2) && temp == 11))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 100;
                        god.result = 2;
                    }
                    else if((Input.GetKey(KeyCode.Alpha2) && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W)) && temp == 12) || (ten == 2 && (one == 1 || one == 2) && temp == 12))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 200;
                        god.result = 2;
                    }
                    else if((Input.GetKey(KeyCode.Alpha3) && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W)) && temp == 13) || (ten == 3 && (one == 1 || one == 2) && temp == 13))
                    {
                        audioSource.PlayOneShot(sound_hazure);
                        Score.can += 0;
                        MainSpriteRenderer.sprite = hazure;
                        phase = 300;
                        god.result = 2;
                    }
                //=======================================================================
                break;
            // 初期状態　賽銭箱の前まで移動
            case 10:
                // オブジェクトを左に移動
                position.x -= fast_speed;
                transform.position = position;
                // オブジェクトが一定の位置に到達した場合の処理
                if (position.x <= -5)
                {
                    Instantiate(prefab_Ramdam.hukidasi, new Vector3(-2.8f, -0.5f, 0), Quaternion.identity);  // 吹き出しの生成
                    Instantiate(prefab_Ramdam.prefab_item[item], new Vector3(-2.8f, -0.5f, 0), Quaternion.identity); //吹き出しの中身の生成
                    // 次の状態をランダムに決定
                    prefab_Ramdam.afterTheBox = 1;
                    phase = 1000/*Random.Range(11, 14)*/;
                    MainSpriteRenderer.sprite = oinori;
                    // Debug.Log("num = " + num);
                }
                break;
            // 一番左の列まで移動
            case 11:
                // オブジェクトを左下に移動
                position.y -= FastPositionSpeed_y;
                transform.position = position;
                if (position.x <= -5 && position.y < -3)
                {
                    // 次の状態を配列の値に基づいて決定
                    prefab_Ramdam.PosLefte = 1;
                    phase = item;
                    // Debug.Log("num = " + num);
                }
                break;
            // 真ん中の列まで移動
            case 12:
                // オブジェクトを右下に移動
                position.y -= SecondPositionSpeed_y;
                position.x += SecondPositionSpeed_x;
                transform.position = position;
                if (position.x >= 0 && position.y < -3)
                {
                    // 次の状態を配列の値に基づいて決定
                    prefab_Ramdam.PosCentre = 1;
                    phase = item;
                    // Debug.Log("num = " + num);
                }
                break;
            // 一番右の列に移動
            case 13:
                // オブジェクトを右下に移動
                position.y -= ThirdPositionSpeed_y;
                position.x += ThirdPositionSpeed_x;
                transform.position = position;
                if (position.x >= 0 && position.y < -3)
                {
                    // 次の状態を配列の値に基づいて決定
                    prefab_Ramdam.PosRight = 1;
                    phase = item;
                    // Debug.Log("num = " + num);
                }
                break;
            //　オブジェクトの削除
            case 100:
                SpriteChangeTime -= Time.deltaTime;
                if(SpriteChangeTime <= 0.0f)
                {
                    prefab_Ramdam.PosLefte = 0;
                    Destroy(this.gameObject);   //  このスクリプトがアタッチされているオブジェクトを消す
                }
                
                break;
            case 200:
                SpriteChangeTime -= Time.deltaTime;
                if(SpriteChangeTime <= 0.0f)
                {
                    prefab_Ramdam.PosCentre = 0;
                    Destroy(this.gameObject);   //  このスクリプトがアタッチされているオブジェクトを消す
                }
                break;
            case 300:
                SpriteChangeTime -= Time.deltaTime;
                if(SpriteChangeTime <= 0.0f)
                {
                    prefab_Ramdam.PosRight = 0;
                    Destroy(this.gameObject);   //  このスクリプトがアタッチされているオブジェクトを消す
                }
                break;
            // 賽銭箱の前で停止
            case 1000:
                SpriteChangeTime -= Time.deltaTime;
                transform.position = position;
                if(SpriteChangeTime <= 0.0f)
                {
                    int a;
                    a = Random.Range(0, 3);
                    switch(a){
                        case 0:
                            if (prefab_Ramdam.PosLefte == 0)
                            {
                                MainSpriteRenderer.sprite = Default;
                                SpriteChangeTime = resetTime;
                                Destroy(obj_0);
                                Destroy(obj_1);
                                Destroy(obj_2);
                                Destroy(obj_3);
                                prefab_Ramdam.afterTheBox = 0;
                                phase = 11;
                                temp = 11;
                            }
                            break;
                        case 1:
                            if (prefab_Ramdam.PosCentre == 0)
                            {
                                MainSpriteRenderer.sprite = Default;
                                SpriteChangeTime = resetTime;
                                Destroy(obj_0);
                                Destroy(obj_1);
                                Destroy(obj_2);
                                Destroy(obj_3);
                                prefab_Ramdam.afterTheBox = 0;
                                phase = 12;
                                temp = 12;
                            }
                            break;
                        case 2:
                            if (prefab_Ramdam.PosRight == 0)
                            {
                                MainSpriteRenderer.sprite = Default;
                                SpriteChangeTime = resetTime;
                                Destroy(obj_0);
                                Destroy(obj_1);
                                Destroy(obj_2);
                                Destroy(obj_3);
                                prefab_Ramdam.afterTheBox = 0;
                                phase = 13;
                                temp = 13;
                            }
                            break;
                        
                    }
                    // if (prefab_Ramdam.PosLefte == 0)
                    // {
                    //     MainSpriteRenderer.sprite = Default;
                    //     SpriteChangeTime = resetTime;
                    //     Destroy(obj_0);
                    //     Destroy(obj_1);
                    //     Destroy(obj_2);
                    //     Destroy(obj_3);
                    //     prefab_Ramdam.afterTheBox = 0;
                    //     phase = 11;
                    //     temp = 11;
                    // }
                    // else if (prefab_Ramdam.PosCentre == 0)
                    // {
                    //     MainSpriteRenderer.sprite = Default;
                    //     SpriteChangeTime = resetTime;
                    //     Destroy(obj_0);
                    //     Destroy(obj_1);
                    //     Destroy(obj_2);
                    //     Destroy(obj_3);
                    //     prefab_Ramdam.afterTheBox = 0;
                    //     phase = 12;
                    //     temp = 12;
                    // }
                    // else if (prefab_Ramdam.PosRight == 0)
                    // {
                    //     MainSpriteRenderer.sprite = Default;
                    //     SpriteChangeTime = resetTime;
                    //     Destroy(obj_0);
                    //     Destroy(obj_1);
                    //     Destroy(obj_2);
                    //     Destroy(obj_3);
                    //     prefab_Ramdam.afterTheBox = 0;
                    //     phase = 13;
                    //     temp = 13;
                    // }
                }
                break;
        }
        Debug.Log ("ループ最後のrecv_の値：" + recv_);
        //recv_ = 0;
    }
}
