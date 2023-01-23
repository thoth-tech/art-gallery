using FastMember;
using Npgsql;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Aboriginal_Art_Gallery_of_Australia.Persistence
{
    /// <summary>
    /// The ExtensionMethods class provides the ability "add" methods to existing types without creating a new derived type, recompiling, or otherwise modifying the original type. 
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Helper Utility for when retriving nullable types from the database, to either return default or the appropriate type where applicable.
        /// </summary>
        /// <typeparam name="T">The type in which value will be returned, it could be any type eg. int, string, bool, etc.</typeparam>
        /// <param name="obj">The object to be checked.</param>
        /// <returns>Returns either default or the appropriate datatype.</returns>
        public static T? ConvertFromNullableValue<T>(object obj)
        {
            return obj == null || obj == DBNull.Value ? default : (T)obj;
        }

        /// <summary>
        /// Adds a <see cref="NpgsqlParameter"/> to the <see cref="NpgsqlParameterCollection"/> given the specified parameter name and
        /// value, works with nullable datatypes.
        /// </summary>
        /// <param name="collection">Represents a collection of parameters relevant to a <see cref="NpgsqlCommand"/> as well as their respective mappings to columns in a <see cref="DataSet"/>.</param>
        /// <param name="parameterName">The name of the <see cref="NpgsqlParameter"/>.</param>
        /// <param name="value">The value of the <see cref="NpgsqlParameter"/> to add to the collection.</param>
        /// <returns>The parameter that was added or null depending on the objects value.</returns>
        public static NpgsqlParameter AddWithNullableValue(this NpgsqlParameterCollection collection, string parameterName, object? value)
        {
            return value == null ? collection.AddWithValue(parameterName, DBNull.Value) : collection.AddWithValue(parameterName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void MapTo<T>(this NpgsqlDataReader dr, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            TypeAccessor fastMember = TypeAccessor.Create(entity.GetType());
            HashSet<string> props = fastMember.GetMembers().Select(x => x.Name).ToHashSet();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                string dbColumnName = dr.GetName(i);

                string[] splits = dbColumnName.Split('_');
                StringBuilder columnName = new("");
                foreach (string split in splits)
                {
                    _ = columnName.Append(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(split.ToLower()));
                }

                string? prop = props.FirstOrDefault(x => x.Equals(columnName.ToString()));

                try
                {
                    if (!string.IsNullOrEmpty(prop))
                    {
                        fastMember[entity, prop] = dr.IsDBNull(i) ? null : dr.GetValue(i);
                    }
                }
                catch (InvalidCastException e)
                when (e.Message.Equals("Unable to cast object of type 'System.DateTime' to type 'System.DateOnly'."))
                {
                    DateOnly dateCast = DateOnly.FromDateTime((DateTime)dr.GetValue(i));
                    fastMember[entity, prop] = dateCast;
                }
            }
        }

        /// <summary>
        /// Validates the URL to ensure it is of type: absolute and includes the appropriate extension.
        /// </summary>
        /// <param name="str">The string to be validated.</param>
        /// <returns>Returns the result as a boolean value.</returns>
        public static bool IsValidURL(this string str)
        {
            if (Uri.TryCreate(str, UriKind.Absolute, out Uri? myUri))
            {
                if (
                    myUri.AbsolutePath.Contains(".jpg") ||
                    myUri.AbsolutePath.Contains(".jpeg") ||
                    myUri.AbsolutePath.Contains(".png") ||
                    myUri.AbsolutePath.Contains(".webp") ||
                    myUri.AbsolutePath.Contains(".gif")
                    )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validates the password string to see if the password contains any illegal characters.
        /// </summary>
        /// <param name="str">The string to be validated.</param>
        /// <returns>Returns the result as a boolean value.</returns>
        public static bool IsValidPassword(this string str)
        {
            Regex validPassword = new("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            return validPassword.IsMatch(str);
        }

        /// Option 1: Email Validation using the .NET library.
        /// <summary>
        /// Validates the email string to see if the email is in the correct format using the .NET mail library.
        /// </summary>
        /// <param name="str">The string to be validated.</param>
        /// <returns>Returns the result as a boolean value.</returns>
        public static bool IsValidEmail(this string str)
        {
            try
            {
                MailAddress m = new(str);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// Option 2: Email Validation using Regex
        /// <summary>
        /// Validates the email string to see if the email is in the correct format using regex.
        /// </summary>
        /// <param name="str">The string to be validated.</param>
        /// <returns>Returns the result as a boolean value.</returns>
        //public static bool IsValidEmail(this string str)
        //{
        //    Regex validEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
        //    if (validEmail.IsMatch(str))
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
