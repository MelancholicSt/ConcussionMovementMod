using EFT;
using EFT.HealthSystem;

namespace ConcussionMovementMod.EffectHandlers;


// TODO: Implement effect that modifies Bot's movement
public abstract class BaseBotEffectHandler<TEffect>(BotOwner bot)  : BaseEffectHandler<TEffect>(bot) where TEffect : IHealthEffect
{
    protected BotOwner Bot => (BotOwner)base.BasePerson;
}