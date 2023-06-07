using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 属性
/// </summary>
public class FacadeAttr : Single<FacadeAttr>
{
    /// <summary>
    /// 注册回调函数
    /// </summary>
    public void RegisterResp()
    {
        WebSocketSharp.Instance.RegistCallback(CmdAttr.RespPlayerAllAttrVoInfo, OnRespPlayerAllAttrVoInfo);
        WebSocketSharp.Instance.RegistCallback(CmdAttr.RespPlayerAttrVoInfo, OnRespPlayerAttrVoInfo);

    }

    /// <summary>
    /// 请求玩家所有属性信息
    /// </summary>
    public void ReqPlayerAllAttrVoInfo(object clientData = null)
    {
        var c2s = new ReqPlayerAllAttrVoInfo();

        WebSocketSharp.Instance.SendDataAsycn(CmdAttr.ReqPlayerAllAttrVoInfo, c2s, clientData);

        ProtobufHelper.Log(c2s);
    }


    /// <summary>
    /// 请求玩家属性信息
    /// </summary>
    /// <param name="AttrType">属性Vo类型</param>
    public void ReqPlayerAttrVoInfo(string AttrType, object clientData = null)
    {
        var c2s = new ReqPlayerAttrVoInfo();
        c2s.AttrType = AttrType;

        WebSocketSharp.Instance.SendDataAsycn(CmdAttr.ReqPlayerAttrVoInfo, c2s, clientData);

        ProtobufHelper.Log(c2s);
    }

    /// <summary>
    /// 返回玩家所有属性信息
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespPlayerAllAttrVoInfo(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespPlayerAllAttrVoInfo>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回玩家属性信息
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespPlayerAttrVoInfo(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespPlayerAttrVoInfo>(proto);
        ProtobufHelper.Log(s2c);
    }


}