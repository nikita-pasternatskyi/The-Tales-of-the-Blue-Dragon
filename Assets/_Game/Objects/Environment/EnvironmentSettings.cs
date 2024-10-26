using UnityEngine;

namespace MP.Game.Objects.Environment
{
    [CreateAssetMenu(menuName = "MP/Environment/EnvironmentSettings")]
    public class EnvironmentSettings : ScriptableObject
    {
        [Header("Ambient lighting")]
        [ColorUsage(true, true)] public Color SkyColor;
        [ColorUsage(true, true)] public Color EquatorColor;
        [ColorUsage(true, true)] public Color GroundColor;

        [Header("Misc")]
        public bool UseFog = true;
        [ColorUsage(true, true)] public Color FogColor;
        public float FogDensity;
        public Color SunColor;

        [Header("Skybox")]
        public Texture SkyboxTexture;
        [ColorUsage(true, true)] public Color SkyboxTint;
        public bool RotateSkybox;
        public float RotationSpeed;
        [Range(0,8)] public float SkyboxExposure;
    }
}
