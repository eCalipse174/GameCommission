using UnityEngine;

public class PetDetector : MonoBehaviour
{
    private bool pointerInside;

    private Vector3 lastMousePos;

    private float petDistance;

    private const float PET_THRESHOLD = 2f;

    private void OnMouseEnter()
    {
        pointerInside = true;

        lastMousePos = Input.mousePosition;

        petDistance = 0f;
    }

    private void OnMouseExit()
    {
        pointerInside = false;

        petDistance = 0f;
    }

    private void Update()
    {
        if (!pointerInside)
            return;

        if (!Input.GetMouseButton(0))
            return;

        Vector3 currentPos =
            Input.mousePosition;

        petDistance +=
            Vector3.Distance(
                currentPos,
                lastMousePos);

        lastMousePos = currentPos;

        if (petDistance >= PET_THRESHOLD)
        {
            petDistance = 0f;

            OnPet();
        }
    }

    private void OnPet()
    {
        Debug.Log($"{name} PET");
    }
}