using UnityEngine;

public class CombatComponent
{
    Bullet _bullet;
    Transform _shootingPivot;
    int _maxBulletSpeed;
    LayerMask layer;

    public CombatComponent(Bullet _bullet, Transform _shootingPivot, int _maxBulletSpeed,LayerMask layer)
    {
        this._bullet = _bullet;
        this._shootingPivot = _shootingPivot;
        this._maxBulletSpeed = _maxBulletSpeed;
        this.layer = layer;

    }

    public void AttackTargetRange(Transform target)
    {
        
        Bullet _bulletclone = Bullet.Instantiate(_bullet, _shootingPivot.position, Quaternion.identity);
       _bulletclone.transform.forward = _shootingPivot.forward;
       _bulletclone.Initialize(target, _maxBulletSpeed, layer);
        
        
    }

  

    //public bool LookForTarget(out T item)
    //{
    //    foreach (T target in _targets)
    //    {
    //        if (_fov.inFOV(target.transform.position))
    //        {
    //            item = target;
    //            break;
    //        }
    //    }
    //    item= null;

    //    return false;
    //}

 
}
