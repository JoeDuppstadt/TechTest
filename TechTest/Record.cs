using System;

namespace TechTest
{
	public class Record
	{
		public Record(int id, string name, string description, int releaseyear)
		{
			ID = id;
			Name = name;
			Description = description;
			ReleaseYear = releaseyear;
		}

		public Record(string name, string description, int releaseyear)
		{
			ID = null;
			Name = name;
			Description = description;
			ReleaseYear = releaseyear;
		}

		public Record(int id)
		{
			ID = id;
		}

		public Record()
		{

		}


		public int? ID { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public int ReleaseYear { get; set; }

		public override string ToString()
		{
			return "ID: " + ID + " Name: " + Name + " Description: " + Description + " ReleaseYear: " + ReleaseYear.ToString();
		}
	}
}
