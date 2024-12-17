using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public string csvFolderPath = "Assets/CSVFiles"; // Cartella con i CSV
    public string objFolderPath = "Assets/Models";   // Cartella con gli OBJ
    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

    // Create a Map Container
    private GameObject mapContainer;

    void Start()
    {
        LoadAllModels();
        Debug.Log("Map and player loaded");
    }

    void LoadPlayer()
    {
        // Player should be loaded from a prefab in the Assets/Resources/Characters LadySylvanasWindrunner/SylvanasContainer 1.prefab
        GameObject playerPrefab = Resources.Load<GameObject>("Characters/LadySylvanasWindrunner/SylvanasContainer 1");
        if (playerPrefab != null)
        {
            GameObject player = Instantiate(playerPrefab);
            // Get child with "Player" tag
            Transform playerTransform = player.transform.Find("Player");

            playerTransform.position = new Vector3(-2843.069f, 424.3101f, 5379.471f);
        }
        else
        {
            Debug.LogError("Player prefab not found");
        }
    }

    void LoadAllModels()
    {
        // Loading Map
        string[] csvFiles = Directory.GetFiles(objFolderPath, "*_ModelPlacementInformation.csv");
        foreach (string csvFile in csvFiles)
        {
            string objFile = GetOBJFileFromCSV(csvFile);
            if (File.Exists(objFile))
            {
                GameObject objModel = LoadOBJ(objFile);
                string mtlFile = objFile.Replace(".obj", ".mtl");
                if (File.Exists(mtlFile))
                {
                    ApplyMaterialFromMTL(objModel, mtlFile, Path.GetDirectoryName(objFile));
                }
            }
        }

        LoadPlayer();
    }

    string GetOBJFileFromCSV(string csvFile)
    {
        string objName = Path.GetFileNameWithoutExtension(csvFile).Replace("_ModelPlacementInformation", "");
        return Path.Combine(Path.GetDirectoryName(csvFile), objName + ".obj");
    }

    GameObject LoadOBJ(string objFile)
    {
        // Usa la libreria Runtime OBJ Importer
        OBJLoader objLoader = new OBJLoader();
        GameObject loadedObj = objLoader.Load(objFile);

        if (loadedObj != null)
        {
            loadedObj.name = Path.GetFileNameWithoutExtension(objFile);

            return loadedObj;
        }
        else
        {
            Debug.LogError("Failed to load OBJ file: " + objFile);
            return null;
        }
    }

    void ApplyMaterialFromMTL(GameObject objModel, string mtlFile, string folderPath)
    {
        string[] lines = File.ReadAllLines(mtlFile);
        string textureFile = null;

        foreach (string line in lines)
        {
            if (line.StartsWith("map_Kd")) // Cerca la texture
            {
                textureFile = line.Split(' ')[1]; // Ottiene il nome della texture
                break;
            }
        }

        if (!string.IsNullOrEmpty(textureFile))
        {
            string texturePath = Path.Combine(folderPath, textureFile);
            if (File.Exists(texturePath))
            {
                Texture2D texture = LoadTexture(texturePath);
                if (texture != null)
                {
                    Material material = new Material(Shader.Find("Standard"));
                    material.mainTexture = texture;

                    MeshRenderer renderer = objModel.GetComponentInChildren<MeshRenderer>();
                    if (renderer != null)
                    {
                        renderer.material = material;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Texture file not found: " + texturePath);
            }
        }
    }

    Texture2D LoadTexture(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2); // Dimensioni placeholder
        if (texture.LoadImage(fileData))
        {
            return texture;
        }
        Debug.LogError("Failed to load texture: " + filePath);
        return null;
    }
}
