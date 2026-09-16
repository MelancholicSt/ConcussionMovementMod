using Cysharp.Threading.Tasks;
using EFT;
using EFT.HealthSystem;

using Contusion = EFT.HealthSystem.ActiveHealthController.Contusion;

namespace ConcussionMovementMod.EffectHandlers.ContusionEffect;

public class BotContusionEffectHandler : BaseBotEffectHandler<Contusion>
{
    public BotContusionEffectHandler(BotOwner bot) : base(bot)
    {
        Plugin.LogSource.LogInfo($"Created ContusionEffectHandler for Bot.\nIt's name is {bot.Profile.Nickname}");
        Plugin.LogSource.LogInfo($"Created contusion effect handler for player with ID: {bot.Profile.Id}");
    }

    protected override void OnEffectRemoved(IHealthEffect effect)
    {
        
    }

    protected override UniTask HandleEffect(Contusion effect)
    {
        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {
        
    }
}