using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace ConcussionMovementMod
{
    // first string below is your plugin's GUID, it MUST be unique to any other mod. Read more about it in BepInEx docs. Be sure to update it if you copy this project.
    [BepInPlugin("ConcussionMovementMod.UniqueGUID", "ConcussionMovementMod", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ModConfiguration Configuration;
        public static ManualLogSource LogSource;

        // BaseUnityPlugin inherits MonoBehaviour, so you can use base unity functions like Awake() and Update()
        private void Awake()
        {
            LogSource = Logger;
            
            this.InitializeConfiguration();

            this.gameObject.AddComponent<MainPlayerManager>();

            LogSource.LogInfo("Concussion Movement Mod ");
        }

        private void InitializeConfiguration()
        {
            
            Configuration = new ModConfiguration
            {
                SwayFrequency = Config.Bind("Debug", "Sway Frequency Coefficient", 1.5f),
                SwayMaxAmplitude = Config.Bind("Debug", "Max Sway Amplitude", 100f),
                ContusionDuration = Config.Bind("Debug", "Contusion Duration", 5f),
                ContusionPainDuration = Config.Bind("Debug", "Contusion Pain Duration", 5f),
                ContusionTremorDuration = Config.Bind("Debug", "Contusion Tremor Duration", 5f),
            };
        }
    }
}
