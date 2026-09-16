using System;
using BepInEx.Logging;
using Comfort.Common;
using ConcussionMovementMod.EffectHandlers;
using ConcussionMovementMod.EffectHandlers.ContusionEffect;
using EFT;
using EFT.Ballistics;

using UnityEngine;

using Contusion = EFT.HealthSystem.ActiveHealthController.Contusion;

namespace ConcussionMovementMod;

public class MainPlayerManager : MonoBehaviour
{
    private GameWorld _gameWorld;
    private Player _mainPlayer;

    private void Awake()
    {
        Plugin.LogSource.Log(LogLevel.Debug, "MainPlayerManager Awake");
    }

    private void Update()
    {
        if (!_gameWorld && Singleton<GameWorld>.Instantiated)
        {
            _gameWorld = Singleton<GameWorld>.Instance;
            _gameWorld.AfterGameStarted += this.OnGameStarted;
            _gameWorld.OnPersonAdd += OnPersonAdd;
        }
        if (!_gameWorld)
            return;

        if (!_mainPlayer)
            return;

        if (Input.GetKeyUp(KeyCode.P))
        {
            this._mainPlayer.ActiveHealthController.DoContusion(Plugin.Configuration.ContusionDuration.Value, 100);
        }
    }

    private void OnPersonAdd(IPlayer person)
    {
        Plugin.LogSource.LogInfo($"Person added: {person}");
        
        Plugin.LogSource.LogInfo("Creating Player's ContusionEffectHandler");

        BaseEffectHandler<Contusion> contusionEffectHandler = person switch
        {
            Player player => new PlayerContusionEffectHandler(player),
            BotOwner bot => new BotContusionEffectHandler(bot),

            _ => new EmptyContusionEffectHandler(person)
        };


        /* Handler to remove effectHandler refs for GC.
         * Once IPlayer dead - the effect handler should be removed from memory 
         */
        Action<IPlayer> onPlayerDeathHandler = null;
            
        onPlayerDeathHandler = (_) =>
        {
            contusionEffectHandler.Dispose();
            person.OnIPlayerDeadOrUnspawn -= onPlayerDeathHandler;
        };
        person.OnIPlayerDeadOrUnspawn += onPlayerDeathHandler;
    }

    private void OnGameStarted()
    {
        this._mainPlayer = this._gameWorld.MainPlayer;
    
    }

    private void OnHit(DamageInfo damageInfo, EBodyPart bodyPartHit, float arg3)
    {
        Plugin.LogSource.LogInfo(
            $"Charachter got hit: {damageInfo.DamageType}, {bodyPartHit.ToString()}"
        );
    }
}
