using UnityEngine;
using UnityEngine.UI;

public class LastScore : MonoBehaviour
{
    private int LastScoreNum = score.can_;   //最終的なスコアを入れる変数
    public Text Scoretext;  //スコアを表示するテキストUI
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(LastScoreNum <= 500)
        {
            Scoretext.text = "<size=150>" + LastScoreNum + "点" +  "</size>" + "\n<size=75>もう少しがんばろう！</size>";
        }
        else
        {
            Scoretext.text ="<size=150>" + LastScoreNum  + "点"+ "</size>" + "\n<size=75>よくできました！</size>";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
