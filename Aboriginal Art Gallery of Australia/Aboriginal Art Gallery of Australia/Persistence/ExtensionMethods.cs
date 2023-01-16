using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Text;
using FastMember;
using Npgsql;


namespace Aboriginal_Art_Gallery_of_Australia.Persistence
{
    public static class ExtensionMethods
    {
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
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var fastMember = TypeAccessor.Create(entity.GetType());
            var props = fastMember.GetMembers().Select(x => x.Name).ToHashSet();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                var dbColumnName = dr.GetName(i);

                var splits = dbColumnName.Split('_');
                var columnName = new StringBuilder("");
                foreach (var split in splits)
                {
                    columnName.Append(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(split.ToLower()));
                }

                var prop = props.FirstOrDefault(x => x.Equals(columnName.ToString()));

                if (prop == "StartDate" || prop == "EndDate")
                {
                    DateOnly dateCast = DateOnly.FromDateTime((DateTime)dr.GetValue(i));
                    fastMember[entity, prop] = dateCast;
                }

                else if (!string.IsNullOrEmpty(prop)) fastMember[entity, prop] = dr.IsDBNull(i) ? null : dr.GetValue(i);
            }
        }

        public static string ToCamelCase(this string str) =>
        string.IsNullOrEmpty(str) || str.Length < 2
            ? str.ToLowerInvariant()
            : char.ToLowerInvariant(str[0]) + str.Substring(1);

        public static bool IsValidURL(this string str)
        {
            if (Uri.TryCreate(str, UriKind.Absolute, out Uri? myUri))
                if (
                    myUri.AbsolutePath.Contains(".jpg") ||
                    myUri.AbsolutePath.Contains(".jpeg") ||
                    myUri.AbsolutePath.Contains(".png") ||
                    myUri.AbsolutePath.Contains(".webp") ||
                    myUri.AbsolutePath.Contains(".gif")
                    )
                    return true;
            return false;
        }

        public static bool IsValidEmail(this string str)
        {
            // Validate if a str is a valid email
            Regex validEmail = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$");
            if (validEmail.IsMatch(str))
            {
                return true;
            }
            return false;
        }

        public static bool IsValidPassword(this string str)
        {
            // Validate if a str is a valid password
            Regex validPassword = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.).*$");
            if (validPassword.IsMatch(str))
            {
                return true;
            }
            return false;
        }
    }
}
