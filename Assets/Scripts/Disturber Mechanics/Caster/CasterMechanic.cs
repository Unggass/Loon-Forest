using System.Collections;
using Unity.ProjectAuditor.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class CasterMechanic : MonoBehaviour
{
    public Spell[] spells;

    [SerializeField] bool isCastingSpell;
    [SerializeField] GameObject ballonTarget;

    CasterSpawner casterSpawn;

    float time = 0;

    void Update()
    {
        if (isCastingSpell)
        {
            isCastingSpell = false;
            StartCoroutine(CastSpell());
        }
    }

    void OnEnable()
    {
        isCastingSpell = true;

        casterSpawn = FindAnyObjectByType<CasterSpawner>();
    }

    IEnumerator CastSpell()
    {
        yield return new WaitForSeconds(1f);

        ballonTarget = GameObject.FindGameObjectWithTag("Ballon");

        int index = Random.Range(0, spells.Length);
        Spell spell = spells[index];

        spell.OnSpellCast();

        time = 0;

        do
        {
            spell.Activate(gameObject, ballonTarget);
            time += Time.deltaTime;
            yield return null;
        }
        while (time < spell.activeDuration);

        spell.Deactivate(gameObject, ballonTarget);

        yield return new WaitForSeconds(1f);

        casterSpawn.isSpawning = false;
        Destroy(gameObject);
    }
}