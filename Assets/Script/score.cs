using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    // スコアを表示するテキストUI
    public Text Scoretext;
    // 現在のスコア
    public  int can;
    //最終的なスコアを記録する
    public static int can_; 

    // Startは初期化処理を行う。ゲーム開始時に一度だけ呼ばれる
    void Start()
    {
        // 現在のスコアを0に初期化
        can = 0;
        // スコアを表示
        Scoretext.text = "スコア:" + can;
    }

    // Updateはフレームごとに呼ばれる
    void Update()
    {
        //逆にidou.csでcan変数を変更する

        can_ = can;
        // 更新されたスコアを表示
        Scoretext.text = "スコア:" + can;
    }
}