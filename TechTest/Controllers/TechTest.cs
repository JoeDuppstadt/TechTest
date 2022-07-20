using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TechTest.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TechTest : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<TechTest> _logger;

		public TechTest(ILogger<TechTest> logger)
		{
			_logger = logger;
		}

		/*
		 *  https://localhost:44305/TechTest
		 */

		[HttpGet]
		public List<Record> Get()
		{
			return dbManager.GetInstance().getRecords();
		}


		/*
		*  https://localhost:44305/TechTest/{idNum}
		*/

		[HttpGet("{id}")]
		public Record Get1Record(int id)
        {
			return dbManager.GetInstance().getOneRecord(id);
        }
		


		/*
		 * https://localhost:44305/TechTest
		 * {"id":"1","name":"TEST","description":"THIS IS A TEST","releaseYear":2000} 
		 */

		[HttpPost]
		public void Post([FromBody] Record item)
		{
			Random rnd = new Random();
			item.ID = rnd.Next(1, 2147483647); // generate random ID. I would have used a guid but it had to be an int value
			dbManager.GetInstance().addRow(item);
		}

		/*
		 * {"id":2}
		 */

		[HttpDelete]
		public void Delete([FromBody] Record item)
        {
			dbManager.GetInstance().deleteRow(item);
        }

		[HttpPut]
		public void Put([FromBody]Record item)
        {
			dbManager.GetInstance().editRow(item);
        }


	}
}
