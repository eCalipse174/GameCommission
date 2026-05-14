using System.Collections;
using UnityEngine;

public class GrowthManager
    : MonoBehaviour
{
    [SerializeField]
    private float adultDelay = 5f;

    private bool triggered;

    private void Update()
    {
        if (triggered)
            return;

        CharacterGrowth[] growths =
            FindObjectsByType
            <CharacterGrowth>(
                FindObjectsSortMode.None);

        foreach (var growth in growths)
        {
            if (!growth.IsGrowthComplete)
            {
                return;
            }
        }

        triggered = true;

        StartCoroutine(
            AdultRoutine(growths));
    }

    private IEnumerator AdultRoutine(
        CharacterGrowth[] growths)
    {
        Debug.Log(
            "All characters ready");

        yield return new WaitForSeconds(
            adultDelay);

        foreach (var growth in growths)
        {
            growth.GrowToAdult();
        }
    }
}