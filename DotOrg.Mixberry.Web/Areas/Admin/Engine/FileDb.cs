using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotOrg.Mixberry.Web.Areas.Admin.Engine;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using DotOrg.Mixberry.Web.Areas.Admin.Models;

namespace DotOrg.Mixberry.Web.Areas.Admin.Engine
{
    public class DbInter
    {
        public bool IsActive()
        {
            bool isActive = false;
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("SELECT [IsActiveImgFor] FROM[u456186].[dbo].[IsActiveImgHome]");
            cmd.CommandType = CommandType.Text;

            try
            {
                connection.Open();
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    isActive = Convert.ToBoolean(reader["IsActiveImgFor"]);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //DbDataList.Add(new CostControlCenterItems() { erorr = ex });
            }
            finally
            {
                connection.Close();
            }
            return isActive;
        }

        public int RowsCol()
        {
            int cnt = 20;
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("SELECT count(*) as cnt FROM [u456186].[dbo].[FileBinary]");
            cmd.CommandType = CommandType.Text;

            try
            {
                connection.Open();
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    cnt = Convert.ToInt32(reader["cnt"]);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //DbDataList.Add(new CostControlCenterItems() { erorr = ex });
            }
            finally
            {
                connection.Close();
            }
            return cnt;
        }

        public void UpdateFile(int Id, string Name, string Descrip, int Order)
        {
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("UPDATE_FILE_PARAMS");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FileID", SqlDbType.Int).Value = Id;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar, int.MaxValue).Value = Name;
            cmd.Parameters.Add("@FileDescr", SqlDbType.VarChar, int.MaxValue).Value= Descrip;
            cmd.Parameters.Add("@Order", SqlDbType.Int).Value = Order;
            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            try
            {
                connection.Open();
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void UpLoadFile(string data_table_title, byte[] data_table_content, int GroupId, string Description, int OrderInd)
        {
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("INSERT_FILE");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@file_binary", SqlDbType.VarBinary).Value = data_table_content;
            cmd.Parameters.Add("@file_name", SqlDbType.VarChar, 255).Value = data_table_title;
            cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = GroupId;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar, int.MaxValue).Value = Description;
            cmd.Parameters.Add("@OrderIndex", SqlDbType.Int).Value = OrderInd;
            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            try
            {
                connection.Open();
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public List<FileImg> DownLoadFiles(int GroupId)
        {
            List<FileImg> DbDataList = new List<FileImg>();
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("SELECT_FILES");
            cmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = GroupId;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    DbDataList.Add(new FileImg()
                    {
                        TitleFile = Convert.ToString(reader["FileName"]),
                        id = Convert.ToString(reader["id"]),
                        Description = Convert.ToString(reader["FileDescription"]),
                        GroupName = Convert.ToString(reader["GroupName"]),
                        order = Convert.ToInt32(reader["Order"])
                    });
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //DbDataList.Add(new CostControlCenterItems() { erorr = ex });
            }
            finally
            {
                connection.Close();
            }
            return DbDataList;
        }

        public FileImg DownLoadFileinfo(int FileID)
        {
            FileImg DbDataList = new FileImg();
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("SELECT_FILE_INFO");
            cmd.Parameters.Add("@FileId", SqlDbType.Int).Value = FileID;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    DbDataList = new FileImg()
                    {
                        TitleFile = Convert.ToString(reader["FileName"]),
                        id = Convert.ToString(reader["id"]),
                        Description = Convert.ToString(reader["FileDescription"]),
                        GroupName = Convert.ToString(reader["GroupName"]),
                        order = Convert.ToInt32(reader["Order"])
                    };
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //DbDataList.Add(new CostControlCenterItems() { erorr = ex });
            }
            finally
            {
                connection.Close();
            }
            return DbDataList;
        }

        public List<FileGroups> GetGroups()
        {
            List<FileGroups> DbDataList = new List<FileGroups>();
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("GET_GROUPS");
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    DbDataList.Add(new FileGroups()
                    {
                        GroupId = Convert.ToInt32(reader["GroupId"]),
                        Description = Convert.ToString(reader["Description"])
                    });
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //DbDataList.Add(new CostControlCenterItems() { erorr = ex });
            }
            finally
            {
                connection.Close();
            }
            return DbDataList;
        }

        public FileImg DeleteFile(int FileId)
        {
            FileImg DbDataList = new FileImg();
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("DELETE_FILE");
            cmd.Parameters.Add("@FileID", SqlDbType.Int).Value = FileId;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //DbDataList.Add(new CostControlCenterItems() { erorr = ex });
            }
            finally
            {
                connection.Close();
            }
            return DbDataList;

        }

        public FileImg DownLoadFile(int FileId)
        {
            FileImg DbDataList = new FileImg();
            SqlConnection connection = DbConnection.GetConnection;
            SqlCommand cmd = new SqlCommand("SELECT_FILE");
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = FileId;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DbDataList.content = (byte[])(reader["binary"]);

                    DbDataList.TitleFile = Convert.ToString(reader["FileName"]);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //DbDataList.Add(new CostControlCenterItems() { erorr = ex });
            }
            finally
            {
                connection.Close();
            }
            return DbDataList;

        }

    }

    public class FileReader
    {
        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}