using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TechTest.Controllers
{
	public class dbManager
	{
		private static readonly object Lock = new object();
		private static dbManager db = null;
		SqlConnection cnn = null;

		private dbManager()
		{
			string connectionString = "Server = localhost; Database = MotionPictures; Trusted_Connection = True;";
			cnn = new SqlConnection(connectionString);

			try
			{
				cnn.Open();
				System.Diagnostics.Debug.WriteLine("Connected to db successfully");
				//cnn.Close();
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine("Failed to connect to db");
			}
		}
		
		// I created this data manager instance because I only want one db connection open. There is no need to have more
		public static dbManager GetInstance()
		{
			if (db == null)
			{
				lock (Lock)
				{
					if (db == null)
					{
						db = new dbManager();
					}
				}
			}
			return db;

		}

		public List<Record> getRecords()
		{
			List<Record> records = null;
			if (checkdbConnection(cnn)) // if the connection is still open
			{
				string sql = "Select * From MotionPictures";
				SqlCommand command = new SqlCommand(sql, cnn);
				SqlDataReader dataReader = command.ExecuteReader();
				records = new List<Record>();
				while (dataReader.Read())
				{                               // ID							name										desc							year	
					records.Add(new Record(Int32.Parse(dataReader.GetValue(0).ToString()), dataReader.GetValue(1).ToString(), dataReader.GetValue(2).ToString(), Int32.Parse(dataReader.GetValue(3).ToString())));
				}
				dataReader.Close();
				command.Dispose();
				return records;
			}
			else return records; // return null list if the db cannot reconnect
		}

		public Record getOneRecord(int id)
        {
			string sql = "Select * from MotionPictures Where ID = @id";
			SqlCommand command = new SqlCommand(sql, cnn);
			command.Parameters.AddWithValue("@id", id);
			SqlDataReader dataReader = command.ExecuteReader();
			Record x = null;
			while (dataReader.Read())
			{
				x = new Record(Int32.Parse(dataReader.GetValue(0).ToString()), dataReader.GetValue(1).ToString(), dataReader.GetValue(2).ToString(), Int32.Parse(dataReader.GetValue(3).ToString()));
			}
			dataReader.Close();
			command.Dispose();
			return x;
		
		}

		public void addRow(Record record)
		{
			if (checkdbConnection(cnn))
			{
				string sql = "INSERT INTO MotionPictures VALUES(@ID, @NAME, @DESC, @RELEASEYEAR)";
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.AddWithValue("@ID", record.ID);
				command.Parameters.AddWithValue("@NAME", record.Name);
				command.Parameters.AddWithValue("@DESC", record.Description);
				command.Parameters.AddWithValue("@RELEASEYEAR", record.ReleaseYear);
				command.ExecuteNonQuery();
				command.Dispose();
			}
		}

		public void deleteRow(Record record)
        {
			if (checkdbConnection(cnn))
			{
				string sql = "DELETE FROM MotionPictures WHERE ID = @id";
				SqlCommand command = new SqlCommand(sql, cnn);
				command.Parameters.AddWithValue("@id", record.ID);
				command.ExecuteNonQuery();
				command.Dispose();
			}
		}

		public void editRow(Record record)
        {
			string sql = "UPDATE MotionPictures SET Name = @NAME, Description = @DESC, ReleaseYear = @YEAR WHERE ID = @ID";
			SqlCommand command = new SqlCommand(sql, cnn);
			command.Parameters.AddWithValue("@ID", record.ID);
			command.Parameters.AddWithValue("@NAME", record.Name);
			command.Parameters.AddWithValue("@DESC", record.Description);
			command.Parameters.AddWithValue("@YEAR", record.ReleaseYear);
			command.ExecuteNonQuery();
			command.Dispose();
		}


		private bool checkdbConnection(SqlConnection cnn)
		{
			if (cnn != null && cnn.State == System.Data.ConnectionState.Closed)
			{
				// lost connection, open a new one
				try
				{
					System.Diagnostics.Debug.WriteLine("Connected to db");
					cnn.Open();
					return true;
				}
				catch
				{
					System.Diagnostics.Debug.WriteLine("Failed to connect to db");
					return false;
				}

			}
			else return true;
		}

	}
}
