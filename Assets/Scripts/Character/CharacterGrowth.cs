using System;
using System.Collections;
using UnityEngine;

public class CharacterGrowth : MonoBehaviour
{
    [Header("Growth")]
    [SerializeField]
    private GrowthStage currentStage = GrowthStage.Baby;
    [SerializeField]
    private int currentGrowthPoint;
    [SerializeField]
    private int maxGrowthPointToYoung = 1;
    [SerializeField]
    private int maxGrowthPointToAdult = 1;

    [Header("Baby Resources")]
    [SerializeField] private Sprite babySprite;
    [SerializeField] private float babyMoveSpeed = 1.5f;

    [Header("Young Resources")]
    [SerializeField] private Sprite youngSprite;
    [SerializeField] private float youngMoveSpeed = 2.0f;

    [Header("Adult Resources")]
    [SerializeField] private Sprite adultSprite;
    [SerializeField] private float adultMoveSpeed = 2.5f;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private CharacterMovement movement;
    private CharacterAnimator characterAnimator;

    public GrowthStage CurrentStage => currentStage;
    public bool IsAdult => currentStage == GrowthStage.Adult;

    public int MaxGrowthPoint =>
        currentStage == GrowthStage.Baby
            ? maxGrowthPointToYoung
            : maxGrowthPointToAdult;

    public bool IsGrowthComplete =>
        currentGrowthPoint >= MaxGrowthPoint;

    public int CurrentGrowthPoint => currentGrowthPoint;

    // 성장 단계가 바뀔 때마다 발생
    public event Action<GrowthStage> OnStageChanged;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    private void Start()
    {
        // 시작 시점 단계도 구독자에게 알려줌 (버튼 초기 상태 세팅용)
        OnStageChanged?.Invoke(currentStage);
    }

    public void LoadGrowthData(int growthPoint, GrowthStage stage)
    {
        currentGrowthPoint = growthPoint;
        currentStage = stage;
        ApplyStageResources(currentStage);
        OnStageChanged?.Invoke(currentStage);
    }

    public void AddGrowthPoint(int amount)
    {
        if (IsAdult) return;

        currentGrowthPoint += amount;
        currentGrowthPoint = Mathf.Clamp(
            currentGrowthPoint, 0, MaxGrowthPoint);

        Debug.Log(
            $"{name} Growth: {currentGrowthPoint}/{MaxGrowthPoint} ({currentStage})");
    }

    public void GrowToYoung()
    {
        if (currentStage != GrowthStage.Baby) return;

        currentStage = GrowthStage.Young;
        currentGrowthPoint = 0;
        ApplyStageResources(GrowthStage.Young);
        OnStageChanged?.Invoke(currentStage);
        Debug.Log($"{name} became young");
    }

    public void GrowToAdult()
    {
        if (currentStage != GrowthStage.Young) return;

        currentStage = GrowthStage.Adult;
        ApplyStageResources(GrowthStage.Adult);
        OnStageChanged?.Invoke(currentStage);
        Debug.Log($"{name} became adult");
    }

    private void ApplyStageResources(GrowthStage stage)
    {
        switch (stage)
        {
            case GrowthStage.Baby:
                if (babySprite != null)
                    spriteRenderer.sprite = babySprite;
                movement.SetMoveSpeed(babyMoveSpeed);
                break;

            case GrowthStage.Young:
                if (youngSprite != null)
                    spriteRenderer.sprite = youngSprite;
                movement.SetMoveSpeed(youngMoveSpeed);
                break;

            case GrowthStage.Adult:
                if (adultSprite != null)
                    spriteRenderer.sprite = adultSprite;
                movement.SetMoveSpeed(adultMoveSpeed);
                break;
        }

        characterAnimator.SetGrowthStage(stage);
    }

    public void ResetGrowth()
    {
        currentGrowthPoint = 0;
        currentStage = GrowthStage.Baby;
        ApplyStageResources(GrowthStage.Baby);
        OnStageChanged?.Invoke(currentStage);
    }

#if DEVELOPMENT_BUILD || UNITY_EDITOR
    [ContextMenu("Debug/Force Baby")]
    private void DebugForceBaby()
    {
        currentStage = GrowthStage.Baby;
        currentGrowthPoint = 0;
        ApplyStageResources(GrowthStage.Baby);
        OnStageChanged?.Invoke(currentStage);
        Debug.Log($"{name} [DEBUG] forced to Baby");
    }

    [ContextMenu("Debug/Force Young")]
    private void DebugForceYoung()
    {
        currentStage = GrowthStage.Young;
        currentGrowthPoint = 0;
        ApplyStageResources(GrowthStage.Young);
        OnStageChanged?.Invoke(currentStage);
        Debug.Log($"{name} [DEBUG] forced to Young");
    }

    [ContextMenu("Debug/Force Adult")]
    private void DebugForceAdult()
    {
        currentStage = GrowthStage.Adult;
        currentGrowthPoint = 0;
        ApplyStageResources(GrowthStage.Adult);
        OnStageChanged?.Invoke(currentStage);
        Debug.Log($"{name} [DEBUG] forced to Adult");
    }

    [ContextMenu("Debug/Max Growth Point")]
    private void DebugMaxGrowthPoint()
    {
        currentGrowthPoint = MaxGrowthPoint;
        Debug.Log($"{name} [DEBUG] growth point maxed: {currentGrowthPoint}/{MaxGrowthPoint}");
    }
#endif
}