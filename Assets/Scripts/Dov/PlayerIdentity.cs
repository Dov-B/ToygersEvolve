using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class PlayerIdentity
{
    private const string PlayerIdKey = "PlayerId";
    private const string BackendUrl = "http://votre-serveur.com/getPlayerId"; // URL du backend

    // R�cup�rer ou g�n�rer un identifiant localement
    public static string GetPlayerId()
    {
        if (PlayerPrefs.HasKey(PlayerIdKey))
            return PlayerPrefs.GetString(PlayerIdKey);

        return null; // Si aucun ID n'est encore disponible
    }

    // Sauvegarder l'ID r�cup�r� depuis le backend
    public static void SetPlayerId(string playerId)
    {
        PlayerPrefs.SetString(PlayerIdKey, playerId);
    }

    // R�cup�rer l'ID Telegram depuis le backend
    public static IEnumerator FetchPlayerIdFromServer(System.Action<string> onSuccess, System.Action onFailure)
    {
        UnityWebRequest request = UnityWebRequest.Get(BackendUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string playerId = request.downloadHandler.text;
            SetPlayerId(playerId); // Sauvegarde locale
            onSuccess?.Invoke(playerId);
        }
        else
        {
            Debug.LogError("Erreur lors de la r�cup�ration du Player ID : " + request.error);
            onFailure?.Invoke();
        }
    }
}
