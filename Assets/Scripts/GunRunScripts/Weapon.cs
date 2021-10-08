using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using com.braineeeeDevs.gr;
using UnityEngine;


namespace com.braineeeeDevs.gr
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