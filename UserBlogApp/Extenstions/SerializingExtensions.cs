using System.Text.Json;

namespace UserBlogApp.Extenstions
{
    public static class SerializingExtensions
    {
        public static string ToJson(this object target) 
            => JsonSerializer.Serialize(target);
    }
}
