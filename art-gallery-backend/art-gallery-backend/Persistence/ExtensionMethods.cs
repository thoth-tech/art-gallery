using FastMember;
using Npgsql;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Art_Gallery_Backend.Persistence
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
        /// Adds a NpgsqlParameter to the NpgsqlParameterCollection given the specified parameter name and value, works with nullable datatypes.
        /// </summary>
        /// <param name="collection">Represents a collection of parameters relevant to a NpgsqlCommand as well as their respective mappings to columns in a DataSet.</param>
        /// <param name="parameterName">The name of the NpgsqlParameter.</param>
        /// <param name="value">The value of the NpgsqlParameter to add to the collection.</param>
        /// <returns>The parameter that was added or null depending on the objects value.</returns>
        public static NpgsqlParameter AddWithNullableValue(this NpgsqlParameterCollection collection, string parameterName, object? value)
        {
            return value == null ? collection.AddWithValue(parameterName, DBNull.Value) : collection.AddWithValue(parameterName, value);
        }

        /// <summary>
        /// Maps field values, contained in database columns, to properties within each bounded context using FastMember ORM.
        /// Ignores snake casing of database column names, and correctly maps DATE (postgreSQL data type) to DateOnly with explicit casting.
        /// </summary>
        /// <typeparam name="T"> Specifies the type of the 'entity' parameter in the method's argument, and the type-constraint that can be
        /// placed on the method when called. </typeparam>
        /// <param name="dr">The data that is to be mapped from its database field, to properties within a bounded context.</param>
        /// <param name="entity">Responsible for specifying the type of object to be mapped by-name with FastMember's
        /// TypeAccessor.</param>
        /// <exception cref="ArgumentNullException"> Throws this exception the entity passed to the argument is null.</exception>
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
        /// Validates the password string to see if the password is of the appropriate length.
        /// </summary>
        /// <param name="str">The string to be validated.</param>
        /// <returns>Returns the result as a bool value.</returns>
        public static bool IsValidPassword(this string str)
        {
            return (str.Length >=14 && str.Length <= 24) ? true : false;
        }


        /// <summary>
        /// Validates the email string to see if the email is in the correct format using regex.
        /// </summary>
        /// <param name="str">The string to be validated.</param>
        /// <returns>Returns the result as a boolean value.</returns>
        public static bool IsValidEmail(this string str)
        {
           bool isEmail = Regex.IsMatch(str, @"^(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)$", RegexOptions.IgnoreCase);

           return isEmail;
        }
    }
}
