using ProjectA.Entity.ProcessDamage;

namespace ProjectA.Interface {
    
    public class EnemyEntity : EntityProcessDamage, IDamageable {
        
        public void ProcessDamage(bool isCharged, PlayerHealth playerHealth) {
            if (!isCharged) {
                playerHealth.TakeDamage(DamagePower);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}