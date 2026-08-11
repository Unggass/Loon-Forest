using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string spellName;
    public string reference;
    public float activeDuration;

    public abstract void OnSpellCast(GameObject caster);
    public abstract void Activate(GameObject caster, GameObject target);
    public abstract void Deactivate(GameObject caster, GameObject target);
}
