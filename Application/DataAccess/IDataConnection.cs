using Domain.Models;

namespace Application.DataAccess
{
    public interface IDataConnection
    {

        List<BrandModel> GetBrands_All();

        List<ModelModel> GetModels_All();

        List<SizeModel> GetSizes_All();

        List<Model_SizeModel> GetModel_Sizes_All();

        List<MeasurementModel> GetMeasurements_All();

        List<UserModel> GetUsers_All();

        UserModel GetUser_ByEmail(UserModel user);

        List<User_ModelModel> GetUserModels_ByUserId(int? userId);

        void UpdateUser(UserModel user);

        void RegisterUser(UserModel user);

        void CreateNewUserMeasurements(UserSubmissionsModel user);

        void CreateNewUserModels(List<User_ModelModel> userModels);


    }
}
