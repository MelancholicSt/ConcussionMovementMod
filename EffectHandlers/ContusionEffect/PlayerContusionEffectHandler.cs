using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using EFT;
using EFT.GameTriggers;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using UnityEngine;
using Contusion = EFT.HealthSystem.ActiveHealthController.Contusion;

namespace ConcussionMovementMod.EffectHandlers.ContusionEffect;

public class PlayerContusionEffectHandler : BasePlayerEffectHandler<Contusion>
{


    private CancellationTokenSource _swayCancellationTokenSource;
    private bool _isDisorientationActive;

    public PlayerContusionEffectHandler(Player player)
        : base(player)
    {
        this._isDisorientationActive = false;

        Plugin.LogSource.LogInfo(
            $"Created contusion effect handler for player with ID: {player.Profile.Id}"
        );
    }

    protected override void OnEffectRemoved(IHealthEffect effect)
    {
        this._isDisorientationActive = false;
    }

    protected override void OnEffectAdded(IHealthEffect effect)
    {
        this._isDisorientationActive = true;
    }

    protected override UniTask HandleEffect(Contusion effect)
    {
        this._swayCancellationTokenSource?.Cancel();
        this._swayCancellationTokenSource?.Dispose();
        this._swayCancellationTokenSource = new CancellationTokenSource();
        foreach (EBodyPart part in Enum.GetValues(typeof(EBodyPart)))
        {
            this.Player.ActiveHealthController.DoPain(
                part,
                Plugin.Configuration.ContusionPainDuration.Value,
                effect.OverallDuration * 3f,
                10
            );
        }

        this.Player.ActiveHealthController.AddEffect<ActiveHealthController.Tremor>(
            EBodyPart.Head,
            effect.OverallDuration / 2,
            Plugin.Configuration.ContusionTremorDuration.Value,
            30f,
            100f
        );
        this.StartSwayLoop(this._swayCancellationTokenSource.Token);
        return UniTask.CompletedTask;
    }

    private async UniTask StartSwayLoop(CancellationToken swayCancellationToken)
    {
        MovementContext movementContext = this.Player.MovementContext;

        float customPhase = 0f;

        while (_isDisorientationActive && Player.HealthController.IsAlive)
        {
            Vector2 currentDirection = movementContext.MovementDirection;

            if (currentDirection.sqrMagnitude < 0.001f || !Player.MovementContext.IsGrounded)
            {
                await UniTask.Yield();
                continue;
            }

            float currentSpeed = Mathf.Max(this.Player.Speed, 1.0f);

            float frequencyModifier = Plugin.Configuration.SwayFrequency.Value;

            customPhase += Time.fixedDeltaTime * currentSpeed * frequencyModifier;

            float sinTime = Mathf.Sin(customPhase);

            float radians = (sinTime * Plugin.Configuration.SwayMaxAmplitude.Value) * Mathf.Deg2Rad;

            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            movementContext.MovementDirection = new Vector2(
                currentDirection.x * cos - currentDirection.y * sin,
                currentDirection.x * sin + currentDirection.y * cos
            );
            await UniTask.Yield();
            
        }
    }

    public override void Dispose() { }
}
