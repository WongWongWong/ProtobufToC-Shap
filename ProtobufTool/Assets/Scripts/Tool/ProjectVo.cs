using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectVo
{
    public string name;
    public string protoPath;
    public string cmdPath;
    public string facadePath;
}

public class ProjectCache
{
    public string curProjectName = "";

    public List<ProjectVo> allProject = new List<ProjectVo>();

    /// <summary>
    /// 当前项目数据
    /// </summary>
    public ProjectVo CurProject
    {
        get
        {
            ProjectVo ret = allProject.Find(vo => vo.name == curProjectName);
            return ret;
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
        string json = LitJson.JsonMapper.ToJson(this);
        PlayerPrefs.SetString("PROJECT_CACHE", json);
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <returns></returns>
    public static ProjectCache GetCache()
    {
        ProjectCache cache;
        string json = PlayerPrefs.GetString("PROJECT_CACHE");
        if (string.IsNullOrEmpty(json))
        {
            cache = new ProjectCache();
        }
        else
        {
            cache = LitJson.JsonMapper.ToObject<ProjectCache>(json);
        }

        return cache;
    }
}