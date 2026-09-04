using System;
using System.Collections;
using System.Linq;
using BepInEx.Logging;
using Comfort.Common;
using EFT;
using EFT.Ballistics;
using EFT.Communications;
using EFT.Game.Spawning;
using EFT.HealthSystem;
using UnityEngine;

namespace SPTClientModExamples;

public class MainPlayerManager : MonoBehaviour
{

    private GameWorld _gameWorld;
    private Player _mainPlayer;
    

    private bool _isDisorientationActive = false;

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
        }
        if (!_gameWorld) return;
        
        if (!_mainPlayer) return;
        
        if (Input.GetKeyUp(KeyCode.P))
        {
            this._mainPlayer.ActiveHealthController.DoContusion(10, 100);
        }
        
        
    }

    private void OnGameStarted()
    {
        this._mainPlayer = this._gameWorld.MainPlayer;
        InitializePlayerEventHandlers();
    }

    private void InitializePlayerEventHandlers()
    {
        _gameWorld.MainPlayer.ActiveHealthController.EffectStartedEvent += this.OnEffectStarted;
        _gameWorld.MainPlayer.ActiveHealthController.EffectRemovedEvent += this.OnEffectRemoved;
        _gameWorld.MainPlayer.BeingHitAction += this.OnHit;
    }

    private void OnHit(DamageInfo damageInfo, EBodyPart bodyPartHit, float arg3)
    {
        Plugin.LogSource.LogInfo($"Charachter got hit: {damageInfo.DamageType}, {bodyPartHit.ToString()}");
    }

    private void OnEffectRemoved(IHealthEffect obj)
    {
        if (obj is not ActiveHealthController.Contusion) return;
        
        this.StopPlayerDisorientation();
    }

    private void OnEffectStarted(IHealthEffect obj)
    {
        if (obj is not ActiveHealthController.Contusion) return;
        
        this.StartPlayerDisorientation();
    }
    
    
    private void StartPlayerDisorientation()
    {
        Plugin.LogSource.Log(LogLevel.Info, "Player is disoriented");
        _isDisorientationActive = true;

        StartCoroutine(RunMovementProblems());
    }
    private void StopPlayerDisorientation()
    {
        Plugin.LogSource.Log(LogLevel.Info, "Player is oriented");
        _isDisorientationActive = false;
    }

    private IEnumerator RunMovementProblems()
    {
        while (_isDisorientationActive)
        {
            Vector3 direction = _mainPlayer.MovementContext.AbsoluteMovementDirection;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                // Плавно меняем угол отклонения
                float currentAngle = Mathf.Sin(Time.time * 2f);

                direction = Quaternion.Euler(0f, currentAngle, 0f) * direction;
                
                
                _mainPlayer.CharacterController.Move(direction, Time.fixedDeltaTime);
            }

            yield return null;
        }
    }
}