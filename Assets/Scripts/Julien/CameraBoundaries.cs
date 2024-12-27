using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    public float maxX, minX, maxY, minY, maxZ, minZ;
    public float smoothTime = 0.3f;  // Temps pour lisser les transitions

    private Vector3 velocity = Vector3.zero;  // Utilisé pour smooth damping

    // Update is called once per frame
    void FixedUpdate()
    {
        // Créer un vecteur contenant la position de la caméra avec des limites
        Vector3 targetPosition = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            Mathf.Clamp(transform.position.z, minZ, maxZ)
        );

        // Si la position actuelle est proche des limites, on n'applique pas SmoothDamp
        if (Mathf.Approximately(transform.position.x, targetPosition.x) &&
            Mathf.Approximately(transform.position.y, targetPosition.y) &&
            Mathf.Approximately(transform.position.z, targetPosition.z))
        {
            transform.position = targetPosition;  // Si on est déjà aux limites, on force la position sans lissage
        }
        else
        {
            // Sinon, on applique SmoothDamp pour lisser
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        Debug.Log(transform.position.x);  // Affiche la position de la caméra pour le debug
    }
}
