using System.Collections;
using UnityEngine;
using Photon.Pun;

class Kalashnikov : Gun
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _numberOfShots;
    [SerializeField] private float _dispersion;
    private PhotonView _photonView;

    public override bool IsShooting { get; private protected set; }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public override void Reload()
    {
    }

    public override void Shoot()
    {
        var randomDispersion = Quaternion.Euler(0f, Random.Range(-_dispersion, _dispersion), 0f);
        StartCoroutine(ShootCoroutine(_numberOfShots));
    }

    private IEnumerator ShootCoroutine(int numberOfShots)
    {
        if (!IsShooting)
        {

            IsShooting = true;
            for (int i = 0; i < numberOfShots; i++)
            {
                var randomDispersion = Quaternion.Euler(0f, Random.Range(-_dispersion, _dispersion), 0f);
                _photonView.RPC("Fire", RpcTarget.AllViaServer, transform.position, transform.rotation * randomDispersion);
                yield return new WaitForSeconds(0.1f);
            }
            IsShooting = false;
        }
    }

    [PunRPC]
    public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject bullet;

        bullet = Instantiate(_bulletPrefab, position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Bullet>().InitializeBullet(_photonView.Owner, (rotation * Vector3.forward), Mathf.Abs(lag));

    }

}
