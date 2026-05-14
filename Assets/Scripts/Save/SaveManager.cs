using System.IO;
using UnityEngine;

public class SaveManager
    : MonoBehaviour
{
    public static SaveManager
        Instance;

    private string SavePath =>
        Path.Combine(
            Application.persistentDataPath,
            "save.json");

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

    private void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        GameSaveData saveData =
            new GameSaveData();

        CharacterController[]
            characters =
                FindObjectsByType
                <CharacterController>(
                    FindObjectsSortMode.None);

        foreach (var character
            in characters)
        {
            CharacterSaveIdentity id =
                character.GetComponent
                <CharacterSaveIdentity>();

            CharacterGrowth growth =
                character.GetComponent
                <CharacterGrowth>();

            CharacterDialogueRuntime
                dialogue =
                    character.GetComponent
                    <CharacterDialogueRuntime>();

            CharacterSaveData data =
                new CharacterSaveData();

            data.characterId =
                id.CharacterId;

            data.posX =
                character.transform
                    .position.x;

            data.posY =
                character.transform
                    .position.y;

            data.growthPoint =
                growth.CurrentGrowthPoint;

            data.growthStage =
                growth.CurrentStage;

            data.dialogueScript =
                dialogue.GetScript();

            saveData.characters
                .Add(data);
        }

        string json =
            JsonUtility.ToJson(
                saveData,
                true);

        File.WriteAllText(
            SavePath,
            json);

        Debug.Log(
            $"Saved: {SavePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log(
                "No Save File");

            return;
        }

        string json =
            File.ReadAllText(
                SavePath);

        GameSaveData saveData =
            JsonUtility.FromJson
            <GameSaveData>(json);

        CharacterController[]
            characters =
                FindObjectsByType
                <CharacterController>(
                    FindObjectsSortMode.None);

        foreach (var character
            in characters)
        {
            CharacterSaveIdentity id =
                character.GetComponent
                <CharacterSaveIdentity>();

            foreach (var save
                in saveData.characters)
            {
                if (save.characterId
                    != id.CharacterId)
                {
                    continue;
                }

                character.transform.position =
                    new Vector3(
                        save.posX,
                        save.posY,
                        character.transform
                            .position.z);

                CharacterGrowth growth =
                    character.GetComponent
                    <CharacterGrowth>();

                growth.LoadGrowthData(
                    save.growthPoint,
                    save.growthStage);

                CharacterDialogueRuntime
                    dialogue =
                        character.GetComponent
                        <CharacterDialogueRuntime>();

                dialogue.SetScript(
                    save.dialogueScript);

                break;
            }
        }

        Debug.Log("Loaded");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(
        bool pause)
    {
        if (pause)
        {
            SaveGame();
        }
    }
}