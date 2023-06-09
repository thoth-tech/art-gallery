﻿using System.Globalization;
using Art_Gallery_Backend.Authentication;
using Art_Gallery_Backend.Models.DTOs;
using Art_Gallery_Backend.Persistence.Interfaces;
using BCrypt.Net;
using Npgsql;

namespace Art_Gallery_Backend.Models.Database_Models
{
    /// <summary>
    /// The User class is responsible for handling the database model associated with gallery users. 
    /// </summary>
    public class User : IUserDataAccess
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Role { get; set; } = "User";
        public DateTime? ActiveAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public User(int accountId, string firstName, string lastName, string email, string passwordHash, string? role, DateTime? activeAt, DateTime modifiedAt, DateTime createdAt)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            ActiveAt = activeAt;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;
        }

        // Active Record - Everything under line 36 is required for the active record implementation.
        private readonly IConfiguration? _configuration;

        public User(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        readonly TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        public List<UserOutputDto> GetUsers()
        {
            List<UserOutputDto> users = new();
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM account", connection);
                {
                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int userID = (int)dr["account_id"];
                                string firstName = (string)dr["first_name"];
                                string lastName = (string)dr["last_name"];
                                string email = (string)dr["email"];
                                string passwordHash = (string)dr["password_hash"];
                                string role = (string)dr["role"];
                                DateTime activeAt = (DateTime)dr["active_at"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];
                                users.Add(new UserOutputDto(userID, firstName, lastName, email, passwordHash, role, activeAt, modifiedAt, createdAt));
                            }
                        }
                        return users;
                    }
                }
            }
        }

        public UserOutputDto? GetUserById(int id)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM account WHERE account_id = @userID", connection);
                {
                    cmd.Parameters.AddWithValue("@userId", id);
                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int userID = (int)dr["account_id"];
                                string firstName = (string)dr["first_name"];
                                string lastName = (string)dr["last_name"];
                                string email = (string)dr["email"];
                                string passwordHash = (string)dr["password_hash"];
                                string role = (string)dr["role"];
                                DateTime activeAt = (DateTime)dr["active_at"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];
                                return new UserOutputDto(userID, firstName, lastName, email, passwordHash, role, activeAt, modifiedAt, createdAt);
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public string? AuthenticateUser(LoginDto login)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("SELECT * FROM account WHERE email = @email", connection);
                {
                    cmd.Parameters.AddWithValue("@email", login.Email);

                    using NpgsqlDataReader dr = cmd.ExecuteReader();
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                int userID = (int)dr["account_id"];
                                string firstName = (string)dr["first_name"];
                                string lastName = (string)dr["last_name"];
                                string email = (string)dr["email"];
                                string passwordHash = (string)dr["password_hash"];
                                string role = (string)dr["role"];
                                DateTime activeAt = (DateTime)dr["active_at"];
                                DateTime modifiedAt = (DateTime)dr["modified_at"];
                                DateTime createdAt = (DateTime)dr["created_at"];

                                UserOutputDto user = new(userID, firstName, lastName, email, passwordHash, role, activeAt, modifiedAt, createdAt);

                                // Authenticate user with given login information and return an auth token if valid
                                bool authenticated = BC.EnhancedVerify(login.Password, user.PasswordHash, hashType: HashType.SHA384);
                                if (authenticated)
                                {
                                    user.PasswordHash = string.Empty; // Removing password hash
                                    TokenAuthentication handler = new(_configuration!);

                                    return handler.GenerateToken(user);
                                }
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public UserInputDto? InsertUser(UserInputDto user)
        {

            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("INSERT INTO account(first_name, last_name, email, password_hash, role, active_at, modified_at, created_at) " +
                                                  "VALUES (@firstName, @lastName, @email, @passwordHash, @role, @activeAt, current_timestamp, current_timestamp);", connection);
                {
                    cmd.Parameters.AddWithValue("@firstName", textInfo.ToTitleCase(user.FirstName));
                    cmd.Parameters.AddWithValue("@lastName", textInfo.ToTitleCase(user.LastName));
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@passwordHash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384));
                    cmd.Parameters.AddWithValue("@role", "User");
                    cmd.Parameters.AddWithValue("@activeAt", DateTime.UtcNow);
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? user : null;
                }
            }
        }

        public UserInputDto? UpdateUser(int id, UserInputDto user)
        {

            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();

                string cmdString = "UPDATE account SET ";

                if (user.FirstName is not null and not "" and not "string")
                {
                    cmdString += "first_name = @firstName, ";
                }
                if (user.LastName is not null and not "" and not "string")
                {
                    cmdString += "last_name = @lastName, ";
                }
                if (user.Email is not null and not "" and not "string")
                {
                    cmdString += "email = @email, ";
                }
                if (user.Password is not null and not "" and not "string")
                {
                    cmdString += "password_hash = @passwordHash, ";
                }
                if (user.Role is not null && user.Role != "" && user.Password != "string")
                {
                    cmdString += "role = @role, ";
                }

                cmdString += "modified_at = current_timestamp WHERE account_id = @accountId";

                using NpgsqlCommand cmd = new(cmdString, connection);
                {
                    cmd.Parameters.AddWithValue("@accountId", id);
                    if (user.FirstName is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@firstName", textInfo.ToTitleCase(user.FirstName));
                    }
                    if (user.LastName is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@lastName", textInfo.ToTitleCase(user.LastName));
                    }
                    if (user.Email is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@email", user.Email);
                    }
                    if (user.Password is not null and not "" and not "string")
                    {
                        cmd.Parameters.AddWithValue("@passwordHash", BC.EnhancedHashPassword(user.Password, hashType: HashType.SHA384));
                    }
                    if (user.Role is not null && user.Role != "" && user.Password != "string")
                    {
                        cmd.Parameters.AddWithValue("@role", textInfo.ToTitleCase(user.Role));
                    }
                    int result = cmd.ExecuteNonQuery();
                    return result is 1 ? user : null;
                }
            }
        }

        public bool DeleteUser(int id)
        {
            using NpgsqlConnection connection = new(_configuration.GetConnectionString("PostgresSQL"));
            {
                connection.Open();
                using NpgsqlCommand cmd = new("DELETE FROM account WHERE account_id = @accountId", connection);
                {
                    cmd.Parameters.AddWithValue("@accountId", id);
                    int result = cmd.ExecuteNonQuery();
                    return result == 1;
                }
            }
        }
    }
}
