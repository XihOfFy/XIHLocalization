using System;
using System.IO;
using System.Text;
using UnityEngine;
//在此仓库（XIHLocalization）此脚本无引用，可以删除
//账号独立本地存储系统，如果公共数据直接PlayerPrefs存取即可，不要调用该类
public static class LocalSaveUtil
{
    public static string UNIN_ID =>"GenYourSelf";//账号独立标识
    static string ACID
    {
        get
        {
            string filePath = UNIN_ID;
            if (string.IsNullOrEmpty(filePath)) filePath = "def";
            filePath = Path.Combine(Application.persistentDataPath, filePath);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            return filePath;
        }
    }
    //防止本地数据账号迁移，复制其他账号数据；防止同账号数据互相穿透
    static int GetOID(string key)
    {
        int sum = 0;
        foreach (char c in key)
        {
            sum += 1 << c;
        }
        string oid = UNIN_ID;
        if (string.IsNullOrEmpty(oid)) return sum;
        foreach (char c in oid)
        {
            sum += 1 << c;
        }
        return sum;
    }
    public static void NormalSaveData(string key, string value)
    {
        string file = Path.Combine(ACID, key);
        if (!File.Exists(file))
        {
            using (File.Create(file))
            {
            }
        }
        File.WriteAllText(file, $"{DateTime.Now.ToFileTime()}|{value}", Encoding.UTF8);
        //PlayerPrefs.SetString(key, value);
        //PlayerPrefs.Save();
    }
    public static string NormalGetData(string key, string defS)
    {
        string file = Path.Combine(ACID, key);
        if (!File.Exists(file)) return defS;
        DateTime wt = File.GetLastWriteTime(file);
        string content = File.ReadAllText(file, Encoding.UTF8);
        string[] cs = content.Split('|');
        if (cs.Length < 2) return defS;
        long.TryParse(cs[0], out long stime);
        DateTime st = DateTime.FromFileTime(stime);
        double spt = (wt - st).TotalSeconds;
        if (spt > 2 || spt < -2)
            return defS;
        return content.Remove(0, cs[0].Length + 1);
    }
    public static void SaveData(string key, string value)
    {
        NormalSaveData(key, EnCodeData(key, value));
    }
    public static string GetData(string key, string defS)
    {
        return DeCodeData(key, NormalGetData(key, defS), defS);
    }
    private static string EnCodeData(string key, string value)
    {
        int size = value.Length;
        int sum = 0;
        foreach (char c in value)
        {
            sum += 1 << c;
        }
        size = sum - size + GetOID(key);
        value = $"{size}|{sum}|{value}";
        return value;
    }
    private static string DeCodeData(string key, string value, string defS)
    {
        string[] ssv = value.Split('|');
        if (ssv.Length < 3) return defS;
        int.TryParse(ssv[0], out int oriSize);
        int.TryParse(ssv[1], out int oriSum);
        string res = value.Remove(0, ssv[0].Length + ssv[1].Length + 2);
        int size = res.Length;
        int sum = 0;
        foreach (char c in res)
        {
            sum += 1 << c;
        }
        size = sum - size + GetOID(key);
        if (size == oriSize && oriSum == sum)
        {
            return res;
        }
        return defS;
    }
}