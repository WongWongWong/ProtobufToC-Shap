/// <summary>
/// 通用
/// </summary>
public class CmdCommon
{
	/// <summary>
    /// 请求心跳
    /// </summary>
    public const int ReqHeartbeat = 1001;
	
	/// <summary>
    /// 请求重连
    /// </summary>
    public const int ReqReconnect = 1005;
	
	/// <summary>
    /// 请求记录奖励
    /// </summary>
    public const int ReqRecordReward = 1007;
	
	/// <summary>
    /// 返回心跳
    /// </summary>
    public const int RespHeartbeat = 1101;
	
	/// <summary>
    /// 返回码
    /// </summary>
    public const int RespCode = 1102;
	
	/// <summary>
    /// 返回断线
    /// </summary>
    public const int RespDisconnect = 1103;
	
	/// <summary>
    /// 返回公告信息
    /// </summary>
    public const int RespNoticeMessage = 1104;
	
	/// <summary>
    /// 返回kcp断线
    /// </summary>
    public const int RespKcpDisconnect = 1106;
	
	/// <summary>
    /// 返回记录奖励
    /// </summary>
    public const int RespRecordReward = 1107;
	

}