using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 대사 편집 UI (드롭다운 방식)
/// 탭으로 Stage 선택, 드롭다운으로 DialogueType 선택
/// 선택한 조합의 대사만 목록에 표시
/// </summary>
public class DialogueEditorUI : MonoBehaviour
{
    public static DialogueEditorUI Instance;

    [Header("탭 버튼")]
    [SerializeField] private Button tabBabyButton;
    [SerializeField] private Button tabAdultButton;
    [SerializeField] private Text tabBabyText;
    [SerializeField] private Text tabAdultText;

    [Header("종류 드롭다운")]
    [SerializeField] private Dropdown typeDropdown;

    [Header("대사 목록이 들어갈 ScrollView의 Content")]
    [SerializeField] private Transform entryListParent;

    [Header("프리팹")]
    [SerializeField] private GameObject entryRowPrefab; // InputField + DeleteButton

    [Header("하단 버튼")]
    [SerializeField] private Button addButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;

    [Header("탭 색상")]
    [SerializeField] private Color activeTabColor = Color.white;
    [SerializeField] private Color inactiveTabColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    // ── 내부 데이터 ──────────────────────────────────────────────
    private CharacterDialogueRuntime dialogueRuntime;

    // stage → type → 대사 목록
    private Dictionary<GrowthStage, Dictionary<DialogueType, List<string>>> editorData;

    private GrowthStage currentStage = GrowthStage.Baby;
    private DialogueType currentType = DialogueType.Idle;

    // ── 레이블 ───────────────────────────────────────────────────
    private static readonly Dictionary<DialogueType, string> TypeLabel = new Dictionary<DialogueType, string>
    {
        { DialogueType.Idle,             "평소 대사" },
        { DialogueType.Hunger,           "배고플 때" },
        { DialogueType.Sleepy,           "졸릴 때" },
        { DialogueType.Happy,            "기쁠 때" },
        { DialogueType.Angry,            "화났을 때" },
        { DialogueType.InteractionStart, "상호작용 시작" },
        { DialogueType.InteractionReply, "상호작용 답변" },
    };

    private static readonly DialogueType[] TypeOrder = new DialogueType[]
    {
        DialogueType.Idle,
        DialogueType.Hunger,
        DialogueType.Sleepy,
        DialogueType.Happy,
        DialogueType.Angry,
        DialogueType.InteractionStart,
        DialogueType.InteractionReply,
    };

    // ─────────────────────────────────────────────────────────────

    private void Awake()
    {
        Instance = this;

        tabBabyButton.onClick.AddListener(() => SwitchTab(GrowthStage.Baby));
        tabAdultButton.onClick.AddListener(() => SwitchTab(GrowthStage.Adult));
        addButton.onClick.AddListener(OnAddEntry);
        saveButton.onClick.AddListener(OnSave);
        closeButton.onClick.AddListener(OnClose);

        // 드롭다운 옵션 초기화
        typeDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (DialogueType t in TypeOrder)
            options.Add(TypeLabel[t]);
        typeDropdown.AddOptions(options);
        typeDropdown.onValueChanged.AddListener(OnDropdownChanged);

        gameObject.SetActive(false);
    }

    /// <summary>CharacterDetailUI에서 호출 ? 캐릭터 runtime 주입 후 열기</summary>
    public void Open(CharacterDialogueRuntime runtime)
    {
        dialogueRuntime = runtime;
        LoadFromRuntime();

        currentStage = GrowthStage.Baby;
        currentType = TypeOrder[0];
        typeDropdown.SetValueWithoutNotify(0);

        RefreshTabUI();
        RefreshEntryList();

        gameObject.SetActive(true);
    }

    // ── 1. Runtime → editorData 로드 ─────────────────────────────
    private void LoadFromRuntime()
    {
        editorData = new Dictionary<GrowthStage, Dictionary<DialogueType, List<string>>>();

        foreach (GrowthStage stage in System.Enum.GetValues(typeof(GrowthStage)))
        {
            editorData[stage] = new Dictionary<DialogueType, List<string>>();
            foreach (DialogueType type in TypeOrder)
                editorData[stage][type] = new List<string>();
        }

        //string script = dialogueRuntime.GetScript();
        //if (string.IsNullOrEmpty(script)) return;

        //var parsed = DialogueParser.Parse(script);
        //foreach (var kv in parsed)
        //{
        //    DialogueType type = kv.Key;
        //    foreach (DialogueEntry entry in kv.Value)
        //    {
        //        if (editorData.ContainsKey(entry.stage) &&
        //            editorData[entry.stage].ContainsKey(type))
        //        {
        //            editorData[entry.stage][type].Add(entry.text);
        //        }
        //    }
        //}
    }

    // ── 2. 탭 전환 ───────────────────────────────────────────────
    private void SwitchTab(GrowthStage stage)
    {
        // 현재 목록 저장 후 탭 전환
        SaveCurrentList();
        currentStage = stage;
        RefreshTabUI();
        RefreshEntryList();
    }

    private void RefreshTabUI()
    {
        if (tabBabyText != null) tabBabyText.color = currentStage == GrowthStage.Baby ? activeTabColor : inactiveTabColor;
        if (tabAdultText != null) tabAdultText.color = currentStage == GrowthStage.Adult ? activeTabColor : inactiveTabColor;
    }

    // ── 3. 드롭다운 변경 ─────────────────────────────────────────
    private void OnDropdownChanged(int index)
    {
        SaveCurrentList();
        currentType = TypeOrder[index];
        RefreshEntryList();
    }

    // ── 4. 대사 목록 갱신 ────────────────────────────────────────
    private void RefreshEntryList()
    {
        // 기존 행 전부 제거
        for (int i = entryListParent.childCount - 1; i >= 0; i--)
            Destroy(entryListParent.GetChild(i).gameObject);

        // 현재 stage+type의 대사 목록 표시
        List<string> lines = editorData[currentStage][currentType];
        for (int i = 0; i < lines.Count; i++)
            CreateEntryRow(lines[i]);
    }

    private void CreateEntryRow(string initialText)
    {
        GameObject rowGO = Instantiate(entryRowPrefab, entryListParent);

        InputField inputField = rowGO.transform.GetChild(0).GetComponent<InputField>();
        Button deleteBtn = rowGO.transform.GetChild(1).GetComponent<Button>();

        if (inputField != null)
            inputField.text = initialText;

        if (deleteBtn != null)
        {
            GameObject capturedRow = rowGO;
            deleteBtn.onClick.AddListener(() =>
            {
                Destroy(capturedRow);
            });
        }
    }

    // ── 5. 현재 목록 → editorData 저장 ───────────────────────────
    private void SaveCurrentList()
    {
        List<string> lines = new List<string>();
        foreach (Transform child in entryListParent)
        {
            InputField inputField = child.GetChild(0).GetComponent<InputField>();
            if (inputField != null)
            {
                string trimmed = inputField.text.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    lines.Add(trimmed);
            }
        }
        editorData[currentStage][currentType] = lines;
    }

    // ── 6. 대사 추가 버튼 ────────────────────────────────────────
    private void OnAddEntry()
    {
        CreateEntryRow(string.Empty);
    }

    // ── 7. 저장 ──────────────────────────────────────────────────
    private void OnSave()
    {
        // 현재 보고 있는 목록 먼저 저장
        SaveCurrentList();

        StringBuilder sb = new StringBuilder();

        foreach (GrowthStage stage in System.Enum.GetValues(typeof(GrowthStage)))
        {
            foreach (DialogueType type in TypeOrder)
            {
                List<string> lines = editorData[stage][type];
                if (lines.Count == 0) continue;

                sb.AppendLine($"[{type}:{stage}]");
                foreach (string line in lines)
                    sb.AppendLine(line);
                sb.AppendLine();
            }
        }

        //dialogueRuntime.SetScript(sb.ToString());
        Debug.Log($"[DialogueEditorUI] {dialogueRuntime.gameObject.name} 대사 저장 완료!");
    }

    // ── 8. 닫기 ──────────────────────────────────────────────────
    private void OnClose()
    {
        gameObject.SetActive(false);
        dialogueRuntime = null;
    }
}