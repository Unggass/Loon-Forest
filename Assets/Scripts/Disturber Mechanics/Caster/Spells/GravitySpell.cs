using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Gravity Spell")]
public class GravitySpell : Spell
{
    public float gravityStrength = 0.3f;

    public override void OnSpellCast(GameObject caster)
    {
        Debug.Log("Gatau ngisi apa");
    }

    public override void Activate(GameObject caster, GameObject target)
    {
        if(target != null)
        {
            Rigidbody2D targetGravityScale =  target.GetComponent<Rigidbody2D>();

            targetGravityScale.gravityScale += gravityStrength;
        }
    }

    public override void Deactivate(GameObject caster, GameObject target)
    {
        if (target != null)
        {
            Rigidbody2D targetGravityScale = target.GetComponent<Rigidbody2D>();

            targetGravityScale.gravityScale -= gravityStrength;
        }
    }
}
