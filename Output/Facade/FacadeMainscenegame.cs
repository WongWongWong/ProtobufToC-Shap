using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主场景比赛
/// </summary>
public class FacadeMainscenegame : Single<FacadeMainscenegame>
{
    /// <summary>
    /// 注册回调函数
    /// </summary>
    public void RegisterResp()
    {
        WebSocketSharp.Instance.RegistCallback(CmdMainscenegame.RespMainSceneGameSimpleInfo, OnRespMainSceneGameSimpleInfo);
        WebSocketSharp.Instance.RegistCallback(CmdMainscenegame.RespMainSceneFootballInfo, OnRespMainSceneFootballInfo);
        WebSocketSharp.Instance.RegistCallback(CmdMainscenegame.RespMainSceneFootballPlayerInfo, OnRespMainSceneFootballPlayerInfo);
        WebSocketSharp.Instance.RegistCallback(CmdMainscenegame.RespMainSceneFootballPlayerGold, OnRespMainSceneFootballPlayerGold);

    }

    /// <summary>
    /// 请求主场景游戏简易信息
    /// </summary>
    /// <param name="GameTypeList">游戏类型List(空列表表示全发)</param>
    public void ReqMainSceneGameSimpleInfo(List<int> GameTypeList, object clientData = null)
    {
        var c2s = new ReqMainSceneGameSimpleInfo();
        c2s.GameTypeList.AddRange(GameTypeList);

        WebSocketSharp.Instance.SendDataAsycn(CmdMainscenegame.ReqMainSceneGameSimpleInfo, c2s, clientData);

        ProtobufHelper.Log(c2s);
    }

    /// <summary>
    /// 返回主场景游戏简易信息
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespMainSceneGameSimpleInfo(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespMainSceneGameSimpleInfo>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回主场景足球信息
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespMainSceneFootballInfo(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespMainSceneFootballInfo>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回主场景足球玩家信息更新
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespMainSceneFootballPlayerInfo(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespMainSceneFootballPlayerInfo>(proto);
        ProtobufHelper.Log(s2c);
    }

    /// <summary>
    /// 返回足球金币
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="clientData"></param>
    void OnRespMainSceneFootballPlayerGold(byte[] proto, object clientData)
    {
        var s2c = ProtobufHelper.DeSerialize<RespMainSceneFootballPlayerGold>(proto);
        ProtobufHelper.Log(s2c);
    }


}