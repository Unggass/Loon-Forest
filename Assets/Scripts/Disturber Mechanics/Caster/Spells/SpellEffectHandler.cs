using NUnit.Framework.Internal.Commands;
using System.Collections;
using UnityEngine;

public class SpellEffectHandler : MonoBehaviour
{
    // for gravity spell
    private float originalGravity;
    private Coroutine gravityRoutine;

    public Ballon ballon;

    public void ApplyGravity(float force, float duration)
    {
        if(gravityRoutine != null) StopCoroutine(gravityRoutine);
        gravityRoutine = StartCoroutine(GravityActivation(force, duration));
    }

    IEnumerator GravityActivation(float force, float duration)
    {
        originalGravity = ballon.ballonGravityScale;

        ballon.ballonGravityScale = originalGravity * force;

        yield return new WaitForSecondsRealtime(duration);

        ballon.ballonGravityScale = originalGravity;

    }
}
