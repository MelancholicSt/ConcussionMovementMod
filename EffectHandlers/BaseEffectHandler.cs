using System;
using Cysharp.Threading.Tasks;
using EFT;
using EFT.HealthSystem;

namespace ConcussionMovementMod.EffectHandlers;

public abstract class BaseEffectHandler<TEffectType> : IDisposable where TEffectType : IHealthEffect
{
    protected readonly IPlayer BasePerson;
    
    public abstract void Dispose();
    
    
    protected BaseEffectHandler(IPlayer basePerson)
    {
        if(basePerson == null) 
            return;
        
        this.BasePerson = basePerson;
        
        
        if (this.BasePerson.HealthController is null) 
            return;
        
        this.BasePerson.HealthController.EffectStartedEvent += OnEffectStarted;
        this.BasePerson.HealthController.EffectRemovedEvent += OnEffectRemoved;
        this.BasePerson.HealthController.EffectAddedEvent += OnEffectAdded;
    }

    protected virtual void OnEffectStarted(IHealthEffect effect)
    {
        if (effect is TEffectType e)
        { 
            this.HandleEffect(e).Forget();  
        } 
    }

    protected abstract void OnEffectRemoved(IHealthEffect effect);

    protected virtual void OnEffectAdded(IHealthEffect effect)
    {
        if (effect is TEffectType e)
        {
            this.HandleEffect(e);
        }
    }

    protected abstract UniTask HandleEffect(TEffectType effect);


}