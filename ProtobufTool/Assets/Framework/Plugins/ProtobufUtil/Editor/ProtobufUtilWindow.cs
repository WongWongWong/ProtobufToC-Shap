using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProtobufPacking.Editor
{
    public class ProtobufUtilWindow : EditorWindow
    {
        string _protoJsonPath;
        string _cmdPath;
        string _facadePath;

        static Type[] DockedWindowTypes =
        {
            typeof(ProtobufUtilWindow),
        };

        static Vector2 _windowSize = new Vector2(600, 200);

        [MenuItem("GameUtils/Protobuf Builder", false, 102)]
        public static void OpenWindow()
        {
            ProtobufUtilWindow window = GetWindow<ProtobufUtilWindow>("配置构建工具", true, DockedWindowTypes);
            window.minSize = window.maxSize = _windowSize;
        }

        private void Awake()
        {
            _protoJsonPath = PlayerPrefs.GetString(nameof(_protoJsonPath), "D:/Git/proto/trunk/protobuf/module");
            _cmdPath = PlayerPrefs.GetString(nameof(_cmdPath), Application.dataPath + "/../Temp/Cmd");
            _facadePath = PlayerPrefs.GetString(nameof(_facadePath), Application.dataPath + "/../Temp/Facade");

            InitTemplate(ref ProtocolHelper.CmdClass, "Assets/Framework/Plugins/ProtobufUtil/Template/CmdClass.txt");
            InitTemplate(ref ProtocolHelper.CmdField, "Assets/Framework/Plugins/ProtobufUtil/Template/CmdField.txt");
            InitTemplate(ref ProtocolHelper.FacadeClass, "Assets/Framework/Plugins/ProtobufUtil/Template/FacadeClass.txt");
            InitTemplate(ref ProtocolHelper.FacadeRegister, "Assets/Framework/Plugins/ProtobufUtil/Template/FacadeRegister.txt");
            InitTemplate(ref ProtocolHelper.FacadeReq, "Assets/Framework/Plugins/ProtobufUtil/Template/FacadeReq.txt");
            InitTemplate(ref ProtocolHelper.FacadeReqAssignment, "Assets/Framework/Plugins/ProtobufUtil/Template/FacadeReqAssignment.txt");
            InitTemplate(ref ProtocolHelper.FacadeReqAssignmentList, "Assets/Framework/Plugins/ProtobufUtil/Template/FacadeReqAssignmentList.txt");
            InitTemplate(ref ProtocolHelper.FacadeResp, "Assets/Framework/Plugins/ProtobufUtil/Template/FacadeResp.txt");
            InitTemplate(ref ProtocolHelper.FuncNotesTitle, "Assets/Framework/Plugins/ProtobufUtil/Template/FuncNotesTitle.txt");
            InitTemplate(ref ProtocolHelper.FuncNotesParam, "Assets/Framework/Plugins/ProtobufUtil/Template/FuncNotesParam.txt");
        }

        private void OnGUI()
        {
            DrawInput("协议json路径:", ref _protoJsonPath, 30);
            DrawInput("Cmd输出路径:", ref _cmdPath, 60);
            DrawInput("Facade输出路径:", ref _facadePath, 90);

            var btnRect = new Rect(50, 140, 100, 30);

            if (GUI.Button(btnRect, "保存"))
            {
                PlayerPrefs.SetString(nameof(_protoJsonPath), _protoJsonPath);
                PlayerPrefs.SetString(nameof(_cmdPath), _cmdPath);
                PlayerPrefs.SetString(nameof(_facadePath), _facadePath);

                Debug.Log("ProtobufUtils 保存成功");
            }

            if (GUI.Button(new Rect(btnRect.x + (btnRect.x + btnRect.width) * 1, 140, btnRect.width, btnRect.height), "导出Cmd"))
            {
                if (string.IsNullOrEmpty(_protoJsonPath))
                {
                    Debug.LogError("协议json路径不能为空!");
                }
                else if (string.IsNullOrEmpty(_cmdPath))
                {
                    Debug.LogError("Cmd输出路径不能为空!");
                }
                else
                {
                    ProtocolHelper.CmdOutput(_protoJsonPath, _cmdPath);
                }
            }

            if (GUI.Button(new Rect(btnRect.x + (btnRect.x + btnRect.width) * 2, 140, btnRect.width, btnRect.height), "导出Facade"))
            {
                if (string.IsNullOrEmpty(_protoJsonPath))
                {
                    Debug.LogError("协议json路径不能为空!");
                }
                else if (string.IsNullOrEmpty(_facadePath))
                {
                    Debug.LogError("Facade输出路径不能为空!");
                }
                else
                {
                    ProtocolHelper.FacadeOutput(_protoJsonPath, _facadePath);
                }
            }
        }

        void DrawInput(string desc, ref string inputText, float y)
        {
            Rect labelRect = new Rect(10, y, 100, 20);
            GUI.Label(labelRect, desc);

            Rect fieldRect = new Rect(labelRect.x + labelRect.width, y, 480, 20);
            inputText = GUI.TextField(fieldRect, inputText);
        }


        static void InitTemplate(ref string temp, string path)
        {
            TextAsset textAssets = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            if (textAssets)
                temp = textAssets.text;
            else
                Debug.LogError($"代码生成模板读取失败:{path}");
        }
    }
}