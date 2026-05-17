using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 인게임 대사 편집 UI
/// Canvas 아래 적당한 GameObject에 붙여서 사용합니다.
/// </summary>
public class DialogueEditorUI : MonoBehaviour
{
    [Header("연결 필수")]
    [SerializeField] private CharacterDialogueRuntime dialogueRuntime;

    [Header("탭 버튼")]
    [SerializeField] private Button tabBabyButton;
    [SerializeField] private Button tabAdultButton;

    [Header("탭 버튼 텍스트 (선택 시 색상 변경용)")]
    [SerializeField] private TextMeshProUGUI tabBabyText;
    [SerializeField] private TextMeshProUGUI tabAdultText;

    [Header("대사 섹션들이 들어갈 스크롤뷰 Content")]
    [SerializeField] private Transform babyContentParent;
    [SerializeField] private Transform adultContentParent;

    [Header("프리팹")]
    [SerializeField] private GameObject sectionPrefab;   // 섹션 제목 + 대사 목록 묶음
    [SerializeField] private GameObject entryRowPrefab;  // 대사 한 줄 (입력란 + 삭제 버튼)

    [Header("하단 버튼")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;

    [Header("탭 색상")]
    [SerializeField] private Color activeTabColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color inactiveTabColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    // ── 내부 데이터 ──────────────────────────────────────────────
    // stage → type → 대사 목록
    private Dictionary<GrowthStage, Dictionary<DialogueType, List<string>>> editorData;

    // 현재 선택된 탭
    private GrowthStage currentStage = GrowthStage.Baby;

    // 섹션 패널 캐싱: stage → type → 대사 행들의 부모 Transform
    private Dictionary<GrowthStage, Dictionary<DialogueType, Transform>> sectionEntryParents;

    // ── 레이블 ────────────────────────────────────────────────────
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

    private static readonly Dictionary<GrowthStage, string> StageLabel = new Dictionary<GrowthStage, string>
    {
        { GrowthStage.Baby,  "아기" },
        { GrowthStage.Adult, "어른" },
    };

    // ─────────────────────────────────────────────────────────────

    private void Awake()
    {
        // 버튼 이벤트
        tabBabyButton.onClick.AddListener(() => SwitchTab(GrowthStage.Baby));
        tabAdultButton.onClick.AddListener(() => SwitchTab(GrowthStage.Adult));
        saveButton.onClick.AddListener(OnSave);
        closeButton.onClick.AddListener(OnClose);
    }

    private void OnEnable()
    {
        LoadFromRuntime();
        BuildAllSections();
        SwitchTab(GrowthStage.Baby);
    }

    // ── 1. Runtime → editorData 로드 ─────────────────────────────
    private void LoadFromRuntime()
    {
        editorData = new Dictionary<GrowthStage, Dictionary<DialogueType, List<string>>>();

        // 빈 구조 먼저 초기화
        foreach (GrowthStage stage in System.Enum.GetValues(typeof(GrowthStage)))
        {
            editorData[stage] = new Dictionary<DialogueType, List<string>>();
            foreach (DialogueType type in System.Enum.GetValues(typeof(DialogueType)))
                editorData[stage][type] = new List<string>();
        }

        // 기존 스크립트 파싱 결과를 editorData에 채우기
        string script = dialogueRuntime.GetScript();
        if (!string.IsNullOrEmpty(script))
        {
            var parsed = DialogueParser.Parse(script);
            foreach (var kv in parsed)
            {
                DialogueType type = kv.Key;
                foreach (DialogueEntry entry in kv.Value)
                {
                    if (editorData.ContainsKey(entry.stage) &&
                        editorData[entry.stage].ContainsKey(type))
                    {
                        editorData[entry.stage][type].Add(entry.text);
                    }
                }
            }
        }
    }

    // ── 2. UI 섹션 전체 생성 ─────────────────────────────────────
    private void BuildAllSections()
    {
        // 기존 UI 정리
        ClearChildren(babyContentParent);
        ClearChildren(adultContentParent);

        sectionEntryParents = new Dictionary<GrowthStage, Dictionary<DialogueType, Transform>>();

        foreach (GrowthStage stage in System.Enum.GetValues(typeof(GrowthStage)))
        {
            sectionEntryParents[stage] = new Dictionary<DialogueType, Transform>();
            Transform contentParent = (stage == GrowthStage.Baby) ? babyContentParent : adultContentParent;

            foreach (DialogueType type in System.Enum.GetValues(typeof(DialogueType)))
            {
                Transform entryParent = BuildSection(contentParent, stage, type);
                sectionEntryParents[stage][type] = entryParent;

                // 기존 대사 행 생성
                foreach (string text in editorData[stage][type])
                    AddEntryRow(entryParent, stage, type, text);
            }
        }
    }

    /// <summary>섹션 하나(제목 + 대사목록 + 추가버튼)를 생성하고, 대사 행들의 부모 Transform을 반환</summary>
    private Transform BuildSection(Transform parent, GrowthStage stage, DialogueType type)
    {
        GameObject sectionGO = Instantiate(sectionPrefab, parent);

        // 섹션 제목 텍스트
        // sectionPrefab 안에 "TitleText" 라는 이름의 TextMeshProUGUI가 있다고 가정
        TextMeshProUGUI titleText = sectionGO.transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
        if (titleText != null)
            titleText.text = TypeLabel[type];

        // 대사 행들이 쌓일 컨테이너
        // sectionPrefab 안에 "EntryContainer" 라는 이름의 Transform이 있다고 가정
        Transform entryContainer = sectionGO.transform.Find("EntryContainer");

        // "대사 추가" 버튼
        // sectionPrefab 안에 "AddButton" 이라는 이름의 Button이 있다고 가정
        Button addButton = sectionGO.transform.Find("AddButton")?.GetComponent<Button>();
        if (addButton != null)
        {
            // 클로저용 변수 캡처
            GrowthStage capturedStage = stage;
            DialogueType capturedType = type;
            Transform capturedParent = entryContainer;

            addButton.onClick.AddListener(() =>
            {
                editorData[capturedStage][capturedType].Add(string.Empty);
                AddEntryRow(capturedParent, capturedStage, capturedType, string.Empty);
            });
        }

        return entryContainer;
    }

    /// <summary>대사 한 줄(입력란 + 삭제 버튼)을 생성</summary>
    private void AddEntryRow(Transform parent, GrowthStage stage, DialogueType type, string initialText)
    {
        GameObject rowGO = Instantiate(entryRowPrefab, parent);

        // entryRowPrefab 안에 "InputField" 라는 TMP_InputField가 있다고 가정
        TMP_InputField inputField = rowGO.transform.Find("InputField")?.GetComponent<TMP_InputField>();
        if (inputField != null)
        {
            inputField.text = initialText;

            // 입력값이 바뀔 때마다 editorData 동기화
            // 행의 인덱스를 활용하기 위해 sibling index 사용
            inputField.onValueChanged.AddListener((val) =>
            {
                int idx = rowGO.transform.GetSiblingIndex();
                if (idx < editorData[stage][type].Count)
                    editorData[stage][type][idx] = val;
            });
        }

        // entryRowPrefab 안에 "DeleteButton" 이라는 Button이 있다고 가정
        Button deleteButton = rowGO.transform.Find("DeleteButton")?.GetComponent<Button>();
        if (deleteButton != null)
        {
            GrowthStage capturedStage = stage;
            DialogueType capturedType = type;
            GameObject capturedRowGO = rowGO;

            deleteButton.onClick.AddListener(() =>
            {
                int idx = capturedRowGO.transform.GetSiblingIndex();
                if (idx < editorData[capturedStage][capturedType].Count)
                    editorData[capturedStage][capturedType].RemoveAt(idx);
                Destroy(capturedRowGO);
            });
        }
    }

    // ── 3. 탭 전환 ───────────────────────────────────────────────
    private void SwitchTab(GrowthStage stage)
    {
        currentStage = stage;

        babyContentParent.gameObject.SetActive(stage == GrowthStage.Baby);
        adultContentParent.gameObject.SetActive(stage == GrowthStage.Adult);

        if (tabBabyText != null) tabBabyText.color = (stage == GrowthStage.Baby) ? activeTabColor : inactiveTabColor;
        if (tabAdultText != null) tabAdultText.color = (stage == GrowthStage.Adult) ? activeTabColor : inactiveTabColor;
    }

    // ── 4. 저장 ──────────────────────────────────────────────────
    private void OnSave()
    {
        StringBuilder sb = new StringBuilder();

        foreach (GrowthStage stage in System.Enum.GetValues(typeof(GrowthStage)))
        {
            foreach (DialogueType type in System.Enum.GetValues(typeof(DialogueType)))
            {
                List<string> lines = editorData[stage][type];
                if (lines.Count == 0) continue;

                // 헤더
                sb.AppendLine($"[{type}:{stage}]");

                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        sb.AppendLine(trimmed);
                }

                sb.AppendLine(); // 섹션 사이 빈 줄
            }
        }

        dialogueRuntime.SetScript(sb.ToString());
        Debug.Log("[DialogueEditorUI] 대사 저장 완료!");
    }

    // ── 5. 닫기 ──────────────────────────────────────────────────
    private void OnClose()
    {
        gameObject.SetActive(false);
    }

    // ── 유틸 ─────────────────────────────────────────────────────
    private static void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
            Destroy(parent.GetChild(i).gameObject);
    }
}
