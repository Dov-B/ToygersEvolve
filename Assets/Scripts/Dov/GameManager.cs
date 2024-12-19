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

        string localPlayerId = PlayerIdentity.GetPlayerId();
        if (string.IsNullOrEmpty(localPlayerId))
        {
            // Si aucun Player ID local, r�cup�rer depuis le backend
            StartCoroutine(PlayerIdentity.FetchPlayerIdFromServer(
                playerId =>
                {
                    Debug.Log("Player ID r�cup�r� depuis Telegram : " + playerId);
                    InitializePlayerData(playerId);
                },
                () =>
                {
                    Debug.LogError("Impossible de r�cup�rer le Player ID !");
                }
            ));
        }
        else
        {
            // ID d�j� localement disponible
            Debug.Log("Player ID local : " + localPlayerId);
            InitializePlayerData(localPlayerId);
        }
    }

    private void InitializePlayerData(string playerId)
    {
        // Charger les donn�es depuis le serveur ou cr�er un nouveau joueur
        StartCoroutine(_saveManager.LoadPlayerData(
            playerId,
            data =>
            {
                _currentPlayerData = data;
                Debug.Log("Donn�es du joueur charg�es : " + JsonUtility.ToJson(_currentPlayerData));
            },
            () =>
            {
                Debug.Log("Aucune donn�e trouv�e, cr�ation d'un nouveau joueur.");
                _currentPlayerData = new PlayerData(playerId, 1, 0);
            }
        ));
    }

    public void SaveProgress()
    {
        if (_currentPlayerData != null)
            StartCoroutine(_saveManager.SavePlayerData(_currentPlayerData));
    }

}
