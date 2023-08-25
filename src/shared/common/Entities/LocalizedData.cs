using Newtonsoft.Json;

namespace Common.Entities
{
    public class LocalizedData : Dictionary<string, string>
    {
        public LocalizedData()
        {
        }

        public LocalizedData(params string[] prams)
        {
            var languages = LanguagesModel.GetLanguages();
            for (int i = 0; i < languages.Count; i++)
            {
                this.Add(languages[i], prams[i]);
            }
        }

        public LocalizedData(string jsonObj)
        {
            if (jsonObj == null)
            {
                foreach (var lang in LanguagesModel.GetLanguages())
                    Add(lang, "");
                return;
            }

            var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonObj);

            LanguagesModel.GetLanguages().ToList().ForEach(k =>
            {
                try
                {
                    if (obj[k] != null)
                        Add(k, obj[k]);
                }
                catch (Exception)
                {
                    Add(k, "");
                }
            });
        }

        public override string ToString()
        {
            try
            {
                var x = JsonConvert.SerializeObject(this);
                return x;
            }
            catch (Exception)
            {
                return "{'en':'','ar':'','id':''}";
            }
        }
    }

    public static class LanguagesModel
    {
        static List<string> Languages = new() { "en", "ar", "id" };

        public static List<string> GetLanguages()
        {
            return Languages;
        }

        public static void AddLanguage(string language)
        {
            Languages.Add(language);
        }

        public static void AddLanguages(string[] languages)
        {
            Languages.AddRange(languages);
        }
    }
}