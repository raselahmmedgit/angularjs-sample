using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using lab.ngdemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Dapper
{
    public class UserRepository : IUserRepository
    {
        private IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public List<User> GetAll()
        {
            return this._db.Query<User>("SELECT * FROM User").ToList();
        }

        public User Find(int id)
        {
            return this._db.Query<User>("SELECT * FROM Users WHERE UserID = @UserID", new { id }).SingleOrDefault();
        }

        public User Add(User user)
        {
            var sqlQuery = "INSERT INTO Users (FirstName, LastName, Email) VALUES(@FirstName, @LastName, @Email); " + "SELECT CAST(SCOPE_IDENTITY() as int)";
            var userId = this._db.Query<int>(sqlQuery, user).Single();
            user.UserID = userId;
            return user;
        }

        public User Update(User user)
        {
            var sqlQuery =
                "UPDATE Users " +
                "SET FirstName = @FirstName, " +
                "    LastName  = @LastName, " +
                "    Email     = @Email " +
                "WHERE UserID = @UserID";
            this._db.Execute(sqlQuery, user);
            return user;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUserInformatiom(int id)
        {
            using (var multipleResults = this._db.QueryMultiple("GetUserByID", new { Id = id }, commandType: CommandType.StoredProcedure))
            {
                var user = multipleResults.Read<User>().SingleOrDefault();
                

                return user;
            }
        }
    }

    public interface IUserRepository
    {
        List<User> GetAll();
        User Find(int id);
        User Add(User user);
        User Update(User user);
        void Remove(int id);
        User GetUserInformatiom(int id);
    }
}