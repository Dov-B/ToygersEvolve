using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class PlayerIdentity
{
    private const string PlayerIdKey = "PlayerId";
    private const string BackendUrl = "http://votre-serveur.com/getPlayerId"; // URL du backend

    // Récupérer ou générer un identifiant localement
    public static string GetPlayerId()
    {
        if (PlayerPrefs.HasKey(PlayerIdKey))
            return PlayerPrefs.GetString(PlayerIdKey);

        return null; // Si aucun ID n'est encore disponible
    }

    // Sauvegarder l'ID récupéré depuis le backend
    public static void SetPlayerId(string playerId)
    {
        PlayerPrefs.SetString(PlayerIdKey, playerId);
    }

    // Récupérer l'ID Telegram depuis le backend
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
            Debug.LogError("Erreur lors de la récupération du Player ID : " + request.error);
            onFailure?.Invoke();
        }
    }
}
