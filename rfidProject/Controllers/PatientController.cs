

using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace project.Controllers{

 public class PatientController : ControllerBase{

        // Connects to database and execute an SQL statement for retrieving the data for Countries
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public DataSet executeSQL (string sqlStatement)
        {

            string connStr = "server=localhost; port=3306; user=root; password=LSWPMGy825mv1u; database=Medical_Surgeries";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(sqlStatement,conn);
            DataSet myResultSet = new DataSet();
            sqlAdapter.Fill(myResultSet,"patient_info");
            return myResultSet;
        }


        // Get list of all staff member
        [HttpGet]
        [Route("patients/")]
        public IActionResult getAllPatient()
        {
            DataSet allPatient = executeSQL("SELECT * FROM patient_info");
            return Ok(allPatient);
        }

        // Delete an existing record from the database by patient id
        [HttpDelete]
        [Route("patients/{id}")]
        public IActionResult deletePatient(int id )
        {
            try {
                    DataSet deletedInstruments = executeSQL("SELECT * FROM patient_info WHERE p_id = " + (char)39 + id + (char)39);

                    int records = deletedInstruments.Tables[0].Rows.Count;
                    if (records == 0) {
                        HttpContext.Response.StatusCode = 404;
                        return NotFound("No Patient found with id  " + id);
                    }
                    executeSQL("SET FOREIGN_KEY_CHECKS=OFF;");
                    executeSQL("DELETE FROM patient_info WHERE p_id = " + (char)39 + id + (char)39);
                    executeSQL("SET FOREIGN_KEY_CHECKS=ON;");
                    HttpContext.Response.StatusCode = 200;
                    var status = "Patient with Id " + id + " Deleted successfully !!";
                    
                    return Ok(status);

            } catch (Exception e) {
                HttpContext.Response.StatusCode = 400;
                
                return Problem(
                    detail: e.StackTrace,
                    title: e.Message);
            }
            
        } 

        // Add record from the database by patient id
        [HttpPost]
        [Route("patients/")]
        public IActionResult addPatient([FromBody] Patient patient )
        {
            try {

                    // executeSQL("SET FOREIGN_KEY_CHECKS=OFF;"); //disabling foreign key
                    Random random = new Random();
                    int id = random.Next(0, 1000);
                    string sqlQuery = "INSERT INTO patient_info VALUES ( " + 
                                                                            (char)39 + id + (char)39 + "," +
                                                                            (char)39 + patient.name + (char)39 + "," +
                                                                            (char)39 + patient.rfid + (char)39 + 
                                                                            ");";
                    executeSQL(sqlQuery);
                    // executeSQL("SET FOREIGN_KEY_CHECKS=ON;"); 
                    HttpContext.Response.StatusCode = 200;
                    var status = "Patient added successfully !!";
                    
                    return Ok(status);

            } catch (Exception e) {
                HttpContext.Response.StatusCode = 400;
                
                return Problem(
                    detail: e.StackTrace,
                    title: e.Message);
            }
            
        } 


       
    }

}