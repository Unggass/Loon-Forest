using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Wind Spell")]
public class WindSpell : Spell
{
    public float windForce = 5f;
    float sign;

    [SerializeField] LayerMask affectedLayer;
    //Vector3 origin = new Vector3(0, 0, 0);
    [SerializeField] float radius = 5f;

    bool isCasted = false;

    public override void OnSpellCast(GameObject caster)
    {
        sign = Random.value < 0.5f ? -1f : 1f;
    }

    public override void Activate(GameObject caster, GameObject target)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.transform.position, radius, affectedLayer);
        foreach (var hit in hits)
        {
            Debug.Log($"Hit: {hit.name} on layer {hit.gameObject.layer}");
            if (hit.TryGetComponent<IWindEffectable>(out var receiver))
            {
                isCasted = true;
                receiver.WindBlow(windForce, sign, isCasted);
            }
        }
    }

    public override void Deactivate(GameObject caster, GameObject target)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.transform.position, radius, affectedLayer);
        foreach (var hit in hits)
        {
            Debug.Log($"Hit: {hit.name} on layer {hit.gameObject.layer}");
            if (hit.TryGetComponent<IWindEffectable>(out var receiver))
            {
                isCasted = false;
                receiver.WindBlow(0f, sign, isCasted);
            }
        }
    }
}