using UnityEngine;

public interface IEffectable
{
    void GravityEffect(float gravityScale);
    void WindEffect(Vector2 WindDirection, float windForce);
}
