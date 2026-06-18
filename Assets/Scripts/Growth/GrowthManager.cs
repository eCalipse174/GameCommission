using System.Collections;
using UnityEngine;

public class GrowthManager : MonoBehaviour
{
    [SerializeField] private float youngDelay = 3f;
    [SerializeField] private float adultDelay = 5f;

    private bool triggered;

    private void Update()
    {
        if (triggered) return;

        CharacterGrowth[] growths =
            FindObjectsByType<CharacterGrowth>(FindObjectsSortMode.None);

        foreach (var growth in growths)
        {
            if (!growth.IsGrowthComplete) return;
        }

        triggered = true;
        StartCoroutine(GrowthRoutine(growths));
    }

    private IEnumerator GrowthRoutine(CharacterGrowth[] growths)
    {
        // Baby -> Young
        if (IsAllInStage(growths, GrowthStage.Baby))
        {
            Debug.Log("All characters ready to grow: Young");
            yield return new WaitForSeconds(youngDelay);

            foreach (var growth in growths)
                growth.GrowToYoung();

            // Young 단계 성장 완료 대기
            yield return new WaitUntil(
                () => AllGrowthComplete(growths));
        }

        // Young -> Adult
        if (IsAllInStage(growths, GrowthStage.Young))
        {
            Debug.Log("All characters ready to grow: Adult");
            yield return new WaitForSeconds(adultDelay);

            foreach (var growth in growths)
                growth.GrowToAdult();
        }

        triggered = false; // 다음 전환을 위해 초기화
    }

    private bool AllGrowthComplete(CharacterGrowth[] growths)
    {
        foreach (var growth in growths)
        {
            if (!growth.IsGrowthComplete) return false;
        }
        return true;
    }

    private bool IsAllInStage(
        CharacterGrowth[] growths,
        GrowthStage stage)
    {
        foreach (var growth in growths)
        {
            if (growth.CurrentStage != stage) return false;
        }
        return true;
    }
}