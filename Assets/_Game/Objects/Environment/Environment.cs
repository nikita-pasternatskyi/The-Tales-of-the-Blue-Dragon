using MP.Game.Assets.Utils;
using System.Collections;
using UnityEngine;

namespace MP.Game.Objects.Environment
{

    public class Environment : UnitySingleton<Environment>
    {
        [SerializeField] private EnvironmentSettings _currentSettings;
        [SerializeField] private float _defaultDuration;
        [SerializeField] private Material _skyboxMaterial;
        private Coroutine _currentCoroutine;

        private void Awake()
        {
            RenderSettings.skybox = _skyboxMaterial;
            if (_currentSettings != null)
                ApplyEnvironment(_currentSettings);
            EnsureSingleton();
        }

        public void TransitionTo(EnvironmentSettings settings)
        {
            TransitionTo(settings, _defaultDuration);
        }
        
        public void TransitionTo(EnvironmentSettings newSettings, float duration)
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(Transition(newSettings, duration));
        }

        private IEnumerator Transition(EnvironmentSettings newSettings, float duration)
        {
            float time = 0;
            RenderSettings.skybox.SetTexture("_Tex_Blend", newSettings.SkyboxTexture);
            do
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / duration);
                if (duration == 0)
                    t = 1;
                var groundColor = Color.Lerp(_currentSettings.GroundColor, newSettings.GroundColor, t);
                var equatorColor = Color.Lerp(_currentSettings.EquatorColor, newSettings.EquatorColor, t);
                var skyColor = Color.Lerp(_currentSettings.SkyColor, newSettings.SkyColor, t);
                var fogColor = Color.Lerp(_currentSettings.FogColor, newSettings.FogColor, t);
                var fogDensity = Mathf.Lerp(_currentSettings.FogDensity, newSettings.FogDensity, t);
                var sunColor = Color.Lerp(_currentSettings.SunColor, newSettings.SunColor, t);
                var skyboxTint = Color.Lerp(_currentSettings.SkyboxTint, newSettings.SkyboxTint, t);
                var exposure = Mathf.Lerp(_currentSettings.SkyboxExposure, newSettings.SkyboxExposure, t);

                RenderSettings.ambientEquatorColor = equatorColor;
                RenderSettings.ambientSkyColor = skyColor;
                RenderSettings.ambientGroundColor = groundColor;
                RenderSettings.fogColor = fogColor;
                RenderSettings.fogDensity = fogDensity;
                RenderSettings.sun.color = sunColor;
                RenderSettings.skybox.SetColor("_TintColor", skyboxTint);
                RenderSettings.skybox.SetFloat("_CubemapTransition", t);
                RenderSettings.skybox.SetFloat("_Exposure", exposure);
                yield return null;
            }
            while (time <= duration);
            RenderSettings.skybox.SetFloat("_EnableFog", newSettings.UseFog ? 1 : 0);
            RenderSettings.skybox.SetTexture("_Tex", newSettings.SkyboxTexture);
            RenderSettings.skybox.SetFloat("_Rotation", newSettings.RotateSkybox ? 1f : 0f);
            RenderSettings.skybox.SetFloat("_RotationSpeed", newSettings.RotationSpeed);
            
            _currentSettings = newSettings;
        }

        public void ApplyEnvironment(EnvironmentSettings settings)
        {
            TransitionTo(settings, 0);
        }
    }
}
