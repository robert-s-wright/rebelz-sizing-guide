using Domain.Models;

namespace Application.DataAccess
{
    public interface IDataConnection
    {

        List<BrandModel> GetBrands_All();

        void AddNewBrand(BrandModel brand);

        List<ModelModel> GetModels_All();

        void AddNewModel(ModelModel newModel);

        List<SizeModel> GetSizes_All();

        void AddNewSize(SizeModel size);

        List<Model_SizeModel> GetModel_Sizes_All();

        void AddModelSizeModels(List<Model_SizeModel> models);

        List<MeasurementModel> GetMeasurements_All();

        List<UserModel> GetUsers_All();

        UserModel GetUser_ByEmail(UserModel user);

        List<User_ModelModel> GetUserModels_ByUserId(int? userId);

        void UpdateUser(UserModel user);

        void RegisterUser(UserModel user);

        void UpdateUserPassword(int userId, string password);

        void CreateNewUserMeasurements(UserSubmissionsModel user);

        void CreateNewUserModels(List<User_ModelModel> userModels);

        void CreateNewUserRecovery(User_RecoveryModel recoveryModel);

        User_RecoveryModel GetUserRecoveryModel(int userId);


    }
}
