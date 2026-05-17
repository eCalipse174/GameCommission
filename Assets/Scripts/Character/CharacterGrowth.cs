using System.Collections;
using UnityEngine;

public class CharacterGrowth
    : MonoBehaviour
{
    [Header("Growth")]

    [SerializeField]
    private GrowthStage currentStage =
        GrowthStage.Baby;

    [SerializeField]
    private int currentGrowthPoint;

    [SerializeField]
    private int maxGrowthPoint = 1;

    [SerializeField]
    private float adultMoveSpeed = 2.5f;

    [Header("Adult Resources")]

    [SerializeField]
    private Sprite adultSprite;

    [SerializeField]
    private RuntimeAnimatorController
        adultAnimator;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    private CharacterMovement movement;

    public GrowthStage CurrentStage =>
        currentStage;

    public bool IsAdult =>
        currentStage ==
        GrowthStage.Adult;

    public bool IsGrowthComplete =>
        currentGrowthPoint >=
        maxGrowthPoint;

    public int CurrentGrowthPoint =>
        currentGrowthPoint;

    private CharacterAnimator characterAnimator;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        characterAnimator = GetComponent<CharacterAnimator>(); // 추가
    }

    public void LoadGrowthData(
    int growthPoint,
    GrowthStage stage)
    {
        currentGrowthPoint =
            growthPoint;

        currentStage =
            stage;

        if (currentStage ==
            GrowthStage.Adult)
        {
            ApplyAdultResources();
        }
    }

    public void AddGrowthPoint(
        int amount)
    {
        if (IsAdult)
            return;

        currentGrowthPoint += amount;

        currentGrowthPoint =
            Mathf.Clamp(
                currentGrowthPoint,
                0,
                maxGrowthPoint);

        Debug.Log(
            $"{name} Growth: " +
            $"{currentGrowthPoint}/" +
            $"{maxGrowthPoint}");
    }

    public void GrowToAdult()
    {
        if (IsAdult)
            return;

        currentStage =
            GrowthStage.Adult;

        ApplyAdultResources();

        Debug.Log(
            $"{name} became adult");
    }

    private void ApplyAdultResources()
    {
        if (adultSprite != null)
            spriteRenderer.sprite = adultSprite;

        if (adultAnimator != null)
            characterAnimator.ApplyController(adultAnimator); // 이걸로 교체

        movement.SetMoveSpeed(adultMoveSpeed);
    }
}