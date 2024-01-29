using Dapper;
using SV20T1080063.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080063.DataLayers.SQLServer
{
    public class EmployeeUserAccountDAL : _BaseDAL, IUserAccountDAL
    {
        /// <summary>
        /// Cài đặt các phép xử lý liên quan đến tài khoản Employee 
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeUserAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string userName, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select EmployeeID as UserId, Email as UserName, FullName, Email, Photo
                    from Employees 
                    where Email = @userName and Password = @password";
                var parameters = new 
                {
                    userName = userName,
                    password = password
                };
                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }


                return data;
        }

        public bool ChangePass(string userName, string password)
        {
            int data;

            using (var connection = OpenConnection())
            {
                var sql = @"update Employees
                                set Password = @password
                            where Email = @username";
                var parameters = new
                {
                    userName = userName,
                    password = password
                };
                data = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }


            if (data == 1)
                return true;
            else 
                return false;
        }
    }
}
