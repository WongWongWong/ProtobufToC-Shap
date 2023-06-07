using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用
/// </summary>
public class FacadeCommon : Single<FacadeCommon>
{
    /// <summary>
    /// 注册回调函数
    /// </summary>
    public void RegisterResp()
    {
        WebSocketSharp.Instance.RegistCallback(CmdCommon.RespHeartbeat, OnRespHeartbeat);
        WebSocketSharp.Instance.RegistCallback(CmdCommon.RespCode, OnRespCode);
        WebSocketSharp.Instance.RegistCallback(CmdCommon.RespDisconnect, OnRespDisconnect);
        WebSocketSharp.Instance.RegistCallback(CmdCommon.RespNoticeMessage, OnRespNoticeMessage);
        WebSocketSharp.Instance.RegistCallback(CmdCommon.RespKcpDisconnect, OnRespKcpDisconnect);
        WebSocketSharp.Instance.RegistCallback(CmdCommon.RespRecordReward, OnRespRecordReward);

    }

    /// <summary>
    /// 请求心跳
    /// </summary>
    public void ReqHeartbeat(object clientData = null)
    {
        var c2s = new ReqHeartbeat();

        WebSocketSharp.Instance.SendDataAsycn(CmdCommon.ReqHeartbeat, c2s, clientData);

        ProtobufHelper.Log(c2s);
    }


    /// <summary>
    /// 请求重连
    /// </summary>
    /// <param name="LoginCode">登录码</param>
    /// <param name="PlayerId">玩家Id</param>
    public void ReqReconnect(string LoginCode, string PlayerId, object clientData = null)
    {
        var c2s = new ReqReconnect();
        c2s.LoginCode = LoginCode;
        c2s.PlayerId = PlayerId;

        WebSocketSharp.Instance.SendDataAsycn(CmdCommon.ReqReconnect, c2s, clientData);

        ProtobufHelper.Log(c2s);
    }


    /// <summary>
    /// 请求记录奖励
    /// </summary>
    /// <param name="RecordId">记录id</param>
    /// <param name="Num">数量</param>
    public void ReqRecordReward(string RecordId, int Num, object clientData = null)
    {
        var c2s = new ReqRecordReward();
        c2s.RecordId = RecordId;
        c2s.Num = Num;

        WebSocketSharp.Instance.SendDataAsycn(CmdCommon.ReqRecordReward, c2s, clientData);

        ProtobufHelper.Log(c2s);
    }

    /// <summary>
    /// 返回心跳
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespHeartbeat(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespHeartbeat>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回码
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespCode(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespCode>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回断线
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespDisconnect(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespDisconnect>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回公告信息
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespNoticeMessage(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespNoticeMessage>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回kcp断线
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespKcpDisconnect(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespKcpDisconnect>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回记录奖励
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespRecordReward(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespRecordReward>(proto);
        ProtobufHelper.Log(s2c);
    }


}