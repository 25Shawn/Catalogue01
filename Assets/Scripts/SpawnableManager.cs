using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Classe de gestion d'affichage d'une chaise et de son déplacement.
/// </summary>
public class SpawnableManager : MonoBehaviour
{
    [SerializeField]
    private ARRaycastManager m_RaycastManager;

    private List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    [SerializeField]
    private GameObject spawnablePrefab;

    private Camera arCam;
    private GameObject spawnedObject;
    private bool estEnDeplacement;


    /// <summary>
    /// Méthode de commencement qui initialise l'objet à afficher et la camera.
    /// </summary>
    void Start()
    {
        spawnedObject = null;
        
        arCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    /// <summary>
    /// Méthode qui fait l'action de déplacer la chaise en maintenant le bouton gauche de la souris.
    /// </summary>
    /// <param name="context">le clique du bouton gauche de la souris</param>
    public void DeplacerChaise(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
       
            estEnDeplacement = true;

        }
        else if (context.canceled)
        {
            estEnDeplacement = false;
        }
    }
    

    /// <summary>
    /// Méthode qui affiche la chaise avec un Input Action. 
    /// Donc, ici le input action est le bouton gauche de la souris.
    /// </summary>
    /// <param name="context">Le bouton gauche de la souris</param>
    public void AfficherChaise(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // code inspiré par la documentation de unity: https://learn.unity.com/tutorial/placing-and-manipulating-objects-in-ar#605103a5edbc2a6c32bf5663
            Ray ray = arCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                
                if (m_RaycastManager.Raycast(Input.mousePosition, m_Hits))
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        
                        if (hit.collider.gameObject.tag == "SpawnableObject")
                        {
                            spawnedObject = hit.collider.gameObject;
                        }
                        else
                        {
                            
                            SpawnPrefab(m_Hits[0].pose.position);
                        }
                    }
                }
        }
    }
    /// <summary>
    /// Méthode qui instantie la chaise dans l'espace.
    /// </summary>
    /// <param name="spawnPosition"> la position de la souris lors du clique</param>
    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// Méthode de mise à jour de la scène qui fait la logique pour le déplacement de la chaise instantié.
    /// </summary>
    private void Update()
    {
        if (!estEnDeplacement || spawnedObject == null)
        {
            return;
        }

        
        if (m_RaycastManager.Raycast(Input.mousePosition, m_Hits))
        {
            Vector3 nouvellePosition = m_Hits[0].pose.position;
            spawnedObject.transform.position = nouvellePosition;
        }
    }
}
