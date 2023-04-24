using System;
using System.Diagnostics.CodeAnalysis;
using Artees.UnitySemVer;
using UnityEditor;
using UnityEngine;

namespace Artees.AppVersion
{
    [CreateAssetMenu(menuName = "Artees/Application Version")]
    public class ApplicationVersion : ScriptableObject
    {
        [SerializeField] private SemVer version;
        [SerializeField] private bool applyOnBuild = true;

        [SerializeField]
        private bool buildVersionInAppName;
        [SerializeField]
        private string applicationName;

        [SuppressMessage("ReSharper", "ConvertToAutoPropertyWithPrivateSetter",
            Justification = "Serializable")]
        public SemVer Version => version;

        [SuppressMessage("ReSharper", "ConvertToAutoProperty", Justification = "Serializable")]
        public bool ApplyOnBuild => applyOnBuild;

        public bool BuildVersionInAppName => buildVersionInAppName;
        public string ApplicationName => applicationName;

        private string GetSanitizedVersion()
        {
            return "_" + version.major + "_" + version.minor + "_" + version.patch + "_" + Version.preRelease;
        } 

        private void Awake()
        {
            if (version != null &&
                (version.autoBuild != SemVerAutoBuild.Type.Manual || version != new SemVer())) return;
            version = SemVer.Parse(Application.version);
            
#if UNITY_EDITOR
            if (BuildVersionInAppName)
                PlayerSettings.productName = ApplicationName + GetSanitizedVersion();
            else
                PlayerSettings.productName = ApplicationName;
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (BuildVersionInAppName)
                PlayerSettings.productName = ApplicationName + GetSanitizedVersion();
            else
                PlayerSettings.productName = ApplicationName;
        }
#endif
    }
}