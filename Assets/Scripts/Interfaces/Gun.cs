
using System.Collections;
using UnityEngine;

abstract class Gun : MonoBehaviour
{
    public abstract bool IsShooting { get; private protected set;}

    public abstract void Shoot();

    public abstract void Reload();
}
