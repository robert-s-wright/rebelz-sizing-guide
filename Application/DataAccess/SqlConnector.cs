using Dapper;
using Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Application.DataAccess
{
    //Todo - internal or public?
    internal class SqlConnector : IDataConnection
    {
        private const string db = "GiData";
        public List<BrandModel> GetBrands_All()
        {

            List<BrandModel> output = new List<BrandModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<BrandModel>("dbo.spBrands_GetAll").ToList();
            }
            return output;

        }
        public List<ModelModel> GetModels_All()
        {
            List<ModelModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<ModelModel>("dbo.spModels_GetAll").ToList();
            }
            return output;
        }
        public List<SizeModel> GetSizes_All()
        {
            List<SizeModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<SizeModel>("dbo.spSizes_GetAll").ToList();
            }
            return output;
        }

        public List<Model_SizeModel> GetModel_Sizes_All()
        {
            List<Model_SizeModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<Model_SizeModel>("dbo.spModel_Sizes_GetAll").ToList();
            }
            return output;
        }

        public List<MeasurementModel> GetMeasurements_All()
        {
            List<MeasurementModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<MeasurementModel>("dbo.spMeasurements_GetAll").ToList();
            }
            return output;
        }


        public List<UserModel> GetUsers_All()
        {
            List<UserModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<UserModel>("dbo.spUsers_GetAll").ToList();
            }
            return output;
        }

        public UserModel GetUser_ByEmail(UserModel user)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@email", user.Email);

                UserModel output = connection.QueryFirstOrDefault<UserModel>("dbo.spUsers_GetByEmail2", p, commandType: CommandType.StoredProcedure);

                if (output != null)
                {
                    p = new DynamicParameters();
                    p.Add("@userId", output.Id);

                    output.UserMeasurements = connection.Query<User_MeasurementModel>("dbo.spUser_Measurements_GetByUserId", p, commandType: CommandType.StoredProcedure).ToList();

                    if (output.UserMeasurements.Count == 0)
                    {
                        output.UserMeasurements.Add(new User_MeasurementModel());
                        output.UserMeasurements[0].UserId = output.Id;
                    }
                    else
                    {
                        User_MeasurementModel newMeasurementModel = (User_MeasurementModel)output.UserMeasurements[output.UserMeasurements.Count - 1].Clone();
                        newMeasurementModel.Id = null;
                        newMeasurementModel.Date = null;
                        output.UserMeasurements.Add(newMeasurementModel);
                    }

                    return output;
                }

                else
                {
                    return user;

                }
            }
        }

        public List<User_ModelModel> GetUserModels_ByUserId(int? userId)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@userId", userId);

                List<User_ModelModel> userModels = connection.Query<User_ModelModel>("dbo.spUser_Models_GetByUserId", p, commandType: CommandType.StoredProcedure).ToList();



                return userModels;

            }
        }

        public void RegisterUser(UserModel user)
        {

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {

                var p = new DynamicParameters();

                p.Add("@email", user.Email);
                p.Add("@password", user.Password);


                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spUsers_Register", p, commandType: CommandType.StoredProcedure);

                user.Id = p.Get<int>("@id");
                foreach (User_MeasurementModel measurement in user.UserMeasurements)
                {
                    measurement.UserId = p.Get<int>("@id");
                }

            }

        }

        public void UpdateUser(UserModel user)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@id", user.Id);
                p.Add("@name", user.Name);

                connection.Execute("dbo.spUsers_Update", p, commandType: CommandType.StoredProcedure);

            }
        }

        public void CreateNewUserMeasurements(UserSubmissionsModel user)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                foreach (User_MeasurementModel measurement in user.User.UserMeasurements)
                {

                    var p = new DynamicParameters();
                    p.Add("@id", measurement.Id);
                    User_MeasurementModel existing = connection.QuerySingleOrDefault<User_MeasurementModel>("dbo.spUser_Measurements_GetById", p, commandType: CommandType.StoredProcedure);

                    if (existing == null)
                    {
                        p = new DynamicParameters();
                        p.Add("@userId", measurement.UserId);
                        p.Add("@height", measurement.Height);
                        p.Add("@weight", measurement.Weight);
                        p.Add("@arm", measurement.Arm);
                        p.Add("@shoulders", measurement.Shoulders);
                        p.Add("@chest", measurement.Chest);
                        p.Add("@belly", measurement.Belly);
                        p.Add("@waist", measurement.Waist);
                        p.Add("@seat", measurement.Seat);
                        p.Add("@thigh", measurement.Thigh);

                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                        p.Add("@date", 0, dbType: DbType.DateTime, direction: ParameterDirection.Output);

                        connection.Execute("dbo.spUser_Measurements_Insert", p, commandType: CommandType.StoredProcedure);

                        measurement.Id = p.Get<int>("@id");
                        measurement.Date = p.Get<DateTime>("date");

                        foreach (User_ModelModel model in user.UserModels)
                        {
                            if (model.UserMeasurementId == null)
                            {
                                model.UserMeasurementId = measurement.Id;
                            }
                        }

                    }
                }
            }
        }

        public void CreateNewUserModels(List<User_ModelModel> userModels)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {


                foreach (User_ModelModel userModel in userModels)
                {
                    if (userModel.Id == null)
                    {
                        var p = new DynamicParameters();
                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                        p.Add("@brandId", userModel.BrandId);
                        p.Add("userId", userModel.UserId);
                        p.Add("@userMeasurementId", userModel.UserMeasurementId);
                        p.Add("@modelId", userModel.ModelId);
                        p.Add("@sizeId", userModel.SizeId);
                        p.Add("@comments", userModel.Comments);

                        connection.Execute("dbo.spUser_Models_Insert", p, commandType: CommandType.StoredProcedure);

                        userModel.Id = p.Get<int>("@id");

                    }
                }

            }
        }
    }
}
