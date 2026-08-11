using UnityEngine;

public class Fog : MonoBehaviour, IWindEffectable
{
    public Renderer fogRender;
    public Material fogMat;

    [Header("Local Time Scale")]
    [Tooltip("1 = Normal, 0 = Pause, Minus = Reverse")]
    public float localTimeScale = 1f;

    [Header("Settings")]
    public float acceleration = 2f;
    public float fogSpeedY = 0f;

    [Range(1, 30)] public float fogSpeedX = 0f;

    private float baseSpeedX = 0f;
    private bool isBlowed = false;

    private float currentSpeedX = 0f;
    private float currentSpeedY = 0f;
    private Vector2 accumulatedOffset = Vector2.zero;

    private static readonly int FogOffsetID = Shader.PropertyToID("_FogOffset");

    void Start()
    {
        if (fogRender == null) fogRender = GetComponent<Renderer>();
        fogMat = fogRender.material;
    }

    private void Update()
    {
        float targetSpeedX = isBlowed ? (baseSpeedX * localTimeScale) : 0f;
        currentSpeedX = Mathf.MoveTowards(currentSpeedX, targetSpeedX * fogSpeedX, acceleration * Time.unscaledDeltaTime);

        float targetSpeedY = isBlowed ? 0f : fogSpeedY;
        currentSpeedY = Mathf.MoveTowards(currentSpeedY, targetSpeedY, acceleration * Time.unscaledDeltaTime);

        float localDeltaTime = Time.deltaTime * localTimeScale;
        accumulatedOffset.x += currentSpeedX * localDeltaTime;
        accumulatedOffset.y += currentSpeedY * localDeltaTime;

        accumulatedOffset.x %= 1f;
        accumulatedOffset.y %= 1f;

        fogMat.SetVector(FogOffsetID, accumulatedOffset);
    }

    public void WindBlow(float strength, float direction, bool condition)
    {
        baseSpeedX = (Mathf.Clamp(strength, 1, strength) * direction) * 0.01f;
        isBlowed = condition;
    }
}