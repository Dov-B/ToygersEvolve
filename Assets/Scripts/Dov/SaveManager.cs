using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string SaveUrl = "http://votre-serveur.com/save";
    private string LoadUrl = "http://votre-serveur.com/load";


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public IEnumerator SavePlayerData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(SaveUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Progression sauvegardée avec succès !");
        else
            Debug.LogError("Erreur lors de la sauvegarde : " + request.error);
    }

    public IEnumerator LoadPlayerData(string playerId, System.Action<PlayerData> onSuccess, System.Action onError)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{LoadUrl}?playerId={playerId}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            onSuccess?.Invoke(data);
        }
        else
        {
            Debug.LogError("Erreur lors du chargement : " + request.error);
            onError?.Invoke();
        }
    }
}
