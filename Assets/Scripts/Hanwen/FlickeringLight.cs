using UnityEngine;
using System.Collections;

/// <summary>
/// Simulates a flickering effect for Point Light sources.
/// Attach this script to a GameObject with a Light component.
/// </summary>
public class FlickeringLight : MonoBehaviour
{
    [Tooltip("The Point Light to control. If left empty, the script will try to get the Light component on the same GameObject.")]
    public Light pointLight;

    [Header("Intensity Range Settings")]
    [Tooltip("Minimum light intensity.")]
    public float minIntensity = 3f;
    [Tooltip("Maximum light intensity.")]
    public float maxIntensity = 5f;

    [Header("Range Settings (Optional)")]
    [Tooltip("Whether to also affect the light's range.")]
    public bool affectRange = true;
    [Tooltip("Minimum light range.")]
    public float minRange = 10f;
    [Tooltip("Maximum light range.")]
    public float maxRange = 10f;

    [Header("Flicker Speed")]
    [Tooltip("Smoothing speed for intensity changes. Higher values = faster changes.")]
    public float intensityChangeSpeed = 5f;
    [Tooltip("Smoothing speed for range changes (if affectRange is true). Higher values = faster changes.")]
    public float rangeChangeSpeed = 5f;

    [Tooltip("Minimum interval (seconds) between new target intensity/range values.")]
    public float minFlickerInterval = 0.05f;
    [Tooltip("Maximum interval (seconds) between new target intensity/range values.")]
    public float maxFlickerInterval = 0.2f;

    private float targetIntensity; // Current target intensity for flicker
    private float targetRange;     // Current target range for flicker
    private Coroutine flickerCoroutine; // Coroutine for updating target values

    [SerializeField] bool isNPC;
    [SerializeField] MiniGameResult miniGameResult;

    void Awake()
    {
        pointLight = GetComponent<Light>();
    }

    void OnEnable()
    {
        // When enabled (including at game start), start the flicker coroutine if possible
        if (pointLight != null && pointLight.type == LightType.Point)
        {
            // Initialize target values to current light settings for smooth start
            targetIntensity = pointLight.intensity;
            if (affectRange)
            {
                targetRange = pointLight.range;
            }

            // Stop any previous coroutine, then start a new one
            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
            }
            flickerCoroutine = StartCoroutine(UpdateFlickerTargetsRoutine());
        }
    }

    void OnDisable()
    {
        // Stop coroutine when GameObject is disabled
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
    }

    /// <summary>
    /// Coroutine: repeatedly sets new target intensity and range values for flicker.
    /// </summary>
    IEnumerator UpdateFlickerTargetsRoutine()
    {
        // Loop until coroutine is stopped
        while (true)
        {
            // Pick a new random target intensity within min/max
            targetIntensity = Random.Range(minIntensity, maxIntensity);

            // If affecting range, pick a new random target range within min/max
            if (affectRange)
            {
                targetRange = Random.Range(minRange, maxRange);
            }

            // Wait a random interval before picking new targets
            // This makes the flicker less predictable and more natural
            yield return new WaitForSeconds(Random.Range(minFlickerInterval, maxFlickerInterval));
        }
    }

    void Update()
    {
        if (!isNPC)
        {
            maxIntensity = LevelManager.maxLight;
            minIntensity = LevelManager.minLight;
        }
        else
        {
            maxIntensity = miniGameResult.maxlight;
            minIntensity = miniGameResult.minlight;
        }



        // Do nothing if script is disabled or no valid light
        if (!enabled || pointLight == null)
        {
            return;
        }

        // Smoothly interpolate current intensity toward target
        // Time.deltaTime ensures frame-rate independence
        pointLight.intensity = Mathf.Lerp(pointLight.intensity, targetIntensity, Time.deltaTime * intensityChangeSpeed);

        // If affecting range, smoothly interpolate current range toward target
        if (affectRange)
        {
            pointLight.range = Mathf.Lerp(pointLight.range, targetRange, Time.deltaTime * rangeChangeSpeed);
        }
    }
}
