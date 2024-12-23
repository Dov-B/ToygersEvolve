using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private SaveManager _saveManager = null;
    private PlayerData _currentPlayerData = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _saveManager = SaveManager.Instance;

        // Récupérer le PlayerId depuis le backend
        StartCoroutine(PlayerIdentity.FetchPlayerIdFromServer(
            playerId =>
            {
                Debug.Log("Player ID récupéré depuis Telegram : " + playerId);

                // Une fois l'ID récupéré, charge les données du joueur
                InitializePlayerData(playerId);
            },
            () =>
            {
                Debug.LogError("Impossible de récupérer le Player ID !");
            }
        ));
    }

    private void InitializePlayerData(string playerId)
    {
        // Charger les données depuis le serveur ou créer un nouveau joueur
        StartCoroutine(_saveManager.LoadPlayerData(
            playerId,
            data =>
            {
                _currentPlayerData = data;
                Debug.Log("Données du joueur chargées : " + JsonUtility.ToJson(_currentPlayerData));
            },
            () =>
            {
                Debug.Log("Aucune donnée trouvée, création d'un nouveau joueur.");
                _currentPlayerData = new PlayerData(playerId, 1, 0);
                SaveProgress();
            }
        ));
    }

    public void SaveProgress()
    {
        if (_currentPlayerData != null)
            StartCoroutine(_saveManager.SavePlayerData(_currentPlayerData));
    }

}
