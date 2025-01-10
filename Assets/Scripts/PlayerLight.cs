using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private float matchLightingDuration = 9;
    
    private const float DefaultIntensity = .6f;
    private const float DefaultPointLightInnerRadius = 0.2f;
    private const float DefaultPointLightOuterRadius = 1.52f;
    private readonly Color defaultLightColor = new Color(224f / 255f, 191f / 255f, 164f / 255f);

    private const float MatchIntensity = .7f;
    private const float MatchPointLightInnerRadius = 0.2f;
    private const float MatchPointLightOuterRadius = 3.53f;
    private readonly Color matchLightColor = new Color(185f / 255f, 84f / 255f, 0f / 255f);

    private Light2D _spotlight;
    private CharacterSound _playerSound;

    private void Start()
    {
        _playerSound = GetComponent<CharacterSound>();
        _spotlight = GetComponentInChildren<Light2D>();
        SetLightToDefault();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Match"))
        {
            Destroy(other.gameObject);
            StartCoroutine(TurnOnMatchLight());
        }
    }

    private IEnumerator TurnOnMatchLight()
    {
        _playerSound.PlayMatchLightingSound();
        SetBiggerLight();
        yield return new WaitForSeconds(matchLightingDuration);
        SetLightToDefault();
    }

    private void SetLightToDefault()
    {
        _spotlight.color = defaultLightColor;
        _spotlight.intensity = DefaultIntensity;
        _spotlight.pointLightInnerRadius = DefaultPointLightInnerRadius;
        _spotlight.pointLightOuterRadius = DefaultPointLightOuterRadius;
    }

    private void SetBiggerLight()
    {
        _spotlight.color = matchLightColor;
        _spotlight.intensity = MatchIntensity;
        _spotlight.pointLightInnerRadius = MatchPointLightInnerRadius;
        _spotlight.pointLightOuterRadius = MatchPointLightOuterRadius;
    }
}