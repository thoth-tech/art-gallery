using FastMember;
using Npgsql;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence
{
    public static class ExtensionMethods
    {
        public static bool ValidateURL(string url)
        {
            Uri myUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out myUri))
                return true;
            return false;
        }
        public static T? ConvertFromNullableValue<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return default;
            else
                return (T)obj;
        }

        public static NpgsqlParameter AddWithNullableValue(this NpgsqlParameterCollection collection, string parameterName, object? value)
        {
            if (value == null)
                return collection.AddWithValue(parameterName, DBNull.Value);
            else
                return collection.AddWithValue(parameterName, value);
        }

        public static void MapTo<T>(this NpgsqlDataReader dr, T entity)
        {
            if (entity == null) throw new
            ArgumentNullException(nameof(entity));

            var fastMember = TypeAccessor.Create(entity.GetType());
            var props = fastMember.GetMembers().Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < dr.FieldCount; i++)
            {
                var prop = props.FirstOrDefault(x => x.Equals(dr.GetName(i), StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(prop))
                    fastMember[entity, prop] = dr.IsDBNull(i) ? null : dr.GetValue(i);
            }
        }

        public static string ToCamelCase(this string str) =>
        string.IsNullOrEmpty(str) || str.Length < 2
            ? str.ToLowerInvariant()
            : char.ToLowerInvariant(str[0]) + str.Substring(1);

        public static bool IsValidURL(this string str)
        {
            Uri myUri;
            if (Uri.TryCreate(str, UriKind.Absolute, out myUri))
                return true;
            return false;
        }
    }
}
