using ProjectA.Entity.Position;
using ProjectA.Scriptables;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;

namespace ProjectA.Pools {
    
    public class ProjectilesPool : PoolBase<EntityPosition> {

        public EntityPosition Prefab;
        public EntityInfo ReflectiveEntity;
        public EntityInfo ReflectiveBossEntity;
        public EntityInfo HardProjectileEntity;
        public EntityInfo ExplosiveEntity;
        public EntityInfo ShieldBreakerEntity;
        
        public EntityPosition GetFromPool() {
            return Get();
        }

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnProjectileEntityRelease>(OnReflectiveEntityRelease);
        }

        private void OnReflectiveEntityRelease(OnProjectileEntityRelease ev) {
            Release(ev.Entity);
        }

        private void Start() {
            InitPool(Prefab, 20, 40);
        }
        
    }
}