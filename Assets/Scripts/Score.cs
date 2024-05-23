using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (GameManager.Instance != null)
        {
            int score = GameManager.Instance.Score;
            scoreText.text = "Score: " + score;
        }
        else
        {
            scoreText.text = "Score: 0";
        }
    }
}
