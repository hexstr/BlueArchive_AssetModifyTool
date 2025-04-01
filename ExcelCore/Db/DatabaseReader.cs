namespace ExcelCore.Db
{
    public class DatabaseReader
    {
        private readonly ExcelContext context_;
        private readonly DatabaseDumper database_dumper_;

        public DatabaseReader(ExcelContext context, bool is_global)
        {
            SQLitePCL.Batteries.Init();
            context_ = context;
            database_dumper_ = new DatabaseDumper(is_global);
        }

        public void Dump(bool is_raw)
        {
            string dump_directory = "dump";
            if (is_raw)
            {
                dump_directory = "raw";
            }
            Directory.CreateDirectory(dump_directory);

            var tasks = new List<Task>
            {
                database_dumper_.Dump([.. context_.CharacterDialogDBSchema.Select(x => x.Bytes)], "CharacterDialogExcel", is_raw),
                database_dumper_.Dump([.. context_.CharacterDialogEventDBSchema.Select(x => x.Bytes)], "CharacterDialogEventExcel", is_raw),
                database_dumper_.Dump([.. context_.LocalizeDBSchema.Select(x => x.Bytes)], "LocalizeExcel", is_raw),
                database_dumper_.Dump([.. context_.LocalizeErrorDBSchema.Select(x => x.Bytes)], "LocalizeErrorExcel", is_raw),
                database_dumper_.Dump([.. context_.LocalizeEtcDBSchema.Select(x => x.Bytes)], "LocalizeEtcExcel", is_raw),
                database_dumper_.Dump([.. context_.LocalizeSkillDBSchema.Select(x => x.Bytes)], "LocalizeSkillExcel", is_raw),
                database_dumper_.Dump([.. context_.ScenarioCharacterNameDBSchema.Select(x => x.Bytes)], "ScenarioCharacterNameExcel", is_raw),
                database_dumper_.Dump([.. context_.ScenarioScriptDBSchema.Select(x => x.Bytes)], "ScenarioScriptExcel", is_raw),
                database_dumper_.Dump([.. context_.VoiceDBSchema.Select(x => x.Bytes)], "VoiceExcel", is_raw),
            };

            Task.WaitAll([.. tasks]);
        }
    }
}
