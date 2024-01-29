using SV20T1080063.DataLayers;
using SV20T1080063.DataLayers.SQLServer;
using SV20T1080063.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080063.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeUserAccountDB;
        private static readonly IUserAccountDAL customerUserAccountDB;

        static UserAccountService()
        {
            string connectionString = "server=MUVODICH\\MANUTD; user id=sa;password=123;database=LiteCommerceDB;TrustServerCertificate=true";
            employeeUserAccountDB = new DataLayers.SQLServer.EmployeeUserAccountDAL(connectionString);
            customerUserAccountDB = new DataLayers.SQLServer.CustomerUserAccountDAL(connectionString);
        }

        public static UserAccount? Authorize(string userName, string password, TypeOfAccount typeOfAccount)
        {
            switch (typeOfAccount)
            {
                case TypeOfAccount.Employee:
                    return employeeUserAccountDB.Authorize(userName, password);
                case TypeOfAccount.Customer:
                    return customerUserAccountDB.Authorize(userName, password);
                default: 
                    return null;

            }
        }
        public static bool ChangePass(string username, string password, TypeOfAccount typeOfAccount)
        {
            switch (typeOfAccount)
            {
                case TypeOfAccount.Employee:
                    return employeeUserAccountDB.ChangePass(username, password);
                case TypeOfAccount.Customer:
                    return customerUserAccountDB.ChangePass(username, password);
                // customer
                default: return false;
            }
        }
    }
    public enum TypeOfAccount
    {
        Employee,
        Customer
    }
}
