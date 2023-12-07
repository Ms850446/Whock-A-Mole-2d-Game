using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI scoreText;
    void Start()
    {
        showScore();
    }
    void showScore(){

        scoreText.text="Score: "+(GameManager.score).ToString("0");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
