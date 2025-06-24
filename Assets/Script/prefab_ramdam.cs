using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefab_ramdam : MonoBehaviour
{
    public GameObject[] prefab_hito; // 生成するプレハブの配列
    public GameObject[] prefab_item;
    public GameObject[] prefab_item_hantoumei;
    public GameObject hukidasi;
    public GameObject hukidasi_hantoumei;
    public GameObject TextHyaku;
    public GameObject TextNihyaku;
    public GameObject TextSanhyaku;
    //private float time; // タイマー用の変数
    //public float resetTime;
    public int number_hito; // ランダムに選ばれたプレハブのインデックス
    public int number_item; // ランダムに選ばれたプレハブのインデックス

    public int startingLayer = 31; // 最初のレイヤーの値
    private int currentLayer; // 現在のレイヤーの値

    //おみくじ売り場前の三箇所に人がいるかいないかを判定する変数
    public int afterTheBox = 0;
    public int PosLefte = 0;
    public int PosCentre = 0;
    public int PosRight = 0;

    void Start()
    {
        //Application.targetFrameRate = 120;
        // time = 1.0f; // Startが呼ばれた時、タイマーを1秒に設定
        currentLayer = startingLayer; // 初期のレイヤー値を設定する
    }

    void Update()
    {
        //time -= Time.deltaTime; // タイマーを減少させる
        if (/*time <= 0.0f*/afterTheBox == 0) // タイマーが0以下になったら
        {
            //time = resetTime; // タイマーをリセット
            number_hito = Random.Range(0, prefab_hito.Length); // プレハブ配列からランダムにインデックスを選ぶ
            number_item = Random.Range(0, prefab_item.Length); // プレハブ配列からランダムにインデックスを選ぶ

            GameObject prefabInstance = Instantiate(prefab_hito[number_hito], new Vector3(0, 0, 0), Quaternion.identity); // 選ばれたプレハブを生成
            // Instantiate(prefab_item[number_item], new Vector3(-2.8f, -0.5f, 0), Quaternion.identity); //吹き出しの中身の生成
            
            afterTheBox = 1;
            // レイヤーを設定する
            // Unity2Dにあるコンポーネント"SpriteRendere"を使用して，生成したオブジェクトのレイヤーを取得
            // https://docs.unity3d.com/ja/2019.4/Manual/class-SpriteRenderer.html（公式ドキュメント）
            // https://wizardia.hateblo.jp/entry/2023/04/29/100000（参考サイト）
            SpriteRenderer spriteRenderer = prefabInstance.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = currentLayer;
            }

            currentLayer--; // レイヤーを減少させる

            // レイヤー名を取得してログに出力する
            int layer = prefabInstance.layer;
            string layerName = LayerMask.LayerToName(layer);
            //Debug.Log("Prefabのレイヤー名: " + layerName);
        }
    }
}
