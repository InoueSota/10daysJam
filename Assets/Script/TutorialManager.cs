using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // チュートリアルの種類
    private enum TutorialType
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

    // チュートリアル - 基本移動
    [SerializeField] private GameObject tutorialText1;
    [SerializeField] private GameObject tutorialText2;
    [SerializeField] private GameObject tutorialText3;
    [SerializeField] private GameObject tutorialText4;
    [SerializeField] private GameObject practiceMoves;
    [SerializeField] private GameObject practiceBubble;
    [SerializeField] private GameObject practiceSliderBack;
    [SerializeField] private GameObject practiceSliderFill;
    private bool isPossibleMove;
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
    private bool isPossibleOrder;
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
    private bool isPossibleAttack;
    private bool isClearAttack;

    // チュートリアル - 祈り
    [SerializeField] private GameObject tutorialText15;
    [SerializeField] private GameObject tutorialText16;

    [SerializeField]private bool isClearTutorial;

    private int inputDecide = 0;
    private int preInputDecide = 0;

    private float pushDecideTime = 0f;

    void Start()
    {
        moveSlider = practiceMoves.transform.Find("PracticeMoveSlider").GetComponent<Slider>();
        moveSlider.value = 0f;

        playerManager = playerObj.GetComponent<PlayerManager>();

        isPossibleMove = false;
        moveAddValue = 0f;
        isPossibleMove = false;
        isPossibleAttack = false;

        isClearAttack = false;
        isClearTutorial = false;

        inputDecide = 0;
        preInputDecide = 0;

        pushDecideTime = 0f;
    }

    void Update()
    {
        bool isPushDecide = GetInputDecide();

        if (!isClearTutorial)
        {
            switch (tutorialType)
            {
                case TutorialType.MOVE1:
                    TextUpdate(isPushDecide, tutorialText1, tutorialText2);
                    break;
                case TutorialType.MOVE2:
                    TextUpdate(isPushDecide, tutorialText2, tutorialText3);
                    break;
                case TutorialType.MOVE3:
                    TextUpdate(isPushDecide, tutorialText3, tutorialText4);
                    break;
                case TutorialType.MOVE4:
                    TextUpdate(isPushDecide, tutorialText4, practiceMoves);
                    if (isPushDecide && speechBubbleObj && tutorialText4 && tutorialText4.GetComponent<AlphaText>().GetIsFadeInClear())
                    {
                        speechBubbleObj.GetComponent<AlphaImage>().FadeOutInitialize();
                    }
                    break;
                // 基本移動の練習
                case TutorialType.PRACTICEMOVE:
                    isPossibleMove = true;
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
                    TextUpdate(isPushDecide, tutorialText5, tutorialText6);
                    break;
                case TutorialType.ORDER2:
                    TextUpdate(isPushDecide, tutorialText6, tutorialText7);
                    break;
                case TutorialType.ORDER3:
                    TextUpdate(isPushDecide, tutorialText7, tutorialText8);
                    break;
                case TutorialType.ORDER4:
                    TextUpdate(isPushDecide, tutorialText8, practiceOrder);
                    if (isPushDecide && speechBubbleObj && tutorialText8 && tutorialText8.GetComponent<AlphaText>().GetIsFadeInClear())
                    {
                        speechBubbleObj.GetComponent<AlphaImage>().FadeOutInitialize();
                    }
                    break;
                case TutorialType.PRACTICEORDER:
                    isPossibleOrder = true;
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
                                tutorialText9.SetActive(true);
                                speechBubbleObj.SetActive(true);
                                speechBubbleObj.GetComponent<AlphaImage>().Initialize();
                                tutorialType = TutorialType.ATTACK1;
                            }
                        }
                    }
                    break;
                case TutorialType.ATTACK1:
                    TextUpdate(isPushDecide, tutorialText9, tutorialText10);
                    break;
                case TutorialType.ATTACK2:
                    TextUpdate(isPushDecide, tutorialText10, tutorialText11);
                    break;
                case TutorialType.ATTACK3:
                    TextUpdate(isPushDecide, tutorialText11, tutorialText12);
                    break;
                case TutorialType.ATTACK4:
                    TextUpdate(isPushDecide, tutorialText12, tutorialText13);
                    break;
                case TutorialType.ATTACK5:
                    TextUpdate(isPushDecide, tutorialText13, tutorialText14);
                    break;
                case TutorialType.ATTACK6:
                    TextUpdate(isPushDecide, tutorialText14, practiceAttack);
                    if (isPushDecide && speechBubbleObj && tutorialText14 && tutorialText14.GetComponent<AlphaText>().GetIsFadeInClear())
                    {
                        speechBubbleObj.GetComponent<AlphaImage>().FadeOutInitialize();
                    }
                    break;
                case TutorialType.PRACTICEATTACK:
                    isPossibleAttack = true;
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
                    TextUpdate(isPushDecide, tutorialText15, tutorialText16);
                    break;
                case TutorialType.PRAY2:

                    if (isPushDecide && tutorialText16 && tutorialText16.activeSelf && tutorialText16.GetComponent<AlphaText>().GetIsFadeInClear() && speechBubbleObj)
                    {
                        tutorialText16.GetComponent<AlphaText>().FadeOutInitialize();
                        speechBubbleObj.GetComponent<AlphaImage>().FadeOutInitialize();
                    }
                    if (tutorialText16 && !tutorialText16.activeSelf)
                    {
                        isClearTutorial = true;
                    }
                    break;
            }
        }

        if (inputDecide != 0)
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

    private void TextUpdate(bool isPushDecide_, GameObject nowObj, GameObject nextObj)
    {
        if (isPushDecide_ && nowObj && nowObj.activeSelf && nowObj.GetComponent<AlphaText>().GetIsFadeInClear())
        {
            nowObj.GetComponent<AlphaText>().FadeOutInitialize();
        }

        else if (!nowObj.activeSelf && !nextObj.activeSelf)
        {
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

    private bool GetInputDecide()
    {
        preInputDecide = inputDecide;
        inputDecide = (int)Input.GetAxisRaw("Abutton");

        if (inputDecide != 0 && preInputDecide == 0)
        {
            return true;
        }
        return false;
    }

    public bool GetIsPossibleMove()
    {
        return isPossibleMove;
    }

    public void MoveAddValue(float addValue)
    {
        float deltaAddValue = addValue * Time.deltaTime;
        moveAddValue += deltaAddValue;
    }

    public bool GetIsPossibleOrder()
    {
        return isPossibleOrder;
    }

    public bool GetIsPossibleAttack()
    {
        return isPossibleAttack;
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
        isPossibleMove = true;
        isPossibleOrder = true;
        isPossibleAttack = true;
        isClearLeft = true;
        isClearRight = true;
        isClearUp = true;
        isClearDown = true;
        isClearAttack = true;
        isClearTutorial = true;
    }

}
