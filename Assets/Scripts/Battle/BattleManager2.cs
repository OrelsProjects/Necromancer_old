using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager2 : MonoBehaviour
{
    public static BattleManager2 Instance { get; private set; }

    [SerializeField]
    private Zombifieable _civPrefab;
    [SerializeField]
    private List<Transform> _defendersSpawnPoints;

    private List<Zombie> _zombies = new List<Zombie>();
    private List<Zombifieable> _zombifieables;
    private List<Zombifieable> _zombifieablesAssigned;
    private bool _isRoundActive = false;

    private GameObject _zombiesParent;
    private GameObject _zombifieablesParent;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _zombiesParent = new GameObject("Zombies");
            _zombifieablesParent = new GameObject("Zombifieables");
        }
        else
        {
            Destroy(gameObject);
        }
        RandomSpawnCivs();
    }

    private void RandomSpawnCivs()
    {
        _zombifieables = new List<Zombifieable>();
        _zombifieablesAssigned = new();
        float screenWidth = Camera.main.orthographicSize;
        float screenHeight = Camera.main.orthographicSize;

        for (int i = 0; i < 40; i++)
        {
            // Instantiate zombifieable at random position within screen bounds.
            Vector3 randomPosition = new(Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight), 0);
            Zombifieable zombifieable = Instantiate(_civPrefab, randomPosition, Quaternion.identity, _zombifieablesParent.transform);
            _zombifieables.Add(zombifieable);
        }
    }

    public void AddZombie(Zombie zombie)
    {
        zombie.transform.parent = _zombiesParent.transform;
        _zombies.Add(zombie);
        if (!_isRoundActive)
        {
            _isRoundActive = true;

            for (int i = 0; i < 5; i++)
            {
                // random position is random in _spawnPoints
                Vector3 randomPosition = _defendersSpawnPoints[Random.Range(0, _defendersSpawnPoints.Count)].position;
                var ranger = Instantiate(CharactersManager.Instance.GetPrefab(CharacterName.Ranger), randomPosition, Quaternion.identity);
                _zombifieables.Add(ranger.GetComponent<Zombifieable>());
            }
        }
    }

    public Damageable GetClosestZombie(Vector3 position)
    {
        CleanupLists();
        Damageable closestZombifieable = null;
        float closestDistance = float.MaxValue;
        foreach (var zombifieable in _zombies)
        {
            Damageable zombie = zombifieable.GetComponent<Damageable>();
            if (zombie.IsDead())
            {
                continue;
            }
            float distance = Vector3.Distance(position, zombifieable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestZombifieable = zombie;
            }
        }
        return closestZombifieable;
    }

    public Zombifieable GetClosestZomifieable(Transform position)
    {
        CleanupLists();

        if (_zombifieables.Count <= 0 || position == null)
        {
            return null;
        }
        // Return the closest hero that is not assigned to an enemy. If there's none, return the closest hero
        Zombifieable closestZomifieable = _zombifieables[0];
        float closestDistance = Vector2.Distance(position.position, closestZomifieable.transform.position);
        foreach (Zombifieable zombifieable in _zombifieables)
        {
            if (zombifieable.IsZombified)
            {
                continue;
            }
            float distance = Vector2.Distance(position.position, zombifieable.transform.position);
            if (distance < closestDistance && !_zombifieablesAssigned.Contains(zombifieable))
            {
                closestDistance = distance;
                closestZomifieable = zombifieable;
            }
        }
        ListUtils.AddUnique(ref _zombifieablesAssigned, closestZomifieable);
        return closestZomifieable;
    }
    private void CleanupLists()
    {
        List<Zombifieable> zombifieablesToRemove = new();
        List<Zombie> zombiesToRemove = new();

        foreach (Zombifieable zombifieable in _zombifieables)
        {
            if (zombifieable == null || zombifieable.gameObject == null || zombifieable.IsZombified)
            {
                zombifieablesToRemove.Add(zombifieable);
            }
        }

        foreach (Zombie zombie in _zombies)
        {
            if (zombie == null || zombie.gameObject == null || zombie.GetComponent<Damageable>().IsDead())
            {
                zombiesToRemove.Add(zombie);
            }
        }

        zombifieablesToRemove.ForEach(enemy =>
        {
            _zombifieables.Remove(enemy);
            _zombifieablesAssigned.Remove(enemy);
        });

        zombiesToRemove.ForEach(zombie =>
        {
            _zombies.Remove(zombie);
        });

        if (_zombifieablesAssigned.Count == _zombifieables.Count)
        {
            _zombifieablesAssigned.Clear();
        }
    }

    public bool IsBattleOver()
    {
        bool isAllZombified = _zombifieables.Count == 0;
        bool isAllZombiesDead = false;
        return isAllZombified || isAllZombiesDead;
    }
}
