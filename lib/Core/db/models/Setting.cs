namespace XSing.Core.db.models
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using env;

    public class Setting
    {
        public Setting() { }

        public Setting(string key, string val)
        {
            this.Key = key;
            this.Value = val;
        }
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public static void Default()
        {
            var db = new SingContext();
            db.Database.EnsureCreated();
            Verify(db, Version, Env.Version);
            Verify(db, Login, "nero");
            Verify(db, IsBlockInternet, "true");
        }
        public static bool If(string key) => new SingContext().Settings.Any(x => x.Key == key);
        public static string Get(string key) => new SingContext().Settings.FirstOrDefault(x => x.Key == key)?.Value;
        public static string GetOrDefault(string key, string @default) => Get(key) ?? @default;

        public static void Set(string key, string value)
        {
            var ctx = new SingContext();

            if (If(key))
            {
                ctx.Settings.First(x => x.Key == key).Value = value;
                ctx.SaveChanges();
                return;
            }
            Verify(ctx, key, value);
        }

        private static void Verify(SingContext db, string key, string value)
        {
            if (!db.Settings.Any(x => x.Key == key))
                return;
            db.Add(new Setting(key, value));
            db.SaveChanges();
        }

        public const string IsBlockInternet = "onlyLocalConnection";
        public const string Login = "login";
        public const string Version = "version";
    }
}