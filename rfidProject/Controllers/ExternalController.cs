

using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc; 


namespace project.Controllers{

 public class ExternalController : ControllerBase{

        [HttpGet]
        [Route("/external/{name}")]
         public IActionResult getMedicineDetails(string name) {


                string userHeader  = Request.Headers["user"];

                if (userHeader == null || userHeader.Length == 0 || (userHeader.Length != 0 && !String.Equals(userHeader, "admin")))
                {
                    return Unauthorized();
                }
               
                var serviceURL =  "https://iterar-mapi-us.p.rapidapi.com/api/" + name + "/substances.json";

                // prepare the HTTP request 
                WebRequest serviceRequest = (WebRequest)WebRequest.Create(serviceURL); 

                // identify the method of HTTP request as GET 
                serviceRequest.Method = "GET"; 
                serviceRequest.ContentLength = 0; 
                serviceRequest.ContentType = "application/json";

                // api key
                serviceRequest.Headers.Add("x-rapidapi-key", "52b2d8d9d4msh1cac7ef7864ac40p17eb41jsn697343616697");
                serviceRequest.Headers.Add("x-rapidapi-host", "iterar-mapi-us.p.rapidapi.com" );

                // establish a connection and retrieve a HTTP response message 
                WebResponse serviceResponse = (WebResponse)serviceRequest.GetResponse();

                // read response data stream 
                Stream receiveStream = serviceResponse.GetResponseStream();

                // properly set the encoding as utf-8 
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8"); 

                // encode the stream using utf-8 
                StreamReader readStream = new StreamReader(receiveStream, encode, true); 

                // read entire stream and store in serviceResult 
                string serviceResult = readStream.ReadToEnd(); 
               
                // return serviceResult

                return Ok(serviceResult);
            }


    }

}