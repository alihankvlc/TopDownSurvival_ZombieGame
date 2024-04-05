using System;
using System.Collections;
using UnityEngine.Serialization;

namespace DeadNation
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.AI;

    public class SpawnManager : Singleton<SpawnManager>
    {
        #region ZombieObjectPooling

        [FormerlySerializedAs("m_ZombiePrefabList")] [Header("Zombie")] [SerializeField]
        private List<GameObject> _zombiePrefabList = new List<GameObject>();

        private List<GameObject> _zombiePoolList = new List<GameObject>();

        private float _offset = 5f;
        private float _generateObjectRadius = 1f;

        private WaitForSeconds m_InitialSpawnDelay;
        private WaitForSeconds m_SpawnInterval;
        private WaitForSeconds m_WaveSpawnInterval;

        public static event Action<int, int> OnChangeZombieCount;
        public static event Action<float> OnChangeWaveStateDuration;

        #endregion

        #region ZombieDropObjects

        [Header("ZombieDropObjects")] [SerializeField]
        private List<GameObject> _zombieDropObjectList = new List<GameObject>();

        private Dictionary<DropType, GameObject> _zombieRewardDropDictionary = new Dictionary<DropType, GameObject>();

        #endregion

        #region BulletPool

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private List<GameObject> _bulletPool = new();

        #endregion

        private void Start()
        {
            CreateZombieObjectPool(_zombiePrefabList, _zombiePoolList);
            //CreateBulletPool();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(GenerateZombieWave(1, 3f, 1f));
            }
        }

        #region ZombieObjectPooling

        public void SpawnZombie(int spawnCount, float initialSpawnDuration, float spawnInterval)
        {
            StartCoroutine(GenerateZombieWave(spawnCount, 3f, 1f));
        }

        private IEnumerator GenerateZombieWave(int spawnCount, float initialSpawnDelay, float spawnInterval)
        {
            OnChangeWaveStateDuration?.Invoke((initialSpawnDelay));
            yield return new WaitForSeconds(initialSpawnDelay);
            OnChangeWaveStateDuration?.Invoke((spawnCount));
            for (int i = 0; i < spawnCount; i++)
            {
                GenerateRandomSpawnPosition();
                OnChangeZombieCount?.Invoke(i + 1, spawnCount);

                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void GenerateRandomSpawnPosition()
        {
            Vector3 point = Vector3.zero;
            Vector3 randomViewportPoint = GetViewportPoint(out var offset);

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ViewportPointToRay(randomViewportPoint);

            if (plane.Raycast(ray, out float dist))
            {
                point = ray.GetPoint(dist) + offset;
                point = GetValidSpawnPoint(point);
            }
        }

        public GameObject GetRandomZombiePoolObject(PoolOperation param)
        {
            bool checkParam = param == PoolOperation.GetObject ? true : false;

            List<GameObject> candidateObjects =
                _zombiePoolList.Where(obj => checkParam ? !obj.activeSelf : obj.activeSelf).ToList();

            if (candidateObjects.Count == 0)
            {
                return null;
            }

            int randomIndex = Random.Range(0, candidateObjects.Count);
            GameObject selectedObject = candidateObjects[randomIndex];

            selectedObject.SetActive(checkParam);
            return selectedObject;
        }

        private void CreateZombieObjectPool(List<GameObject> prefab, List<GameObject> poolList)
        {
            foreach (GameObject obj in prefab)
            {
                GameObject spawnObject = Instantiate(obj);
                spawnObject.SetActive(false);
                poolList.Add(spawnObject);
            }
        }

        private Vector3 GetValidSpawnPoint(Vector3 initialPoint)
        {
            NavMeshHit hit;
            Vector3 newPoint = initialPoint;
            int attempts = 0;

            while (!NavMesh.SamplePosition(newPoint, out hit, _offset, NavMesh.AllAreas) && attempts < 5)
            {
                newPoint = initialPoint + Random.insideUnitSphere * _offset;
                attempts++;
            }

            if (NavMesh.SamplePosition(newPoint, out hit, _offset, NavMesh.AllAreas))
            {
                GetRandomZombiePoolObject(PoolOperation.GetObject).transform.position = hit.position;
                return hit.position;
            }
            else
            {
                GetRandomZombiePoolObject(PoolOperation.GetObject).transform.position = initialPoint;
                Debug.LogWarning("NavMesh alanında geçerli bir nokta bulunamadı. Başlangıç noktası kullanılıyor.");
                return initialPoint;
            }
        }

        private Vector3 GetSpawnOffsetByViewportPosition(Vector3 vector, float sign)
        {
            return vector * sign * _offset;
        }

        private Vector3 GetViewportPoint(out Vector3 offset)
        {
            var viewportPoint = Vector3.zero;

            offset = Vector3.zero;

            if (Random.value > 0.5f)
            {
                var dir = Mathf.Round(Random.value);
                viewportPoint = new Vector3(dir, Random.value);

                offset = GetSpawnOffsetByViewportPosition(Vector3.right, dir < 0.001f ? -1f : 1f);
            }
            else
            {
                var dir = Mathf.Round(Random.value);
                viewportPoint = new Vector3(Random.value, dir);

                offset = GetSpawnOffsetByViewportPosition(Vector3.forward, dir < 0.001f ? -1f : 1f);
            }

            return viewportPoint;
        }

        #endregion

        #region Player

        private void CreateBulletPool()
        {
            for (int i = 0; i < 15; i++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Add(bullet);
            }
        }

        public GameObject GetBullet(Vector3 position, Quaternion rotation)
        {
            GameObject bullet = _bulletPool.FirstOrDefault(r => !r.activeSelf);
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.SetActive(true);

            return bullet;
        }

        #endregion
    }
}