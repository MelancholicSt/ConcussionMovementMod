using EFT;
using UnityEngine;

namespace ConcussionMovementMod;

public class PlayerEffectsDispatcher : MonoBehaviour
{
    
    
    public void Init(GameWorld gameWorld)
    {
        if (gameWorld is null)
        {
            Plugin.LogSource.LogWarning("GameWorld is null.");
            return;
        }
        
        gameWorld.OnPersonAdd += OnPersonAdded;
    }

    private void OnPersonAdded(IPlayer player)
    {
        
    }
}