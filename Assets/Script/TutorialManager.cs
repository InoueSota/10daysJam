using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // チュートリアルの種類
    public enum TutorialType
    {
        MOVE1,
        MOVE2,
        MOVE3,
        MOVE4,
        PRACTICEMOVE,
        ORDER1,
        ORDER2,
        ORDER3,
        ORDER4,
        PRACTICEORDER,
        ATTACK1,
        ATTACK2,
        ATTACK3,
        ATTACK4,
        ATTACK5,
        ATTACK6,
        PRACTICEATTACK,
        PRAY1,
        PRAY2
    }
    private TutorialType tutorialType = TutorialType.MOVE1;

    // プレイヤー
    [SerializeField] private GameObject playerObj;
    private PlayerManager playerManager;
    // チュートリアルガモ
    [SerializeField] private GameObject tutorialDuckObj;

    // 吹き出し
    [SerializeField] private GameObject speechBubbleObj;

    // テキストを自動更新させる
    [SerializeField] private float textActiveTime;
    private float textActiveLeftTime;

    // チュートリアル - 基本移動
    [SerializeField] private GameObject tutorialText1;
    [SerializeField] private GameObject tutorialText2;
    [SerializeField] private GameObject tutorialText3;
    [SerializeField] private GameObject tutorialText4;
    [SerializeField] private GameObject practiceMoves;
    [SerializeField] private GameObject practiceBubble;
    [SerializeField] private GameObject practiceSliderBack;
    [SerializeField] private GameObject practiceSliderFill;
    [SerializeField] private GameObject obstacleObj;
    [SerializeField] private GameObject grassObj;
    private float moveAddValue;
    // スライダー
    private Slider moveSlider;

    // チュートリアル - 指示
    [SerializeField] private GameObject tutorialText5;
    [SerializeField] private GameObject tutorialText6;
    [SerializeField] private GameObject tutorialText7;
    [SerializeField] private GameObject tutorialText8;
    [SerializeField] private GameObject practiceOrder;
    [SerializeField] private GameObject practiceOrderLeft;
    [SerializeField] private GameObject practiceOrderRight;
    [SerializeField] private GameObject practiceOrderUp;
    [SerializeField] private GameObject practiceOrderDown;
    private bool isClearLeft;
    private bool isClearRight;
    private bool isClearUp;
    private bool isClearDown;

    // チュートリアル - 攻撃
    [SerializeField] private GameObject tutorialText9;
    [SerializeField] private GameObject tutorialText10;
    [SerializeField] private GameObject tutorialText11;
    [SerializeField] private GameObject tutorialText12;
    [SerializeField] private GameObject tutorialText13;
    [SerializeField] private GameObject tutorialText14;
    [SerializeField] private GameObject practiceAttack;
    [SerializeField] private GameObject practiceAttackClear;
    [SerializeField] private GameObject dummyObj;
    private bool isClearAttack;

    // チュートリアル - 祈り
    [SerializeField] private GameObject tutorialText15;
    [SerializeField] private GameObject tutorialText16;

    // チュートリアル完了フラグ
    private bool isClearTutorial;

    private float pushDecideTime = 0f;

    void Start()
    {
        textActiveLeftTime = textActiveTime;

        moveSlider = practiceMoves.transform.Find("PracticeMoveSlider").GetComponent<Slider>();
        moveSlider.value = 0f;

        playerManager = playerObj.GetComponent<PlayerManager>();

        moveAddValue = 0f;

        isClearAttack = false;
        isClearTutorial = false;

        pushDecideTime = 0f;
    }

    void Update()
    {
        if (!isClearTutorial)
        {
            switch (tutorialType)
            {
                case TutorialType.MOVE1:
                    if (tutorialDuckObj && tutorialDuckObj.GetComponent<TutorialDuckManager>().GetIsEnterComplete())
                    {
                        if (speechBubbleObj && !speechBubbleObj.activeSelf && tutorialText1 && !tutorialText1.activeSelf)
                        {
                            speechBubbleObj.SetActive(true);
                            tutorialText1.SetActive(true);
                        }
                        TextUpdate(tutorialText1, tutorialText2);
                    }
                    break;
                case TutorialType.MOVE2:
                    TextUpdate(tutorialText2, tutorialText3);
                    break;
                case TutorialType.MOVE3:
                    TextUpdate(tutorialText3, tutorialText4);
                    break;
                case TutorialType.MOVE4:
                    TextUpdate(tutorialText4, practiceMoves);
                    if (speechBubbleObj && tutorialText4 && tutorialText4.GetComponent<AlphaText>().GetIsFadeInClear())
                    {
                        if (textActiveLeftTime <= 0f) { speechBubbleObj.GetComponent<AlphaImage>().FadeOutInitialize(); }
                    }
                    break;
                // 基本移動の練習
                case TutorialType.PRACTICEMOVE:
                    if (moveSlider)
                    {
                        moveSlider.value = moveAddValue / 100.0f;
                        if (moveSlider.value >= 1f)
                        {
                            FadeOutImageInitialize(practiceBubble);
                            FadeOutImageInitialize(practiceSliderBack);
                            FadeOutImageInitialize(practiceSliderFill);
                            if (practiceBubble && !practiceBubble.activeSelf)
                            {
                                if (practiceMoves && tutorialText5 && speechBubbleObj)
                                {
                                    practiceMoves.SetActive(false);
                                    tutorialText5.SetActive(true);
                                    speechBubbleObj.SetActive(true);
                                    speechBubbleObj.GetComponent<AlphaImage>().Initialize();
                                    tutorialType = TutorialType.ORDER1;
                                }
                            }
                        }
                    }
                    break;
                case TutorialType.ORDER1:
                    TextUpdate(tutorialText5, tutorialText6);
                    break;
                case TutorialType.ORDER2:
                    TextUpdate(tutorialText6, tutorialText7);
                    break;
                case TutorialType.ORDER3:
                    TextUpdate(tutorialText7, tutorialText8);
                    break;
                case TutorialType.ORDER4:
                    TextUpdate(tutorialText8, practiceOrder);
                    if (speechBubbleObj && tutorialText8 && tutorialText8.GetComponent<AlphaText>().GetIsFadeInClear())
                    {
                        if (textActiveLeftTime <= 0f) { speechBubbleObj.GetComponent<AlphaImage>().FadeOutInitialize(); }
                    }
                    break;
                case TutorialType.PRACTICEORDER:
                    CheckPracticeOrder();
                    if (isClearLeft && isClearRight && isClearUp && isClearDown)
                    {
                        FadeOutImageInitialize(practiceOrder);
                        FadeOutImageInitialize(practiceOrderLeft);
                        FadeOutImageInitialize(practiceOrderRight);
                        FadeOutImageInitialize(practiceOrderUp);
                        FadeOutImageInitialize(practiceOrderDown);
                        if (practiceOrder && !practiceOrder.activeSelf)
                        {
                            if (tutorialText9 && speechBubbleObj)
                            {
                                obstacleObj.SetActive(true);
                                grassObj.SetActive(true);
                                tutorialText9.SetActive(true);
                                speechBubbleObj.SetActive(true);
                                speechBubbleObj.GetComponent<AlphaImage>().Initialize();
                                tutorialType = TutorialType.ATTACK1;
                            }
                        }
                    }
                    break;
                case TutorialType.ATTACK1:
                    TextUpdate(tutorialText9, tutorialText10);
                    break;
                case TutorialType.ATTACK2:
                    TextUpdate(tutorialText10, tutorialText11);
                    break;
                case TutorialType.ATTACK3:
                    TextUpdate(tutorialText11, tutorialText12);
                    break;
                case TutorialType.ATTACK4:
                    TextUpdate(tutorialText12, tutorialText13);
                    break;
                case TutorialType.ATTACK5:
                    TextUpdate(tutorialText13, tutorialText14);
                    break;
                case TutorialType.ATTACK6:
                    TextUpdate(tutorialText14, practiceAttack);
                    if (speechBubbleObj && tutorialText14 && tutorialText14.GetComponent<AlphaText>().GetIsFadeInClear())
                    {
                        dummyObj.SetActive(true);
                        if (textActiveLeftTime <= 0f) { speechBubbleObj.GetComponent<AlphaImage>().FadeOutInitialize(); }
                    }
                    break;
                case TutorialType.PRACTICEATTACK:
                    if (isClearAttack)
                    {
                        if (practiceAttackClear)
                        {
                            practiceAttackClear.SetActive(true);
                        }
                        FadeOutImageInitialize(practiceAttack);
                        FadeOutImageInitialize(practiceAttackClear);
                        if (practiceAttack && !practiceAttack.activeSelf)
                        {
                            if (tutorialText15 && speechBubbleObj)
                            {
                                tutorialText15.SetActive(true);
                                speechBubbleObj.SetActive(true);
                                speechBubbleObj.GetComponent<AlphaImage>().Initialize();
                                tutorialType = TutorialType.PRAY1;
                            }
                        }
                    }
                    break;
                case TutorialType.PRAY1:
                    TextUpdate(tutorialText15, tutorialText16);
                    break;
                case TutorialType.PRAY2:

                    if (tutorialText16 && tutorialText16.activeSelf && tutorialText16.GetComponent<AlphaText>().GetIsFadeInClear() && speechBubbleObj)
                    {
                        textActiveLeftTime -= Time.deltaTime;
                        if (textActiveLeftTime <= 0f)
                        {
                            tutorialText16.GetComponent<AlphaText>().FadeOutInitialize();
                            speechBubbleObj.GetComponent<AlphaImage>().FadeOutInitialize();
                            tutorialDuckObj.GetComponent<TutorialDuckManager>().SetDuckMoveStart();
                        }
                    }
                    if (tutorialText16 && !tutorialText16.activeSelf)
                    {
                        isClearTutorial = true;
                    }
                    break;
            }
        }

        if ((int)Input.GetAxisRaw("Abutton") != 0)
        {
            pushDecideTime += Time.deltaTime;
            if (pushDecideTime >= 3f)
            {
                switch (tutorialType)
                {
                    case TutorialType.MOVE1:
                        Destroy(tutorialText1);
                        break;
                    case TutorialType.MOVE2:
                        Destroy(tutorialText2);
                        break;
                    case TutorialType.MOVE3:
                        Destroy(tutorialText3);
                        break;
                    case TutorialType.MOVE4:
                        Destroy(tutorialText4);
                        break;
                    case TutorialType.PRACTICEMOVE:
                        Destroy(practiceBubble);
                        Destroy(practiceSliderBack);
                        Destroy(practiceSliderFill);
                        break;
                    case TutorialType.ORDER1:
                        Destroy(tutorialText5);
                        break;
                    case TutorialType.ORDER2:
                        Destroy(tutorialText6);
                        break;
                    case TutorialType.ORDER3:
                        Destroy(tutorialText7);
                        break;
                    case TutorialType.ORDER4:
                        Destroy(tutorialText8);
                        break;
                    case TutorialType.PRACTICEORDER:
                        Destroy(practiceOrder);
                        Destroy(practiceOrderLeft);
                        Destroy(practiceOrderRight);
                        Destroy(practiceOrderUp);
                        Destroy(practiceOrderDown);
                        break;
                    case TutorialType.ATTACK1:
                        Destroy(tutorialText9);
                        break;
                    case TutorialType.ATTACK2:
                        Destroy(tutorialText10);
                        break;
                    case TutorialType.ATTACK3:
                        Destroy(tutorialText11);
                        break;
                    case TutorialType.ATTACK4:
                        Destroy(tutorialText12);
                        break;
                    case TutorialType.ATTACK5:
                        Destroy(tutorialText13);
                        break;
                    case TutorialType.ATTACK6:
                        Destroy(tutorialText14);
                        break;
                    case TutorialType.PRACTICEATTACK:
                        Destroy(practiceAttack);
                        Destroy(practiceAttackClear);
                        break;
                    case TutorialType.PRAY1:
                        Destroy(tutorialText15);
                        break;
                    case TutorialType.PRAY2:
                        Destroy(tutorialText16);
                        break;
                }
                Destroy(speechBubbleObj);
                Destroy(tutorialDuckObj);
                Destroy(dummyObj);
                FlagAllClear();
            }
        }
        else
        {
            pushDecideTime = 0;
        }
    }

    private void TextUpdate(GameObject nowObj, GameObject nextObj)
    {
        if (nowObj && nowObj.activeSelf && nowObj.GetComponent<AlphaText>().GetIsFadeInClear())
        {
            textActiveLeftTime -= Time.deltaTime;
            if (textActiveLeftTime <= 0f) { nowObj.GetComponent<AlphaText>().FadeOutInitialize(); }
        }

        else if (!nowObj.activeSelf && !nextObj.activeSelf)
        {
            textActiveLeftTime = textActiveTime;
            nextObj.SetActive(true);
            tutorialType++;
        }
    }

    private void FadeOutImageInitialize(GameObject fadeOutObj)
    {
        if (fadeOutObj)
        {
            fadeOutObj.GetComponent<AlphaImage>().FadeOutInitialize();
        }
    }

    public void MoveAddValue(float addValue)
    {
        float deltaAddValue = addValue * Time.deltaTime;
        moveAddValue += deltaAddValue;
    }

    public void CheckPracticeOrder()
    {
        if (playerManager && playerManager.orderLeft)
        {
            if (practiceOrderLeft && !practiceOrderLeft.activeSelf)
            {
                practiceOrderLeft.SetActive(true);
            }
            isClearLeft = true;
        }
        if (playerManager && playerManager.orderRight)
        {
            if (practiceOrderRight && !practiceOrderRight.activeSelf)
            {
                practiceOrderRight.SetActive(true);
            }
            isClearRight = true;
        }
        if (playerManager && playerManager.orderStack)
        {
            if (practiceOrderUp && !practiceOrderUp.activeSelf)
            {
                practiceOrderUp.SetActive(true);
            }
            isClearUp = true;
        }
        if (playerManager && playerManager.orderDown)
        {
            if (practiceOrderDown && !practiceOrderDown.activeSelf)
            {
                practiceOrderDown.SetActive(true);
            }
            isClearDown = true;
        }
    }

    public void SetClearAttack()
    {
        isClearAttack = true;
    }

    public bool GetIsClearTutorial()
    {
        return isClearTutorial;
    }

    private void FlagAllClear()
    {
        isClearLeft = true;
        isClearRight = true;
        isClearUp = true;
        isClearDown = true;
        isClearAttack = true;
        isClearTutorial = true;
    }

    public TutorialType GetTutorialType()
    {
        return tutorialType;
    }
}
