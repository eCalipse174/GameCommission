using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailUI : MonoBehaviour
{
    public static CharacterDetailUI Instance;

    [SerializeField] private GameObject root;
    [Space]
    [SerializeField] private Text nameText;
    [SerializeField] private Text genderText;
    [SerializeField] private Text stageText;
    [SerializeField] private Text descriptionText;

    // ── 추가된 부분 ───────────────────────────────────────────────
    [Space]
    [SerializeField] private Button editDialogueButton; // "대사 편집" 버튼
    // ─────────────────────────────────────────────────────────────

    private CharacterDialogueRuntime currentRuntime;

    private void Awake()
    {
        Instance = this;
        root.SetActive(false);

        // ── 추가된 부분 ───────────────────────────────────────────
        if (editDialogueButton != null)
            editDialogueButton.onClick.AddListener(OpenDialogueEditor);
        // ─────────────────────────────────────────────────────────
    }

    public void Open(CharacterDialogueRuntime runtime)
    {
        currentRuntime = runtime;
        root.SetActive(true);

        CharacterController controller = runtime.GetComponent<CharacterController>();
        CharacterGrowth growth = runtime.GetComponent<CharacterGrowth>();
        CharacterData data = controller.Data;

        nameText.text = data.characterName;
        genderText.text = data.gender.ToString();
        stageText.text = growth.CurrentStage == GrowthStage.Baby ? "유아" : "성체";
        descriptionText.text = data.description;
    }

    public void Close()
    {
        root.SetActive(false);
    }

    // ── 추가된 부분 ───────────────────────────────────────────────
    /// <summary>
    /// "대사 편집" 버튼 클릭 시 호출.
    /// 현재 열려있는 캐릭터의 runtime을 DialogueEditorUI에 넘기고 엽니다.
    /// </summary>
    private void OpenDialogueEditor()
    {
        if (currentRuntime == null) return;
        DialogueEditorUI.Instance.Open(currentRuntime);
    }
    // ─────────────────────────────────────────────────────────────
}