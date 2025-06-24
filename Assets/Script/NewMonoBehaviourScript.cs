using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int TimeBonus_timeInterval;
    public float fast_speed;  // 賽銭箱の前まで行く時のスピード
    public float FastPositionSpeed_y;  // 左のポジションまで移動する時のスピード
    public float SecondPositionSpeed_x, SecondPositionSpeed_y;  // 真ん中のポジションまで移動する時のスピード
    public float ThirdPositionSpeed_x, ThirdPositionSpeed_y;  // 右のポジションまで移動する時のスピード
    private GameObject[] character;
    idou Idou;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 

        
        // for(int i = 0; i <= character.Length; i++)
        // {
            
        // }
    }

    // Update is called once per frame
    void Update()
    {
        character = GameObject.FindGameObjectsWithTag("character");
        foreach(GameObject obj in character)
        {
            Idou = obj.GetComponent<idou>();
        }
        Idou.TimeBonus_timeInterval = TimeBonus_timeInterval;
        Idou.fast_speed = fast_speed;
        Idou.FastPositionSpeed_y = FastPositionSpeed_y;
        Idou.SecondPositionSpeed_x = SecondPositionSpeed_x;
        Idou.SecondPositionSpeed_y = SecondPositionSpeed_y;
        Idou.ThirdPositionSpeed_x = ThirdPositionSpeed_x;
        Idou.ThirdPositionSpeed_y = ThirdPositionSpeed_y;
    }
}