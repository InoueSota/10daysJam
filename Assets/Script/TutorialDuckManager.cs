using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDuckManager : MonoBehaviour
{
    // 移動開始フラグ
    private bool isStartMove;
    // 移動完了フラグ - Enter
    private bool isEnterComplete;
    // 移動完了フラグ - Exit
    private bool isExitComplete;

    // イージングで移動させる
    private float moveTime;
    private float moveLeftTime;
    private Vector3 moveStartPosition;
    private Vector3 moveEndPosition;

    private RectTransform rectTransform;
    private Animator animator;

    void Start()
    {
        isStartMove = true;
        isEnterComplete = false;
        isExitComplete = false;

        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();

        moveTime = 1f;
        moveLeftTime = moveTime;
        moveStartPosition = rectTransform.localPosition;
        moveEndPosition = new(550, 140, rectTransform.localPosition.z);
    }

    void Update()
    {
        if (isStartMove && !isEnterComplete)
        {
            EasingMove();
            if (moveLeftTime <= 0f) 
            {
                moveEndPosition = moveStartPosition;
                moveStartPosition = rectTransform.localPosition;
                moveLeftTime = moveTime;
                isStartMove = false;
                isEnterComplete = true;
            }
        }
        else if (isStartMove && !isExitComplete)
        {
            rectTransform.localScale = Vector3.one;
            EasingMove();
            if (moveLeftTime <= 0f) { isExitComplete = true; }
        }
    }

    private void EasingMove()
    {
        moveLeftTime -= Time.deltaTime;
        if (moveLeftTime < 0f) { moveLeftTime = 0f; }

        float t = moveLeftTime / moveTime;
        rectTransform.localPosition = Vector3.Lerp(moveEndPosition, moveStartPosition, t * t);
    }

    public bool GetIsEnterComplete()
    {
        return isEnterComplete;
    }

    public void SetDuckMoveStart()
    {
        isStartMove = true;
    }
}
