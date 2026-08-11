using System.Collections;
using Unity.ProjectAuditor.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CasterMechanic : MonoBehaviour
{
    public Spell[] spells;

    [SerializeField] bool isCastingSpell;
    [SerializeField] GameObject ballonTarget;

    CasterSpawner casterSpawn;

    float time;

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

        spell.OnSpellCast(gameObject);

        /*if(spell.reference == "Wind")
        {
            Debug.Log("Acivate Wind Spell");

            time = 0f;
            do
            {
                spell.Activate(gameObject, ballonTarget);

                time += Time.deltaTime;
                yield return null;
            }
            while (time < spell.activeDuration);
        }
        else
        {
            spell.Activate(gameObject, ballonTarget);

            yield return new WaitForSeconds(spell.activeDuration);
        }*/

        spell.Activate(gameObject, ballonTarget);

        yield return new WaitForSeconds(spell.activeDuration);

        spell.Deactivate(gameObject, ballonTarget);

        yield return new WaitForSeconds(1f);

        casterSpawn.isSpawning = false;
        Destroy(gameObject);
    }
}