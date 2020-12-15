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
            sqlAdapter.Fill(myResultSet,"surgeryRoom_info");
            return myResultSet;
        }

        // Get list of all surgery rooms
        [HttpGet]
        [Route("surgeryRooms/")]
        public IActionResult getAllSurgRooms()
        {
            DataSet allSurgeryRooms = executeSQL("SELECT * FROM surgeryRoom_info");
            return Ok(allSurgeryRooms);
        }

        //get a list of scheduled/complete/cancelled surgeries
        [HttpGet]
        [Route("surgeryRooms/progress/{progress}")]
        public IActionResult getProgress(String progress) {
            try {
                DataSet surgeries = executeSQL("SELECT * FROM surgery_info WHERE s_progress = " + (char)39 + progress + (char)39);

                //TODO return the dataset of surgeries.

            } catch (Exception e) {
                HttpContext.Response.StatusCode = 400;
                
                return Problem(
                    detail: e.StackTrace,
                    title: e.Message);
            }
        }

        // Return the doctor who performed the surgery
        [HttpGet]
        [Route("surgeryRooms/staff/{surgery_id}")]
        public IActionResult getSurgeryDocter(int surgery_id) {
            try {
                DataSet surgery = executeSQL("SELECT * FROM surgery_info WHERE s_id = " + (char)39 + surgery_id + (char)39);

                //TODO get the doctor who performed the surgery
                return Ok(surgery.doctor) //incorrect


            } catch (Exception e) {
                HttpContext.Response.StatusCode = 400;
                
                return Problem(
                    detail: e.StackTrace,
                    title: e.Message);
            }
        }




        // Delete an existing record from the database by room id
        [HttpDelete]
        [Route("surgeryRooms/{id}")]
        public IActionResult deleteSurgeryRoom(int id )
        {
            try {
                    DataSet deletedRoom = executeSQL("SELECT * FROM surgeryRoom_info WHERE room_id = " + (char)39 + id + (char)39);

                    int records = deletedRoom.Tables[0].Rows.Count;
                    if (records == 0) {
                        HttpContext.Response.StatusCode = 404;
                        return NotFound("No room found with id  " + id);
                    }
                    executeSQL("SET FOREIGN_KEY_CHECKS=OFF;");
                    executeSQL("DELETE FROM surgeryRoom_info WHERE room_id = " + (char)39 + id + (char)39);
                    executeSQL("SET FOREIGN_KEY_CHECKS=ON;");
                    HttpContext.Response.StatusCode = 200;
                    var status = "Surgery room with Id " + id + " Deleted successfully !!";
                    
                    return Ok(status);

            } catch (Exception e) {
                HttpContext.Response.StatusCode = 400;
                
                return Problem(
                    detail: e.StackTrace,
                    title: e.Message);
            }
        } 



        // Add record from the database by surgeryRooms id
        [HttpPost]
        [Route("surgeryRooms/")]
        public IActionResult addRoom([FromBody] SurgeryRoom room )
        {
            try {

                    // executeSQL("SET FOREIGN_KEY_CHECKS=OFF;"); //disabling foreign key
                    Random random = new Random();
                    int id = random.Next(0, 1000);
                    string sqlQuery = "INSERT INTO medicine_info VALUES ( " + 
                                                                            (char)39 + id + (char)39 + "," +
                                                                            (char)39 + room.name + (char)39 + "," +
                                                                            (char)39 + room.rfid + (char)39 + 
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







