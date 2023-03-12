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
    public class I18NExcel2CfgEditor:EditorWindow
    {
        const string excelPath = "doc/Localization.xlsx";
        const string classPath = "Assets/Test/I18NKeys.cs";
        public static string AssetEditorPath = $"Assets/ABs/I18N/{I18NUtil.ASSET_NAME}";
        [MenuItem("XIHUtil/I18N")]
        static void I18NWindow() {
            EditorWindow.GetWindow<I18NExcel2CfgEditor>(nameof(I18NExcel2CfgEditor));
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.HelpBox($"本地化工具，将{excelPath}的配置表转换为{AssetEditorPath}配置文件，并生成{classPath}类辅助使用本地化key,若想改变xlsx文件路径或输出c#路径，可修改本脚本{nameof(excelPath)}和{nameof(classPath)}的常量值",MessageType.Warning);
            EditorGUILayout.HelpBox($"要求:Excel必须在{excelPath}，\r\n包含localization表，\r\n单元格必须全是string类型，\r\n第一列首单元格必须是\"key\"列,其他列首单元格必须是和{nameof(XIHLanguage)}枚举内容一致(none除外),字母大小需一致，\r\n \"key\"列不许出现空，否则表示终止", MessageType.Info);
            if (GUILayout.Button($"执行生成{AssetEditorPath}")) {
                Excel2Cfg(EditorUtility.DisplayDialog("确认", $"是否生成{classPath}", "确定", "否"));
            }
            EditorGUILayout.EndVertical();
        }
        public void Excel2Cfg(bool genCs)
        {
            if (!File.Exists(excelPath))
            {
                Debug.LogError($"未找到{excelPath}文件");
                return;
            }
            XSSFWorkbook xssfWorkbook;
            using (FileStream file = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
            var lgsDics = new Dictionary<XIHLanguage, int>();
            while (--size > 0)
            {
                string val = firtRow.GetCell(size).StringCellValue;
                if (string.IsNullOrEmpty(val) || !Enum.TryParse(val, out XIHLanguage language)) {
                    Debug.LogWarning($"不存在此翻译语言，请考虑在{nameof(XIHLanguage)}中定义");
                    continue;
                }
                if (lgsDics.ContainsKey(language))
                {
                    Debug.LogWarning($"包含重复列表 {val} 将替换为 {size+1} 列对应翻译");
                    continue;
                }
                lgsDics[language] = size;
            }

            if (genCs) {
                HashSet<string> keys = null;
                keys = GetCfg(lgsDics, sheet);
                GenerClass(keys);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("操作完成");
        }
        HashSet<string> GetCfg(Dictionary<XIHLanguage, int> lgsDics, ISheet sheet)
        {
            I18NCfg cfg = AssetDatabase.LoadAssetAtPath<I18NCfg>(AssetEditorPath);
            if (cfg == null)
            {
                cfg = ScriptableObject.CreateInstance<I18NCfg>();
                var dir = Path.GetDirectoryName(AssetEditorPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                AssetDatabase.CreateAsset(cfg, AssetEditorPath);
            }
            cfg.keyWords = new List<KeyWords>();
            int rowNum = 1;
            IRow row = sheet.GetRow(rowNum);
            string key = row?.GetCell(0)?.StringCellValue;
            HashSet<string> keys = new HashSet<string>();
            while (!string.IsNullOrEmpty(key))
            {
                if (keys.Contains(key))
                {
                    Debug.LogWarning($"包含重复Key:{key},在表中位置【{rowNum + 1}】行");
                }
                else
                { 
                    var kws = new KeyWords() { key = key, words = new List<Words>() };
                    cfg.keyWords.Add(kws);
                    foreach (var kv in lgsDics) {
                        string word = row.GetCell(kv.Value)?.StringCellValue;
                        if (string.IsNullOrEmpty(word))
                        {
                            Debug.LogWarning($"Key:{key}所对应word为空,在表中位置【{rowNum + 1}】行，【{kv.Value + 1}】列");
                        }
                        else
                        {
                            kws.words.Add(new Words() { language=kv.Key,word= word });
                        }
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
        void GenerClass(HashSet<string> keys)
        {
            if (keys == null) return;
            if (!File.Exists(classPath))
            {
                using (File.Create(classPath)) { }
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace XIHLocalization");
            sb.AppendLine("{");
            sb.AppendLine($"    public partial class I18NKeys");
            sb.AppendLine("    {");
            foreach (var key in keys)
            {
                sb.AppendLine($"        public static string {key.ToUpper()} =>  {nameof(I18NUtil)}.{nameof(I18NUtil.TranslateText)}(\"{key}\");");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            Debug.Log($"生成 {classPath} 成功");
            File.WriteAllText(classPath, sb.ToString());
        }
    }
}

