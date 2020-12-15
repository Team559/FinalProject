

using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace project.Controllers{

 public class LoginController : ControllerBase{

        // Connects to database and execute an SQL statement for retrieving the data for Countries
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public DataSet executeSQL (string sqlStatement)
        {

            string connStr = "server=localhost; port=3306; user=root; password=LSWPMGy825mv1u; database=Medical_Surgeries";
            MySqlConnection conn = new MySqlConnection(connStr);;
            MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(sqlStatement,conn);
            DataSet myResultSet = new DataSet();
            sqlAdapter.Fill(myResultSet,"staff_info");
            return myResultSet;
        }


        // Get list of all instruments
        [HttpPost]
        [Route("authenticate/")]
        public IActionResult authnticateUser([FromBody] Staff loginUser)
        {
            DataSet user = executeSQL("SELECT * FROM staff_info WHERE st_userid = " + (char)39 + loginUser.userid + (char)39 + "AND st_password = " + (char)39 + loginUser.password + (char)39);

            int records = user.Tables[0].Rows.Count;
            if (records == 0) {
                HttpContext.Response.StatusCode = 404;
                return NotFound("Please enter valid user credentials");
            }
            return Ok("validated");
        }

        

    }

}