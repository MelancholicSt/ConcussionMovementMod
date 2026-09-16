using BepInEx.Configuration;

namespace ConcussionMovementMod;


public class ModConfiguration
{
    public ConfigEntry<float> SwayFrequency;
    public ConfigEntry<float> SwayMaxAmplitude;
    
    public ConfigEntry<float> ContusionDuration;
    public ConfigEntry<float> ContusionPainDuration;
    public ConfigEntry<float> ContusionTremorDuration;
    
    
}