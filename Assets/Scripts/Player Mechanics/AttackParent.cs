using UnityEngine;

public class AttackParent : MonoBehaviour
{
    public Vector2 pointerPos;
    
    public bool rotationLocked = false;

    [Space (10)]
    [Header("Rotation Clamp")]
    [SerializeField] float minAngle = -45f;
    [SerializeField] float maxangle = 45f;

    // Update is called once per frame
    void Update()
    {
        if (rotationLocked) { return; }
        Vector2 dir = (pointerPos - (Vector2)transform.position).normalized;

        float angle = Vector2.SignedAngle(Vector2.up, dir);
        angle = Mathf.Clamp(angle, minAngle, maxangle);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
