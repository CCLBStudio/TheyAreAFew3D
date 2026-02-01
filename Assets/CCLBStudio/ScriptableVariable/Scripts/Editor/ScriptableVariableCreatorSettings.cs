using UnityEngine;

namespace CCLBStudio.ScriptableVariable.Scripts
{
    [CreateAssetMenu(menuName = "CCLB Studio/Scriptable Variable/Editor/Settings", fileName = nameof(ScriptableVariableCreatorSettings))]
    public class ScriptableVariableCreatorSettings : ScriptableObject
    {
        public const string Token = "##";
        public static readonly string[] Tags = new []{"TYPE", "CAPITALTYPE"};

        public string saveAssetPath = "/CCLBStudio/ScriptableVariable/CreatedVariables/";
        public string saveTypePath = "/CCLBStudio/ScriptableVariable/Scripts/";
        [TextArea(3, 15)]
        public string simpleTypeTemplate;
        [TextArea(3, 15)]
        public string typeListTemplate;
        [TextArea(3, 15)]
        public string componentSetterTemplate;
    }
}