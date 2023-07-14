using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }

        [SerializeField]
        private Character[] AllHeroes;
        [SerializeField]
        private Character[] AllEnemies;

        [SerializeField]
        private Transform _spawnEnemy;
        [SerializeField]
        private Transform _spawnHero;

        private CharacterName[] _heroes;
        private CharacterName[] _enemies;

        private List<Damageable> _spawnedEnemies = new List<Damageable>();
        private List<Damageable> _spawnedHeroes = new List<Damageable>();

        private List<Damageable> _enemiesAssigned = new List<Damageable>(); // All the enemies that has a hero assigned to them to attack
        private List<Damageable> _heroesAssigned = new List<Damageable>(); // All the heroes that has an enemy assigned to them to attack

        private Vector3 _randomFactor;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _randomFactor = new(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            }
            else
            {
                Destroy(gameObject);
            }
        }



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // On space click start the game
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartBattle();
            }
        }

        public void SetHeroes(CharacterName[] heroes)
        {
            _heroes = heroes;
        }

        public void SetEnemies(CharacterName[] enemies)
        {
            _enemies = enemies;
        }

        public void StartBattle()
        {
            foreach (CharacterName hero in _heroes)
            {
                Damageable heroObject = Instantiate(CharactersManager.Instance.GetPrefab(hero)).GetComponent<Damageable>();
                heroObject.GetComponent<Character>().CharacterType = CharacterType.Hero;
                heroObject.transform.position = _spawnHero.position + _randomFactor;
                _spawnedHeroes.Add(heroObject);
            }

            foreach (CharacterName enemy in _enemies)
            {
                Damageable enemyObject = Instantiate(CharactersManager.Instance.GetPrefab(enemy)).GetComponent<Damageable>();
                enemyObject.GetComponent<Character>().CharacterType = CharacterType.Enemy;
                enemyObject.transform.position = _spawnEnemy.position + _randomFactor;
                enemyObject.transform.localScale = new Vector3(-enemyObject.transform.localScale.x, enemyObject.transform.localScale.y, enemyObject.transform.localScale.z);
                _spawnedEnemies.Add(enemyObject);
            }
        }
        public Damageable GetClosestEnemy(Transform position)
        {
            cleanupLists();

            if (_spawnedEnemies.Count <= 0 || position == null)
            {
                return null;
            }

            Damageable closestEnemy = _spawnedEnemies[0];
            float closestDistance = Vector2.Distance(position.position, closestEnemy.transform.position);
            foreach (Damageable Enemy in _spawnedEnemies)
            {
                float distance = Vector2.Distance(position.position, Enemy.transform.position);
                if (distance < closestDistance && (!_enemiesAssigned.Contains(Enemy) || _enemiesAssigned.Count == _spawnedEnemies.Count))
                {
                    closestDistance = distance;
                    closestEnemy = Enemy;
                }
            }
            ListUtils.AddUnique(ref _enemiesAssigned, closestEnemy);
            return closestEnemy;
        }

        public Damageable GetClosestHero(Transform position)
        {
            cleanupLists();

            if (_spawnedHeroes.Count <= 0 || position == null)
            {
                return null;
            }
            // Return the closest hero that is not assigned to an enemy. If there's none, return the closest hero
            Damageable closestHero = _spawnedHeroes[0];
            float closestDistance = Vector2.Distance(position.position, closestHero.transform.position);
            foreach (Damageable hero in _spawnedHeroes)
            {
                float distance = Vector2.Distance(position.position, hero.transform.position);
                if (distance < closestDistance && (!_heroesAssigned.Contains(hero) || _heroesAssigned.Count == _spawnedHeroes.Count))
                {
                    closestDistance = distance;
                    closestHero = hero;
                }
            }
            ListUtils.AddUnique(ref _heroesAssigned, closestHero);
            return closestHero;
        }
        private void cleanupLists()
        {
            List<Damageable> enemiesToRemove = new List<Damageable>();
            List<Damageable> heroesToRemove = new List<Damageable>();
             
            foreach (Damageable enemy in _spawnedEnemies)
            {
                if (enemy == null || enemy.gameObject == null || enemy.IsDead())
                {
                    enemiesToRemove.Add(enemy);
                }
            }

            foreach (Damageable hero in _spawnedHeroes)
            {
                if (hero == null || hero.gameObject == null || hero.IsDead())
                {
                    heroesToRemove.Add(hero);
                }
            }
            
            heroesToRemove.ForEach(hero =>
            {
                _spawnedHeroes.Remove(hero);
                _enemiesAssigned.Remove(hero);
            });

            enemiesToRemove.ForEach(enemy =>
            {
                _spawnedEnemies.Remove(enemy);
                _enemiesAssigned.Remove(enemy);
            });
        }

        public bool IsBattleOver()
        {
            bool isNotAllDead = _spawnedHeroes.Find(hero => hero.IsNotDead()) != null || _spawnedEnemies.Find(enemy => !enemy.IsNotDead()) != null;
            bool isNothingSpawned = _spawnedHeroes.Count <= 0 || _spawnedEnemies.Count <= 0;

            return !isNotAllDead || isNothingSpawned;
        }
    }

}