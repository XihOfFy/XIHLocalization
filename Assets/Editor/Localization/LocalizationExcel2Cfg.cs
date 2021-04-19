using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace XIHLocalization
{
    public class LocalizationExcel2Cfg
    {
        const string excelPath = "../doc/Localization.xlsx";
        const string classPath = "Assets/Scripts/Localization/LocalizationConsts.cs";
        [MenuItem("XIHUtil/LocalizationExcel2Cfg")]
        public static void Excel2Cfg()
        {
            string path = Path.Combine("Assets/", excelPath);
            Debug.Log($"要求:Excel必须在{path}，\r\n包含localization表，\r\n单元格必须全是string类型，\r\n第一列首单元格必须是\"key\"列,其他列首单元格必须是和{nameof(XIHLanguage)}枚举内容一致(none除外),字母大小需一致，\r\n \"key\"列不许出现空，否则表示终止");
            if (!File.Exists(path))
            {
                Debug.LogError($"未找到{path}文件");
                return;
            }
            XSSFWorkbook xssfWorkbook;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                xssfWorkbook = new XSSFWorkbook(file);
            }
            ISheet sheet = xssfWorkbook.GetSheet("localization");
            if (sheet == null)
            {
                Debug.LogError($"未找到Excel文件{excelPath}的 localization 表");
                return;
            }
            IRow firtRow = sheet.GetRow(0);
            if (firtRow.GetCell(0).StringCellValue != "key")
            {
                Debug.LogError($"文件{excelPath}的第一个单元格不是\"key\"作为表头");
                return;
            }
            int size = firtRow.Cells.Count;
            HashSet<string> lgs = new HashSet<string>();
            foreach (var enu in Enum.GetNames(typeof(XIHLanguage)))
            {
                lgs.Add(enu);
            }
            Dictionary<string, int> lgsDics = new Dictionary<string, int>();
            while (--size > 0)
            {
                string val = firtRow.GetCell(size).StringCellValue;
                if (string.IsNullOrEmpty(val) || !lgs.Contains(val)) continue;
                if (lgsDics.ContainsKey(val))
                {
                    Debug.LogWarning($"包含重复列表 {val} 将替换为 {size} 列对应翻译");
                    continue;
                }
                lgsDics[val] = size;
            }
            HashSet<string> keys = null;
            foreach (var kv in lgsDics)
            {
                Enum.TryParse(kv.Key, out XIHLanguage language);
                switch (language)
                {
                    case XIHLanguage.cn:
                    case XIHLanguage.zh_TW:
                    case XIHLanguage.en:
                        keys = GetCfg(language, sheet, kv.Value);
                        break;
                    default:
                        break;
                }
            }
            GenerClass(keys);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("操作完成");
        }
        static HashSet<string> GetCfg(XIHLanguage language, ISheet sheet, int col)
        {
            LanguageCfg cfg = AssetDatabase.LoadAssetAtPath<LanguageCfg>($"Assets/Resources/Config/Localization/{language}");
            if (cfg == null)
            {
                cfg = ScriptableObject.CreateInstance<LanguageCfg>();
                AssetDatabase.CreateAsset(cfg, $"Assets/Resources/Config/Localization/{language}.asset");
            }
            cfg.keyWords = new List<KeyWord>();
            int rowNum = 1;
            IRow row = sheet.GetRow(rowNum);
            string key = row?.GetCell(0)?.StringCellValue;
            HashSet<string> keys = new HashSet<string>();
            while (!string.IsNullOrEmpty(key))
            {
                key = key.ToLower();
                if (keys.Contains(key))
                {
                    Debug.LogWarning($"包含重复Key:{key},在表中位置【{rowNum + 1}】行");
                }
                else
                {
                    string word = row.GetCell(col)?.StringCellValue;
                    if (string.IsNullOrEmpty(word))
                    {
                        Debug.LogWarning($"Key:{key}所对应word为空,在表中位置【{rowNum + 1}】行，【{col + 1}】列");
                    }
                    else
                    {
                        cfg.keyWords.Add(new KeyWord() { key = key, word = word });
                    }
                }
                keys.Add(key);
                row = sheet.GetRow(++rowNum);
                key = row?.GetCell(0)?.StringCellValue;
            }
            EditorUtility.SetDirty(cfg);
            AssetDatabase.SaveAssets();
            return keys;
        }
        static void GenerClass(HashSet<string> keys)
        {
            if (keys == null) return;
            if (!File.Exists(classPath))
            {
                using (File.Create(classPath)) { }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("namespace XIHLocalization\r\n");
            sb.Append("{\r\n");
            sb.Append("    public class LocalizationConsts\r\n");
            sb.Append("    {\r\n");
            foreach (var key in keys)
            {
                sb.Append($"        public static string {key.ToUpper()} =>  LocalizationUtil.TranslateText(\"{key}\");\r\n");
            }
            sb.Append("    }\r\n");
            sb.Append("}\r\n");
            Debug.Log("生成 LocalizationConsts.cs 成功");
            File.WriteAllText(classPath, sb.ToString());
        }
    }
}

