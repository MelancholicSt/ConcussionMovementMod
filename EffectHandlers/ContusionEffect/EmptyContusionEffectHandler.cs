using Cysharp.Threading.Tasks;
using EFT;
using EFT.HealthSystem;

namespace ConcussionMovementMod.EffectHandlers.ContusionEffect;

public class EmptyContusionEffectHandler(IPlayer basePerson)
    : BaseEffectHandler<ActiveHealthController.Contusion>(basePerson)
{
    public override void Dispose()
    {
        
    }

    protected override void OnEffectRemoved(IHealthEffect effect)
    {
        
    }

    protected override UniTask HandleEffect(ActiveHealthController.Contusion effect)
    {
        return UniTask.FromResult(UniTask.CompletedTask);
    }
}