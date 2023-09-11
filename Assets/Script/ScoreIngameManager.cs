using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreIngameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;
    float alpha;
    float moveTime;
    float moveLeftTime;

    void Start()
    {
        alpha = 0f;
        GetComponent<TextMeshProUGUI>().color = new(1f, 1f, 1f, alpha);
    }

    void Update()
    {
        if (moveLeftTime >= 0f)
        {
            moveLeftTime -= Time.deltaTime;
            float t = moveLeftTime / moveTime;

            alpha = Mathf.Lerp(0f, 1f, t);
            GetComponent<TextMeshProUGUI>().color = new(1f, 1f, 1f, alpha);
        }
        else
        {
            Destroy(gameObject);
        }

        if (score < 1000)
        {
            scoreText.text = string.Format("{0:000}", score);
        }
        else if (score < 10000)
        {
            scoreText.text = string.Format("{0:0000}", score);
        }
        else if (score < 100000)
        {
            scoreText.text = string.Format("{0:00000}", score);
        }
        else if (score < 1000000)
        {
            scoreText.text = string.Format("{0:000000}", score);
        }
        else if (score < 10000000)
        {
            scoreText.text = string.Format("{0:0000000}", score);
        }
    }

    public void Initialized(int scoreValue, int size, float deathTime)
    {
        score = scoreValue;
        scoreText.fontSize = size;
        moveTime = deathTime;
        moveLeftTime = moveTime;
        //float randomAngle = Random.Range(-20f, 20f);
        //transform.Rotate(new(0f, 0f, 1f), randomAngle);
    }
}
