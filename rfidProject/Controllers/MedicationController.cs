
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
                    DataSet deletedRoom = executeSQL("SELECT * FROM medicine_info WHERE m_id = " + (char)39 + id + (char)39);

                    int records = deletedRoom.Tables[0].Rows.Count;
                    if (records == 0) {
                        HttpContext.Response.StatusCode = 404;
                        return NotFound("No medication found with id  " + id);
                    }
                    executeSQL("SET FOREIGN_KEY_CHECKS=OFF;");
                    executeSQL("DELETE FROM medicine_info WHERE m_id = " + (char)39 + id + (char)39);
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


        // Add record from the database by medication id
        [HttpPost]
        [Route("medications/")]
        public IActionResult addMed([FromBody] Medication med )
        {
            try {

                    // executeSQL("SET FOREIGN_KEY_CHECKS=OFF;"); //disabling foreign key
                    Random random = new Random();
                    int id = random.Next(0, 1000);
                    string sqlQuery = "INSERT INTO medicine_info VALUES ( " + 
                                                                            (char)39 + id + (char)39 + "," +
                                                                            (char)39 + med.name + (char)39 + "," +
                                                                            (char)39 + med.rfid + (char)39 + 
                                                                            ");";
                    executeSQL(sqlQuery);
                    // executeSQL("SET FOREIGN_KEY_CHECKS=ON;"); 
                    HttpContext.Response.StatusCode = 200;
                    var status = "Medication added successfully !!";
                    
                    return Ok(status);

            } catch (Exception e) {
                HttpContext.Response.StatusCode = 400;
                
                return Problem(
                    detail: e.StackTrace,
                    title: e.Message);
            }
            
        } 




