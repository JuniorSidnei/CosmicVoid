using ProjectA.Entity;

namespace ProjectA.Interface {
    
    public interface IDamageable {
        void ProcessDamage(bool isCharged);
        void ProcessPlayerDamage(bool isCharged);
        void ProcessProjectileDamage(bool isReflected, int damagePower);
        void ProcessProjectileDamage(ReflectiveEntity reflectiveEntity);
    }
}