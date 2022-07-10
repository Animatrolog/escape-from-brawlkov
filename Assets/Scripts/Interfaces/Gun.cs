using UnityEngine;
using Photon.Pun;

abstract class Gun : MonoBehaviourPunCallbacks
{
    public PlayerChar OwnerChar;

    public abstract bool IsShooting { get; private protected set;}

    public abstract void Shoot();

    public abstract void Reload();
}
