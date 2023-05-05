using Application.Authentication;
using Application.Authentication.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public static class Methods
    {


        private static readonly IPasswordHasher passwordHasher = new PasswordHasher();

        private static readonly IRecoveryHasher _recoveryHasher = new RecoveryHasher();


        public static void BrandModelSizeRequestCompilation(BrandModelSizeRequestModel data)
        {
            data.Brands = GlobalConfig.Connection.GetBrands_All();
            data.Models = GlobalConfig.Connection.GetModels_All();
            data.Sizes = GlobalConfig.Connection.GetSizes_All();
            data.Model_Sizes = GlobalConfig.Connection.GetModel_Sizes_All();
            data.Measurements = GlobalConfig.Connection.GetMeasurements_All();
        }

        public static List<string> SizeNamesToStringArray()
        {
            List<SizeModel> sizes = GlobalConfig.Connection.GetSizes_All();

            List<string> sizeNames = new List<string>();

            foreach (SizeModel size in sizes)
            {
                sizeNames.Add(size.SizeName);
            }
            return sizeNames;
        }

        public static void RegisterUser(UserModel user)
        {
            user.Password = passwordHasher.Hash(user.Password);
            GlobalConfig.Connection.RegisterUser(user);
            user.Password = null;
        }



        public static bool LoginUser(UserModel user, UserModel storedUser, IConfiguration configuration)
        {
            bool validate = passwordHasher.Verify(storedUser.Password, user.Password);

            if (validate)
            {
                user = GlobalConfig.Connection.GetUser_ByEmail(user);
                user.Password = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GetUserSubmissions(UserModel user)
        {
            UserSubmissionsModel userSubmissions = new UserSubmissionsModel();

            userSubmissions.User = GlobalConfig.Connection.GetUser_ByEmail(user);

            if (userSubmissions.User != null)
            {
                userSubmissions.UserModels = GlobalConfig.Connection.GetUserModels_ByUserId(user.Id);

                return true;
            }
            else
            {
                return false;
            }
        }

        public static void PostUserSubmissions(UserSubmissionsModel user)
        {
            GlobalConfig.Connection.CreateNewUserMeasurements(user);
            GlobalConfig.Connection.CreateNewUserModels(user.UserModels);
        }

        public static User_RecoveryModel CreateUserRecoveryHash(string email)
        {


            UserModel user = new UserModel();
            user.Email = email;

            user = GlobalConfig.Connection.GetUser_ByEmail(user);

            User_RecoveryModel recovery = new User_RecoveryModel();
            if (user.Id != null)
            {
                recovery.UserId = (int)user.Id;
                recovery.Hash = _recoveryHasher.Hash(email);
            }

            GlobalConfig.Connection.CreateNewUserRecovery(recovery);

            return recovery;
        }




    }
}
