using EFT;
using EFT.HealthSystem;

namespace ConcussionMovementMod.EffectHandlers;

public abstract class BasePlayerEffectHandler<TEffect>(Player player) : BaseEffectHandler<TEffect>(player)
    where TEffect : IHealthEffect
{
    protected Player Player => (Player)base.BasePerson;
}