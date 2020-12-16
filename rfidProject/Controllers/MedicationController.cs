

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

using project.Models;


 namespace project.Controllers{

  public class MedicationController : ControllerBase{

         // Connects to database and execute an SQL statement for retrieving the data for Countries
         [ApiExplorerSettings(IgnoreApi = true)]
         [NonAction]
         public DataSet executeSQL (string sqlStatement)
         {
             string connStr = "server=localhost; port=3306; user=root; password=LSWPMGy825mv1u; database=Medical_Surgeries";
             MySqlConnection conn = new MySqlConnection(connStr);;
             MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(sqlStatement,conn);
             DataSet myResultSet = new DataSet();
             sqlAdapter.Fill(myResultSet,"medicine_info");
             return myResultSet;
         }


         // Get list of all medications
         [HttpGet]
         [Route("medications/")]
         public IActionResult getAllMeds()
         {
             DataSet allMeds = executeSQL("SELECT * FROM medicine_info");
             return Ok(allMeds);
         }

         // Delete an existing record from the database by medication id
         [HttpDelete]
         [Route("medications/{id}")]
         public IActionResult deleteMedication(int id )
         {
             try {
                     DataSet deletedMed = executeSQL("SELECT * FROM medicine_info WHERE m_rfid = " + (char)39 + id + (char)39);

                     int records = deletedMed.Tables[0].Rows.Count;
                     if (records == 0) {
                         HttpContext.Response.StatusCode = 404;
                         return NotFound("No medication found with id  " + id);
                     }
                     executeSQL("SET FOREIGN_KEY_CHECKS=OFF;");
                     executeSQL("DELETE FROM medicine_info WHERE m_rfid = " + (char)39 + id + (char)39);
                     executeSQL("SET FOREIGN_KEY_CHECKS=ON;");
                     HttpContext.Response.StatusCode = 200;
                     var status = "Medication with Id " + id + " Deleted successfully !!";
                    
                     return Ok(status);

             } catch (Exception e) {
                 HttpContext.Response.StatusCode = 400;
                
                 return Problem(
                     detail: e.StackTrace,
                     title: e.Message);
             }
         }

         [HttpGet]
         [Route("medications/given/{patient_id}")]
         public IActionResult getMedsGiven(string patient_id) {
             try {
                 DataSet med = executeSQL("SELECT * FROM medicine_info WHERE given_to LIKE " + (char)39 + "%"+ patient_id +"%"+ (char)39);

                 return Ok(med);

             } catch (Exception e) {
                 HttpContext.Response.StatusCode = 400;
                
                 return Problem(
                     detail: e.StackTrace,
                     title: e.Message);
             }
         }


         // Add record from the database by medication id
         // use NULL for accessed/given/used when the med has not been used.
         [HttpPut]
         [Route("medications/{id}")]
         public IActionResult upadteMedicine(string id )
         {
             try {
                // executeSQL("SET FOREIGN_KEY_CHECKS=OFF;"); //disabling foreign key

                string sqlQuery = "UPDATE medicine_info SET m_status = 0 WHERE m_rfid =  " + (char)39 + id + (char)39;
                executeSQL(sqlQuery);

                DataSet updatedInstruments = executeSQL("SELECT * FROM medicine_info WHERE m_rfid =  " + (char)39 + id + (char)39);

                int records = updatedInstruments.Tables[0].Rows.Count;
                if (records == 0) {
                    HttpContext.Response.StatusCode = 404;
                    return NotFound("No Medication found with RFID  " + id);
                }

                HttpContext.Response.StatusCode = 200;
                var status = "Medication with RFID " + id + " Updated successfully !!";
                
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




//  //get the nurse who accessed a medicine by the medicines id
        //  [HttpGet]
        //  [Route("medications/accessed/{rfid}")]
        //  public IActionResult getAccessed(int rfid) {
        //      try {
        //          DataSet med = executeSQL("SELECT * FROM medicine_info WHERE m_rfid = " + (char)39 + rfid + (char)39);

        //          return Ok(med);

        //      } catch (Exception e) {
        //          HttpContext.Response.StatusCode = 400;
                
        //          return Problem(
        //              detail: e.StackTrace,
        //              title: e.Message);
        //      }
        //  }



