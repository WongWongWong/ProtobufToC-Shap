using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProtobufPacking
{
    public class ProtocolHelper
    {
        public static string CmdClass;
        public static string CmdField;

        public static string FacadeClass;
        public static string FacadeRegister;
        public static string FacadeReq;
        public static string FacadeReqAssignment;
        public static string FacadeReqAssignmentList;

        public static string FacadeResp;

        public static string FuncNotesTitle;
        public static string FuncNotesParam;

        /// <summary>
        /// 加载单个模块
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ModuleVo LoadModule(string path)
        {
            string text = ProtobufFileManager.GetTextStream(path);
            ModuleVo ret = LitJson.JsonMapper.ToObject<ModuleVo>(text);
            ret.name = Processing(ret.name);
            return ret;
        }

        /// <summary>
        /// 加载所有模块
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<ModuleVo> LoadAllModule(string path)
        {
            List<ModuleVo> ret = new List<ModuleVo>();
            var locations = ProtobufFileManager.GetFilePathList(path);
            foreach (string location in locations)
            {
                var vo = LoadModule(location);
                ret.Add(vo);
            }
            return ret;
        }

        /// <summary>
        /// 生成cmd字符串
        /// </summary>
        /// <param name="moduleVo"></param>
        /// <returns></returns>
        public static string GenerateCmdString(ModuleVo moduleVo)
        {
            string classStr = CmdClass;
            classStr = classStr.Replace("{comment}", moduleVo.comment);
            classStr = classStr.Replace("{name}", moduleVo.name);

            string fieldsStr = "";
            for (int i = 0; i < moduleVo.protocols.Length; i++)
            {
                ProtocolVo protocolVo = moduleVo.protocols[i];

                if (protocolVo.clientTypes == null || protocolVo.clientTypes.Length == 0)
                    continue; // 非协议数据, 不需要生成

                string fieldStr = CmdField.Replace("{comment}", protocolVo.comment);
                fieldStr = fieldStr.Replace("{name}", protocolVo.name);
                fieldStr = fieldStr.Replace("{protocolId}", protocolVo.protocolId.ToString());

                fieldsStr += fieldStr;
            }

            if (fieldsStr.Length > 0)
            {
                classStr = classStr.Replace("{fields}", fieldsStr);
                return classStr;
            }

            return null;
        }

        /// <summary>
        /// 生成收发封装类 字符串
        /// </summary>
        /// <param name="moduleVo"></param>
        /// <returns></returns>
        public static string GenerateFacadeString(ModuleVo moduleVo)
        {
            string classStr = FacadeClass;
            classStr = classStr.Replace("{comment}", moduleVo.comment);
            classStr = classStr.Replace("{name}", moduleVo.name);
            classStr = classStr.Replace("{name}", moduleVo.name);

            string registersStr = "";
            string functionStr = "";
            for (int i = 0; i < moduleVo.protocols.Length; i++)
            {
                ProtocolVo protocolVo = moduleVo.protocols[i];

                if (protocolVo.clientTypes == null || protocolVo.clientTypes.Length == 0)
                    continue; // 非协议数据, 不需要生成

                if (protocolVo.clientTypes[0] == 1) // 请求 接口
                {
                    // 生成注释
                    string funcNotes = FuncNotesTitle.Replace("{comment}", protocolVo.comment);
                    string param = ""; // 记录请求参数
                    string assugbnebt = "";// 记录赋值参数
                    for (int k = 0; k < protocolVo.fields.Length; k++)
                    {
                        var field = protocolVo.fields[k];
                        string fieldName = Processing(field.name);// 参数名字
                        #region 生成参数说明注释
                        {
                            funcNotes += "\n";
                            funcNotes += FuncNotesParam.Replace("{name}", fieldName).Replace("{comment}", field.comment);
                        }
                        #endregion

                        #region 生成传参
                        {
                            string typeStr;
                            switch (field.type)
                            {
                                case "string":
                                    typeStr = "string";
                                    break;
                                case "int32":
                                    typeStr = "int";
                                    break;
                                case "int64":
                                    typeStr = "long";
                                    break;
                                case "double":
                                    typeStr = "double";
                                    break;
                                default:
                                    typeStr = field.type;
                                    break;
                            }

                            // 是否数组
                            if (field.repeated)
                                typeStr = $"List<{typeStr}>";

                            // 是否非第一个参数, 加逗号分割
                            if (param.Length > 0)
                                param += ", ";

                            // 拼接参数字段
                            param += $"{typeStr} {fieldName}";
                        }
                        #endregion

                        #region 生成赋值
                        {
                            if (field.repeated)
                                assugbnebt += FacadeReqAssignmentList.Replace("{name}", fieldName).Replace("{name}", fieldName);
                            else
                                assugbnebt += FacadeReqAssignment.Replace("{name}", fieldName).Replace("{name}", fieldName);
                        }
                        #endregion
                    }

                    // 生成函数字符串
                    string reqFunctionStr = FacadeReq.Replace("{funcNotes}", funcNotes);
                    reqFunctionStr = reqFunctionStr.Replace("{name}", protocolVo.name).Replace("{name}", protocolVo.name).Replace("{name}", protocolVo.name);
                    reqFunctionStr = reqFunctionStr.Replace("{param}", param);
                    reqFunctionStr = reqFunctionStr.Replace("{assugbnebt}", assugbnebt);
                    reqFunctionStr = reqFunctionStr.Replace("{modulename}", moduleVo.name);

                    functionStr += reqFunctionStr;
                }
                else if (protocolVo.clientTypes[0] == 0) // 返回 接口
                {
                    // 生成注册返回函数字符串
                    string registerStr = FacadeRegister.Replace("{modulename}", moduleVo.name);
                    registerStr = registerStr.Replace("{name}", protocolVo.name);
                    registerStr = registerStr.Replace("{name}", protocolVo.name);
                    registersStr += registerStr;

                    // 生成返回函数字符串
                    string respFunctionStr = FacadeResp.Replace("{comment}", protocolVo.comment);
                    respFunctionStr = respFunctionStr.Replace("{name}", protocolVo.name).Replace("{name}", protocolVo.name);

                    functionStr += respFunctionStr;
                }
            }

            if (registersStr.Length > 0 || functionStr.Length > 0)
            {
                classStr = classStr.Replace("{register}", registersStr);
                classStr = classStr.Replace("{function}", functionStr);
                return classStr;
            }

            return null;
        }

        /// <summary>
        /// cmd输出
        /// </summary>
        /// <param name="protoJsonPath"></param>
        /// <param name="outputPath"></param>
        public static void CmdOutput(string protoJsonPath, string outputPath)
        {
            // 补全斜杠
            if (!outputPath.EndsWith("/"))
                outputPath += "/";

            // 检测创建路径
            ProtobufFileManager.CreateDirPath(outputPath);

            // 生成文件
            var modules = LoadAllModule(protoJsonPath);
            foreach (var module in modules)
            {
                string cmdString = GenerateCmdString(module);
                if (string.IsNullOrEmpty(cmdString))
                    continue;

                ProtobufFileManager.ExportFile(cmdString, $"{outputPath}Cmd{module.name}.cs");
                Debug.Log($"{module.name} Cmd生成成功");
            }

            // 打开文件夹
            Application.OpenURL(outputPath);
        }


        public static void FacadeOutput(string protoJsonPath, string outputPath)
        {
            // 补全斜杠
            if (!outputPath.EndsWith("/"))
                outputPath += "/";

            // 检测创建路径
            ProtobufFileManager.CreateDirPath(outputPath);

            // 生成文件
            var modules = LoadAllModule(protoJsonPath);
            foreach (var module in modules)
            {
                string cmdString = GenerateFacadeString(module);
                if (string.IsNullOrEmpty(cmdString))
                    continue;

                ProtobufFileManager.ExportFile(cmdString, $"{outputPath}Facade{module.name}.cs");
                Debug.Log($"{module.name} Facade生成成功");
            }

            // 打开文件夹
            Application.OpenURL(outputPath);
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string Processing(string str)//处理这段英文的方法
        {
            string[] strArray = str.Split("_".ToCharArray());
            string result = string.Empty;//定义一个空字符串

            foreach (string s in strArray)//循环处理数组里面每一个字符串
            {
                //result += System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s) + " ";
                result += s.Substring(0, 1).ToUpper() + s.Substring(1);
                //.Substring(0, 1).ToUpper()把循环到的字符串第一个字母截取并转换为大写，并用s.Substring(1)得到循环到的字符串除第一个字符后的所有字符拼装到首字母后面。
            }
            return result;
        }
    }
}