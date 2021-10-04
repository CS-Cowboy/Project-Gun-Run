using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using com.braineeeeDevs.gunRun;
using UnityEngine;


namespace com.braineeeeDevs.gunRun
{
    public class Weapon : BasicObject, IShoot
    {
       protected int ammo;
        void IShoot.PropelProjectile(){
            
        }
        void IShoot.Reload(){
            
        }
    }
}