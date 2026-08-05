using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Wind Spell")]
public class WindSpell : Spell
{
    public float windForce = 5f;
    float sign;

    public override void OnSpellCast()
    {
        sign = Random.value < 0.5f ? -1f : 1f;
    }

    public override void Activate(GameObject caster, GameObject target)
    {
        if (target != null)
        {
            Rigidbody2D targetVelocity = target.GetComponent<Rigidbody2D>();

            targetVelocity.linearVelocity += new Vector2(windForce * sign, 0f);
        }
    }

    public override void Deactivate(GameObject caster, GameObject target)
    {
        if (target != null)
        {
            Rigidbody2D targetVelocity = target.GetComponent<Rigidbody2D>();

            targetVelocity.linearVelocity -= new Vector2(windForce * sign, 0f);
        }
    }
}
