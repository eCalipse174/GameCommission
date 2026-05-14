using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailUI
    : MonoBehaviour
{
    public static CharacterDetailUI
        Instance;

    [SerializeField]
    private GameObject root;

    [SerializeField]
    private InputField scriptInputField;

    [Space]
    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text genderText;

    [SerializeField]
    private Text stageText;

    [SerializeField]
    private Text descriptionText;


    private CharacterDialogueRuntime
        currentRuntime;

    private void Awake()
    {
        Instance = this;

        root.SetActive(false);
    }

    public void Open(
        CharacterDialogueRuntime runtime)
    {
        currentRuntime = runtime;

        scriptInputField.text =
            runtime.GetScript();

        root.SetActive(true);

        CharacterController controller = runtime.GetComponent<CharacterController>();

        CharacterGrowth growth = runtime.GetComponent<CharacterGrowth>();

        CharacterData data =
            controller.Data;

        nameText.text =
            data.characterName;

        genderText.text =
            data.gender.ToString();

        stageText.text = 
            growth.CurrentStage == GrowthStage.Baby ? "유아" : "성체";

        descriptionText.text =
            data.description;
    }

    public void Close()
    {
        root.SetActive(false);
    }

    public void Apply()
    {
        if (currentRuntime == null)
            return;

        currentRuntime.SetScript(
            scriptInputField.text);
    }
}