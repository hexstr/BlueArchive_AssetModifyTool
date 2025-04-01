using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ExcelCore.Db
{
    public class BaseDBSchema
    {
        [Key]
        public int RowId { get; set; }
        public byte[] Bytes { get; set; }
    }

    public class CharacterDialogDBSchema : BaseDBSchema
    {
        public long CharacterId { get; set; }
        public int DialogCategory { get; set; }
        public int DialogCondition { get; set; }
    }

    public class CharacterDialogEventDBSchema : BaseDBSchema
    {
        public long CostumeUniqueId { get; set; }
        public long OriginalCharacterId { get; set; }
        public long EventID { get; set; }
        public int DialogCategory { get; set; }
        public int DialogCondition { get; set; }
    }

    public class LocalizeDBSchema : BaseDBSchema
    {
        public long Key { get; set; }
    }

    public class LocalizeErrorDBSchema : BaseDBSchema
    {
        public long Key { get; set; }
    }

    public class LocalizeEtcDBSchema : BaseDBSchema
    {
        public long Key { get; set; }
    }

    public class LocalizeSkillDBSchema : BaseDBSchema
    {
        public long Key { get; set; }
    }

    public class ScenarioCharacterNameDBSchema : BaseDBSchema
    {
        public long CharacterName { get; set; }
    }

    public class ScenarioScriptDBSchema : BaseDBSchema
    {
        public long GroupId { get; set; }
    }

    public class VoiceDBSchema : BaseDBSchema
    {
        public long Id { get; set; }
    }

    public class ExcelContext : DbContext
    {
        private readonly string file_path_;

        public DbSet<CharacterDialogDBSchema> CharacterDialogDBSchema { get; set; }
        public DbSet<CharacterDialogEventDBSchema> CharacterDialogEventDBSchema { get; set; }
        public DbSet<LocalizeDBSchema> LocalizeDBSchema { get; set; }
        public DbSet<LocalizeErrorDBSchema> LocalizeErrorDBSchema { get; set; }
        public DbSet<LocalizeEtcDBSchema> LocalizeEtcDBSchema { get; set; }
        public DbSet<LocalizeSkillDBSchema> LocalizeSkillDBSchema { get; set; }
        public DbSet<ScenarioCharacterNameDBSchema> ScenarioCharacterNameDBSchema { get; set; }
        public DbSet<ScenarioScriptDBSchema> ScenarioScriptDBSchema { get; set; }
        public DbSet<VoiceDBSchema> VoiceDBSchema { get; set; }

        public ExcelContext(string file_path)
        {
            file_path_ = file_path;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={file_path_}");
    }
}
