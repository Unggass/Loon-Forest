using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Teleport Spell")]
public class TeleportSpell : Spell
{
    public float minPosX = 0;
    public float maxPosX = 0;

    float randomX;

    public override void Activate(GameObject caster, GameObject target)
    {
        // Note : untuk Smentara,
        // bisa di Polish kaloudah Animationnya

        if (target != null)
        {
            target.SetActive(false);
        }
    }

    public override void Deactivate(GameObject caster, GameObject target)
    {
        // Note : untuk Smentara,
        // bisa di Polish kaloudah Animationnya

        if(target != null)
        {
            float direction = Random.Range(minPosX, maxPosX);
            float sign = Random.value < 0.5 ? -1 : 1;

            float spawnX = target.transform.position.x + direction * sign;

            target.transform.position = new Vector2(spawnX, target.transform.position.y);
            target.SetActive(true);
        }
    }

    public override void OnSpellCast(GameObject caster)
    {
        randomX = Random.Range(minPosX, maxPosX);
    }
}
