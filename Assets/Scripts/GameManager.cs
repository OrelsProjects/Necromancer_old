using Assets.Scripts;

using UnityEngine;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        BattleManager.Instance.SetEnemies(new CharacterName[] { CharacterName.Orc, CharacterName.Knight, CharacterName.Knight });
        BattleManager.Instance.SetHeroes(new CharacterName[] { CharacterName.Ranger, CharacterName.Knight, CharacterName.Mage });
        // BattleManager.Instance.SetEnemies(new EnemyType[] { EnemyType.Orc });
        // BattleManager.Instance.SetHeroes(new CharacterName[] { CharacterName.Ranger });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
