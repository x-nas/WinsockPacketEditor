using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WinsockPacketEditor
{
    public static class PacketExtractorConfig
    {
        public static readonly BindingList<PacketExtractorInfo> Items = new BindingList<PacketExtractorInfo>();

        public static void EnsureTable()
        {
            using (var conn = new SqliteConnection(Operate.DataBase.conStr))
            using (var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS PacketExtractor (GUID TEXT NOT NULL PRIMARY KEY, Data TEXT NOT NULL);", conn))
            { conn.Open(); cmd.ExecuteNonQuery(); }
        }

        public static void Load()
        {
            try
            {
                EnsureTable(); var fresh = new List<PacketExtractorInfo>();
                using (var conn = new SqliteConnection(Operate.DataBase.conStr))
                using (var cmd = new SqliteCommand("SELECT Data FROM PacketExtractor;", conn))
                { conn.Open(); using (var reader = cmd.ExecuteReader()) while (reader.Read()) { var item = JsonConvert.DeserializeObject<PacketExtractorInfo>(reader.GetString(0)); if (item != null) { string ignored; Normalize(item, out ignored); fresh.Add(item); } } }
                Items.Clear(); foreach (var item in fresh) Items.Add(item); PacketVariableEngine.Publish(Items);
            }
            catch (Exception ex) { Operate.DoLog(nameof(Load), ex); }
        }

        public static void Save()
        {
            try
            {
                EnsureTable(); using (var conn = new SqliteConnection(Operate.DataBase.conStr))
                { conn.Open(); using (var tx = conn.BeginTransaction()) { using (var del = new SqliteCommand("DELETE FROM PacketExtractor;", conn, tx)) del.ExecuteNonQuery(); foreach (PacketExtractorInfo item in Items) using (var cmd = new SqliteCommand("INSERT INTO PacketExtractor(GUID,Data) VALUES(@id,@data);", conn, tx)) { cmd.Parameters.AddWithValue("@id", item.Id.ToString("D")); cmd.Parameters.AddWithValue("@data", JsonConvert.SerializeObject(item)); cmd.ExecuteNonQuery(); } tx.Commit(); } }
                PacketVariableEngine.Publish(Items);
            }
            catch (Exception ex) { Operate.DoLog(nameof(Save), ex); }
        }

        /// <summary>
        /// 取值器列表 + 每个变量的当前运行值（仅显示）。
        ///
        /// CurrentValue 在模型上是 [JsonIgnore]（不落库、不下推），直接序列化
        /// PacketExtractorInfo 供桥接返回时会被丢掉；这里显式投影成带 CurrentValue
        /// 的行，界面「当前值」列才能读到外壳运行值或注入目标回传的镜像。
        /// </summary>
        public static List<PacketExtractorRow> GetRows()
        {
            var rows = new List<PacketExtractorRow>();
            foreach (PacketExtractorInfo extractor in Items)
            {
                if (extractor == null) continue;
                var row = new PacketExtractorRow
                {
                    Id = extractor.Id,
                    Name = extractor.Name,
                    IsEnable = extractor.IsEnable,
                    Scope = extractor.Scope,
                    Description = extractor.Description,
                    Variables = new List<PacketVariableRow>(),
                };
                foreach (PacketVariableInfo variable in extractor.Variables ?? new List<PacketVariableInfo>())
                {
                    if (variable == null) continue;
                    row.Variables.Add(new PacketVariableRow
                    {
                        Id = variable.Id,
                        Name = variable.Name,
                        Kind = variable.Kind,
                        DataType = variable.DataType,
                        Value = variable.Value,
                        Extraction = variable.Extraction,
                        TtlSeconds = variable.TtlSeconds,
                        CurrentValue = PacketVariableEngine.GetCurrentDisplay(extractor, variable),
                    });
                }
                rows.Add(row);
            }
            return rows;
        }

        /// <summary>按 GUID 找取值器与变量；保存滤镜的替换格时校验引用是否还在。</summary>
        public static bool TryGetVariable(Guid extractorId, Guid variableId, out PacketExtractorInfo extractor, out PacketVariableInfo variable)
        {
            extractor = Items.FirstOrDefault(x => x != null && x.Id == extractorId);
            variable = extractor == null || extractor.Variables == null ? null : extractor.Variables.FirstOrDefault(x => x != null && x.Id == variableId);
            return extractor != null && variable != null;
        }

        /// <summary>系统备份使用的分节。保留 GUID，确保滤镜中的取值器/变量引用恢复后仍然有效。</summary>
        public static XElement GetBackupXml()
        {
            return new XElement("PacketExtractors", new XElement("Data", JsonConvert.SerializeObject(Items.ToList(), Formatting.Indented)));
        }

        /// <summary>从系统备份整份恢复取值器；节存在即替换当前列表。</summary>
        public static void LoadBackupXml(XElement section)
        {
            if (section == null || section.Name != "PacketExtractors") return;
            XElement data = section.Element("Data");
            List<PacketExtractorInfo> loaded = data == null ? null : JsonConvert.DeserializeObject<List<PacketExtractorInfo>>(data.Value);
            if (loaded == null) throw new InvalidDataException("取值器备份格式无效");
            Items.Clear();
            foreach (PacketExtractorInfo item in loaded)
            {
                string error;
                if (!Normalize(item, out error)) throw new InvalidDataException(error);
                Items.Add(item);
            }
            Save();
        }

        public static string Add() { var item = new PacketExtractorInfo { Name = UI.T("PacketExtractor.DefaultName", "取值器") + " " + (Items.Count + 1) }; Items.Add(item); Save(); return item.Id.ToString().ToUpperInvariant(); }
        public static bool SaveOne(PacketExtractorInfo item, out string error) { if (!Normalize(item, out error)) return false; var old = Items.FirstOrDefault(x => x.Id == item.Id); if (old == null) Items.Add(item); else Items[Items.IndexOf(old)] = item; Save(); return true; }
        public static int Delete(IEnumerable<Guid> ids) { var set = new HashSet<Guid>(ids ?? Enumerable.Empty<Guid>()); int count = 0; for (int i = Items.Count - 1; i >= 0; i--) if (set.Contains(Items[i].Id)) { Items.RemoveAt(i); count++; } if (count > 0) Save(); return count; }

        public static void SetAllEnable(bool enable)
        {
            foreach (PacketExtractorInfo item in Items) item.IsEnable = enable;
            Save();
        }

        public static async Task<bool> ClearDialog()
        {
            if (Items.Count == 0 || !await UI.Confirm("取值器", "确定删除全部取值器吗？")) return false;
            Items.Clear(); Save(); return true;
        }

        public static async Task ExportDialog(IEnumerable<Guid> ids = null)
        {
            List<PacketExtractorInfo> picked = Pick(ids);
            if (picked.Count == 0) return;
            string path = await UI.PickSave(new FilePick { Filter = "取值器列表文件（*.pex）|*.pex", FileName = picked.Count == 1 ? picked[0].Name : "取值器列表" });
            if (string.IsNullOrEmpty(path)) return;
            var password = await Operate.SystemConfig.GetEncryptExportAsync("导出取值器列表");
            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("PacketExtractors", new XElement("Data", JsonConvert.SerializeObject(picked, Formatting.Indented))));
            document.Save(path);
            if (password.DoEncrypt) Operate.SystemConfig.EncryptXMLFile(path, password.Password);
            UI.Notify(UiIcon.Success, "导出取值器列表成功", path);
        }

        public static async Task ImportDialog()
        {
            string path = await UI.PickOpen(new FilePick { Filter = "取值器列表文件（*.pex）|*.pex" });
            if (string.IsNullOrEmpty(path)) return;
            XDocument encrypted = null;
            if (Operate.SystemConfig.IsEncryptXMLFile(path))
            {
                encrypted = await Operate.SystemConfig.GetEncryptImportAsync("导入取值器列表", path);
                if (encrypted == null) return;
            }
            List<PacketExtractorInfo> loaded = ReadImportFile(path, encrypted);
            if (loaded == null) throw new InvalidDataException("取值器文件格式无效");
            int count = 0;
            foreach (PacketExtractorInfo item in loaded)
            {
                if (item == null) continue;
                item.Id = Guid.NewGuid();
                item.Name = UniqueName(item.Name);
                foreach (PacketVariableInfo variable in item.Variables ?? new List<PacketVariableInfo>()) variable.Id = Guid.NewGuid();
                string error;
                if (Normalize(item, out error)) { Items.Add(item); count++; }
            }
            if (count > 0) { Save(); UI.Notify(UiIcon.Success, "导入取值器列表成功", path); }
        }

        /// <summary>当前 .pex 为可加密的 XML 容器；保留第一版 JSON 文件的导入兼容。</summary>
        private static List<PacketExtractorInfo> ReadImportFile(string path, XDocument document)
        {
            if (document == null)
            {
                string text = File.ReadAllText(path, Encoding.UTF8);
                if (!text.TrimStart().StartsWith("<", StringComparison.Ordinal)) return JsonConvert.DeserializeObject<List<PacketExtractorInfo>>(text);
                document = XDocument.Parse(text);
            }
            XElement data = document.Root == null ? null : document.Root.Element("Data");
            if (data == null || document.Root.Name != "PacketExtractors") throw new InvalidDataException("取值器文件格式无效");
            return JsonConvert.DeserializeObject<List<PacketExtractorInfo>>(data.Value);
        }

        public static async Task<int> ApplyListAction(Operate.SystemConfig.ListAction action, IEnumerable<Guid> ids)
        {
            List<PacketExtractorInfo> picked = Pick(ids);
            if (picked.Count == 0) return 0;
            int before = Items.Count;
            switch (action)
            {
                case Operate.SystemConfig.ListAction.Top: MoveTop(picked); break;
                case Operate.SystemConfig.ListAction.Up: MoveUp(picked); break;
                case Operate.SystemConfig.ListAction.Down: MoveDown(picked); break;
                case Operate.SystemConfig.ListAction.Bottom: MoveBottom(picked); break;
                case Operate.SystemConfig.ListAction.Copy:
                    foreach (PacketExtractorInfo item in picked) { PacketExtractorInfo copy = Clone(item); copy.Id = Guid.NewGuid(); copy.Name = UniqueName(copy.Name + " 副本"); foreach (PacketVariableInfo variable in copy.Variables) variable.Id = Guid.NewGuid(); Items.Add(copy); }
                    break;
                case Operate.SystemConfig.ListAction.Export: await ExportDialog(picked.Select(x => x.Id)); return 0;
                case Operate.SystemConfig.ListAction.Delete:
                    if (await UI.Confirm("取值器", "确定删除选中的取值器吗？")) foreach (PacketExtractorInfo item in picked) Items.Remove(item);
                    break;
            }
            if (action != Operate.SystemConfig.ListAction.Export) Save();
            return Items.Count - before;
        }

        private static List<PacketExtractorInfo> Pick(IEnumerable<Guid> ids)
        {
            if (ids == null) return Items.ToList();
            var set = new HashSet<Guid>(ids); return Items.Where(x => set.Contains(x.Id)).ToList();
        }
        private static string UniqueName(string raw)
        {
            string basis = string.IsNullOrWhiteSpace(raw) ? "取值器" : raw.Trim(); string name = basis; int n = 2;
            while (Items.Any(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase))) name = basis + " " + n++;
            return name;
        }
        private static void MoveTop(List<PacketExtractorInfo> list) { foreach (PacketExtractorInfo item in list) Items.Remove(item); for (int i = list.Count - 1; i >= 0; i--) Items.Insert(0, list[i]); }
        private static void MoveBottom(List<PacketExtractorInfo> list) { foreach (PacketExtractorInfo item in list) Items.Remove(item); foreach (PacketExtractorInfo item in list) Items.Add(item); }
        private static void MoveUp(List<PacketExtractorInfo> list) { var set = new HashSet<PacketExtractorInfo>(list); for (int i = 1; i < Items.Count; i++) if (set.Contains(Items[i]) && !set.Contains(Items[i - 1])) { PacketExtractorInfo x = Items[i - 1]; Items[i - 1] = Items[i]; Items[i] = x; } }
        private static void MoveDown(List<PacketExtractorInfo> list) { var set = new HashSet<PacketExtractorInfo>(list); for (int i = Items.Count - 2; i >= 0; i--) if (set.Contains(Items[i]) && !set.Contains(Items[i + 1])) { PacketExtractorInfo x = Items[i + 1]; Items[i + 1] = Items[i]; Items[i] = x; } }

        private static PacketExtractorInfo Clone(PacketExtractorInfo item) { return JsonConvert.DeserializeObject<PacketExtractorInfo>(JsonConvert.SerializeObject(item)); }
        private static bool Normalize(PacketExtractorInfo item, out string error)
        {
            error = null; if (item == null) { error = "取值器不能为空"; return false; }
            item.Name = (item.Name ?? string.Empty).Trim(); if (item.Name.Length == 0 || item.Name.Length > 64) { error = "取值器名称必须为 1 到 64 个字符"; return false; }
            if (item.Id == Guid.Empty) item.Id = Guid.NewGuid(); if (Items.Any(x => x.Id != item.Id && string.Equals(x.Name, item.Name, StringComparison.OrdinalIgnoreCase))) { error = "取值器名称已存在"; return false; }
            item.Variables = item.Variables ?? new List<PacketVariableInfo>(); if (item.Variables.Count > 200) { error = "一个取值器最多 200 个变量"; return false; }
            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (PacketVariableInfo v in item.Variables) { if (v == null) { error = "变量不能为空"; return false; } v.Name = (v.Name ?? string.Empty).Trim(); if (v.Name.Length == 0 || v.Name.Length > 64 || !names.Add(v.Name)) { error = "变量名不能为空、不能重复，且最多 64 个字符"; return false; } if (v.Id == Guid.Empty) v.Id = Guid.NewGuid(); v.Extraction = v.Extraction ?? new PacketExtractionSpec(); if (v.Extraction.Length < 1 || v.Extraction.Length > 4 * 1024 * 1024) { error = "取值长度必须在 1 到 4 MB 之间"; return false; } if (v.TtlSeconds < 0 || v.TtlSeconds > 604800) { error = "TTL 超出范围"; return false; } }
            return true;
        }
    }
}
