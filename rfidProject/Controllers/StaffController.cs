

using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Models;

namespace project.Controllers {

 public class StaffController : ControllerBase{

        // Connects to database and execute an SQL statement for retrieving the data for Countries
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public DataSet executeSQL (string sqlStatement)
        {

            string connStr = "server=localhost; port=3306; user=root; password=LSWPMGy825mv1u; database=Medical_Surgeries";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(sqlStatement,conn);
            DataSet myResultSet = new DataSet();
            sqlAdapter.Fill(myResultSet,"staff_info");
            return myResultSet;
        }


        // Get list of all staff member
        [HttpGet]
        [Route("staff/")]
        public IActionResult getAllStaff()
        {
            DataSet allStaff = executeSQL("SELECT * FROM staff_info");
            return Ok(allStaff);
        }

        // Get list of staff members by type doctor or nurse
        [HttpGet]
        [Route("staff/type/{type}/")]
        public IActionResult getStaffByType(string type)
        {
            
            DataSet allStaff = executeSQL("SELECT * FROM staff_info WHERE st_type = " + (char)39 + type + (char)39);
            return Ok(allStaff);
        }

        // Get list of all serailized instruments
        [HttpGet]
        [Route("staff/name/{name}/")]
        public IActionResult getStaffByName(string name)
        {
            DataSet allStaff = executeSQL("SELECT * FROM staff_info WHERE st_name LIKE " + (char)39 + "%" + name + "%" + (char)39);
            return Ok(allStaff);
        }

        // Get list of all serailized instruments
        [HttpGet]
        [Route("staff/dept/{name}/")]
        public IActionResult getDeptByName(string name)
        {
            DataSet allStaff = executeSQL("SELECT * FROM staff_info WHERE st_dept LIKE " + (char)39 + "%" + name + "%" + (char)39);
            return Ok(allStaff);
        }
          
    }
}