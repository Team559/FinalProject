
 

using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;
using System.Xml;
using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Models;

using project.Models;
 
 namespace project.Controllers{

  public class SurgeryController : ControllerBase{

         // Connects to database and execute an SQL statement for retrieving the data for Countries
         [ApiExplorerSettings(IgnoreApi = true)]
         [NonAction]
         public DataSet executeSQL (string sqlStatement)
         {
             string connStr = "server=localhost; port=3306; user=root; password=LSWPMGy825mv1u; database=Medical_Surgeries";
             MySqlConnection conn = new MySqlConnection(connStr);;
             MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(sqlStatement,conn);
             DataSet myResultSet = new DataSet();
             sqlAdapter.Fill(myResultSet,"surgery_info");
             return myResultSet;
         }

         // Get list of all surgeries
         [HttpGet]
         [Route("surgeries/")]
         public IActionResult getAllSurgies()
         {
             DataSet allSurgeryRooms = executeSQL("SELECT * FROM surgery_info");
             return Ok(allSurgeryRooms);
         }

         [HttpGet]
         [Route("surgeries/patient/{name}")]
         public IActionResult getSurgeryByPatient(string name) {
             try {
                 DataSet med = executeSQL("SELECT * FROM surgery_info WHERE patient_name LIKE " + (char)39 + "%"+ name +"%"+ (char)39);
                 return Ok(med);

             } catch (Exception e) {
                 HttpContext.Response.StatusCode = 400;
                
                 return Problem(
                     detail: e.StackTrace,
                     title: e.Message);
             }
         }

         [HttpGet]
         [Route("surgeries/doctor/{name}")]
         [Produces("application/json")]
         public IActionResult getSurgeryByDoctor(string name) {
             try {
                 DataSet med = executeSQL("SELECT * FROM surgery_info WHERE doctor_name LIKE " + (char)39 + "%"+ name +"%"+ (char)39);
                 return Ok(med);

             } catch (Exception e) {
                 HttpContext.Response.StatusCode = 400;
                
                 return Problem(
                     detail: e.StackTrace,
                     title: e.Message);
             }
         }

         [HttpGet]
         [Route("surgeries/doctorXML/{name}")]
         [Produces("application/xml")]
         public IActionResult getSurgeryByDoctorXML(string name) {
             try {
                 DataSet med = executeSQL("SELECT * FROM surgery_info WHERE doctor_name LIKE " + (char)39 + "%"+ name +"%"+ (char)39);

                int records = med.Tables[0].Rows.Count;
                if (records == 0) {
                    HttpContext.Response.StatusCode = 404;
                    return NotFound("No doctor found with name   " + name);
                }
                //else read the row:
                //  DataTable table = med.Tables[0];
                //  foreach (DataRow dr in table.Rows)
                // {
                //     String id =  dr["s_id"].ToString();
                //     String sname = dr["s_name"].ToString();
                //     String doctorname = dr["doctor_name"].ToString();
                //     String patientname = dr["patient_name"].ToString();
                //     String sroom = dr["s_room"].ToString();

                //     StringBuilder sb = new StringBuilder();
                //     XmlWriterSettings settings = new XmlWriterSettings();
                //     settings.Indent = true;
                //     settings.OmitXmlDeclaration = true;
                //     settings.NewLineOnAttributes = true;
                //     XmlWriter writer = XmlWriter.Create(sb, settings);

                //     writer.WriteStartDocument();
                //     writer.WriteStartElement("Staff");

                //     writer.WriteStartElement("Person");
                //     writer.WriteAttributeString("SurgeryName", sname);
                //     writer.WriteAttributeString("ID", id);
                //     writer.WriteAttributeString("DoctorName", doctorname);
                //     writer.WriteAttributeString("PatientNAme", patientname);
                //     writer.WriteAttributeString("SurgeryRoom", sroom);
                //     writer.WriteEndElement();

                //     writer.WriteEndElement();
                //     writer.WriteEndDocument();

                //     writer.Flush(); 

                //     XmlDocument xmlDocument = new XmlDocument();
                //     xmlDocument.LoadXml(sb.ToString());
                    
                //     return new ContentResult{
                //         ContentType = "application/xml",
                //         Content = xmlDocument.ToString(),
                //         StatusCode = 200
                //     };
                // }    
                 
                 return Ok(med);

             } catch (Exception e) {
                 HttpContext.Response.StatusCode = 400;
                
                 return Problem(
                     detail: e.StackTrace,
                     title: e.Message);
             }
         }




         // Delete an existing record from the database by surgery id
         [HttpDelete]
         [Route("surgeries/{id}")]
         public IActionResult deleteMedication(int id )
         {
             try {
                     DataSet deletedRoom = executeSQL("SELECT * FROM surgery_info WHERE s_id = " + (char)39 + id + (char)39);

                     int records = deletedRoom.Tables[0].Rows.Count;
                     if (records == 0) {
                         HttpContext.Response.StatusCode = 404;
                         return NotFound("No Surgery found with id  " + id);
                     }
                     executeSQL("SET FOREIGN_KEY_CHECKS=OFF;");
                     executeSQL("DELETE FROM surgery_info WHERE s_id = " + (char)39 + id + (char)39);
                     executeSQL("SET FOREIGN_KEY_CHECKS=ON;");
                     HttpContext.Response.StatusCode = 200;
                     var status = "Surgery with Id " + id + " Deleted successfully !!";
                    
                     return Ok(status);

             } catch (Exception e) {
                 HttpContext.Response.StatusCode = 400;
                
                 return Problem(
                     detail: e.StackTrace,
                     title: e.Message);
             }
         }

         //Update 
         [HttpPut]
         [Route("surgeries/{id}")]
         public IActionResult upadteSurgery(string id )
         {
             try {

                DataSet updatedInstruments = executeSQL("SELECT * FROM surgery_info WHERE s_id =  " + (char)39 + id + (char)39);

                int records = updatedInstruments.Tables[0].Rows.Count;
                if (records == 0) {
                    HttpContext.Response.StatusCode = 404;
                    return NotFound("No Surgery found with id   " + id);
                } else {
                    string sqlQuery = "UPDATE surgery_info SET s_progress = 'Completed' WHERE s_id =  " + (char)39 + id + (char)39;
                    executeSQL(sqlQuery);
                    HttpContext.Response.StatusCode = 200;
                    var status = "Surgery with id " + id + " Updated successfully !!";
                
                    return Ok(status);
                }

             } catch (Exception e) {
                 HttpContext.Response.StatusCode = 400;
                
                 return Problem(
                     detail: e.StackTrace,
                     title: e.Message);
            }
         } 
    }
 }






