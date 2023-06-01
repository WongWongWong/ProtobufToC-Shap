using ProtobufPacking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITool : MonoBehaviour
{
    [Header("对象")]
    [SerializeField]
    Dropdown ProjectSelected;
    [SerializeField]
    InputField ProjectNameInput;
    [SerializeField]
    InputField ProtoPathInput;
    [SerializeField]
    InputField CmdPathInput;
    [SerializeField]
    InputField FacadePathInput;
    [SerializeField]
    Button SaveButton;
    [SerializeField]
    Button DelButton;
    [SerializeField]
    Button CmdOutputButton;
    [SerializeField]
    Button FacadeOutputButton;
    [SerializeField]
    RectTransform LogLayout;

    [SerializeField]
    Text logTextPrefab;


    [Header("模板")]
    [SerializeField]
    TextAsset CmdClass;
    [SerializeField]
    TextAsset CmdField;
    [SerializeField]
    TextAsset FacadeClass;
    [SerializeField]
    TextAsset FacadeRegister;
    [SerializeField]
    TextAsset FacadeReq;
    [SerializeField]
    TextAsset FacadeReqAssignment;
    [SerializeField]
    TextAsset FacadeReqAssignmentList;
    [SerializeField]
    TextAsset FacadeResp;
    [SerializeField]
    TextAsset FuncNotesTitle;
    [SerializeField]
    TextAsset FuncNotesParam;

    // 项目缓存
    ProjectCache _cache;

    void Awake()
    {
        ProtocolHelper.CmdClass = CmdClass.text;
        ProtocolHelper.CmdField = CmdField.text;
        ProtocolHelper.FacadeClass = FacadeClass.text;
        ProtocolHelper.FacadeRegister = FacadeRegister.text;
        ProtocolHelper.FacadeReq = FacadeReq.text;
        ProtocolHelper.FacadeReqAssignment = FacadeReqAssignment.text;
        ProtocolHelper.FacadeReqAssignmentList = FacadeReqAssignmentList.text;
        ProtocolHelper.FacadeResp = FacadeResp.text;
        ProtocolHelper.FuncNotesTitle = FuncNotesTitle.text;
        ProtocolHelper.FuncNotesParam = FuncNotesParam.text;
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveButton.onClick.AddListener(OnClickSaveBtn);
        //DelButton.onClick.AddListener(OnClickDelBtn);
        CmdOutputButton.onClick.AddListener(OnClickCmdOutputBtn);
        FacadeOutputButton.onClick.AddListener(OnClickFacadeOutputBtn);

        ProjectSelected.onValueChanged.AddListener(OnSelectedChanged);

        // 初始化缓存
        _cache = ProjectCache.GetCache();

        RefreshOptions();
        RefreshInput();

        logTextPrefab.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += OnLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= OnLog;
    }

    // 刷新选择项目
    void RefreshOptions()
    {
        List<string> options = new List<string>();
        ProjectSelected.ClearOptions();
        foreach (var projectVo in _cache.allProject)
        {
            options.Add(projectVo.name);
        }
        ProjectSelected.AddOptions(options);

        int index = options.IndexOf(_cache.curProjectName);
        if (index != -1)
        {
            ProjectSelected.value = index;
        }
        else
        {
            ProjectSelected.value = 0;
        }
    }

    // 刷新显示曾
    void RefreshInput()
    {
        // 刷新显示层
        var curProject = _cache.CurProject;
        if (curProject != null)
        {
            ProjectNameInput.text = curProject.name;
            ProtoPathInput.text = curProject.protoPath;
            CmdPathInput.text = curProject.cmdPath;
            FacadePathInput.text = curProject.facadePath;
        }
        else
        {
            ProjectNameInput.text = "";
            ProtoPathInput.text = "";
            CmdPathInput.text = "";
            FacadePathInput.text = "";
        }
    }

    /// <summary>
    /// 切换
    /// </summary>
    void OnSelectedChanged(int index)
    {
        if (_cache.allProject.Count > index)
        {
            _cache.curProjectName = _cache.allProject[index].name;
            _cache.Save();
            RefreshInput();
        }

    }

    void OnClickSaveBtn()
    {
        if (string.IsNullOrEmpty(ProjectNameInput.text))
        {
            Debug.LogWarning("名字不能为空!");
            return;
        }

        ProjectVo vo = _cache.allProject.Find(vo => vo.name == ProjectNameInput.text);
        if (vo == null)
        {
            vo = new ProjectVo();
            _cache.allProject.Add(vo);
        }

        vo.name = ProjectNameInput.text;
        vo.protoPath = ProtoPathInput.text;
        vo.cmdPath = CmdPathInput.text;
        vo.facadePath = FacadePathInput.text;

        _cache.curProjectName = vo.name;
        RefreshOptions();

        _cache.Save();

        Debug.Log($"保存项目:{_cache.curProjectName}");
    }

    public void OnClickDelBtn()
    {
        if (_cache.allProject.Count == 0)
        {
            Debug.Log("没有可删除的项目!");
            return;
        }

        Debug.Log($"删除项目:{_cache.curProjectName}");
        _cache.allProject.RemoveAll(vo => vo.name == _cache.curProjectName);
        if (_cache.allProject.Count > 0)
        {
            _cache.curProjectName = _cache.allProject[0].name;
        }
        else
        {
            _cache.curProjectName = "";
        }
        _cache.Save();

        RefreshOptions();
        RefreshInput();
    }

    void OnClickCmdOutputBtn()
    {
        string outputPath = CmdPathInput.text;
        if (string.IsNullOrEmpty(outputPath))
        {
            Debug.LogWarning("Cmd路径不能为空!");
            return;
        }

        // 补全斜杠
        if (!outputPath.EndsWith("/"))
            outputPath += "/";

        // 检测创建路径
        FileManager.Ins.CreateDirPath(outputPath);

        // 生成文件
        var modules = ProtocolHelper.LoadAllModule(ProtoPathInput.text);
        foreach (var module in modules)
        {
            string cmdString = ProtocolHelper.GenerateCmdString(module);
            if (string.IsNullOrEmpty(cmdString))
                continue;

            FileManager.Ins.ExportFile(cmdString, $"{outputPath}Cmd{module.name}.cs");
            Debug.Log($"{module.name} Cmd生成成功");
        }

        // 打开文件夹
        Application.OpenURL(outputPath);
    }

    void OnClickFacadeOutputBtn()
    {
        string outputPath = FacadePathInput.text;
        if (string.IsNullOrEmpty(outputPath))
        {
            Debug.LogWarning("Facade路径不能为空!");
            return;
        }

        // 补全斜杠
        if (!outputPath.EndsWith("/"))
            outputPath += "/";

        // 检测创建路径
        FileManager.Ins.CreateDirPath(outputPath);

        // 生成文件
        var modules = ProtocolHelper.LoadAllModule(ProtoPathInput.text);
        foreach (var module in modules)
        {
            string cmdString = ProtocolHelper.GenerateFacadeString(module);
            if (string.IsNullOrEmpty(cmdString))
                continue;

            FileManager.Ins.ExportFile(cmdString, $"{outputPath}Facade{module.name}.cs");
            Debug.Log($"{module.name} Facade生成成功");
        }

        // 打开文件夹
        Application.OpenURL(outputPath);
    }

    void OnLog(string condition, string stackTrace, LogType type)
    {
        string text = $"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}] [{Enum.GetName(typeof(LogType), type)}] {condition}";

        if (type == LogType.Exception || type == LogType.Error)
        {
            text += $"\n{stackTrace}";
        }

        var logText = Instantiate(logTextPrefab, LogLayout);
        logText.text = text;
        logText.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(LogLayout);
    }
}