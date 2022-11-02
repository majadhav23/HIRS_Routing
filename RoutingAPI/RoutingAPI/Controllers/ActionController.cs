using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Configuration;
using RoutingAPI.CommonMethods;

namespace RoutingAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ActionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Action to run all methods by redirecting
        /// </summary>
        [HttpGet("runAll")]
        public async Task<ActionResult> RunAllActions()
        {
            Console.WriteLine("Process Started!");
            Console.WriteLine("=================");
            return RedirectToAction("GrabExtractZip");
        }

        /// <summary>
        /// Grabbing the ZIP File: from specified path and extracting
        /// </summary>
        [HttpGet("grabZIP")]
        public async Task<ActionResult<String>> GrabExtractZip()
        {
            string path = _configuration["ZipFilePath"];
            var folderName = Path.Combine("Resources", "uploads");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(path, "routing.zip");
            ZipFile.ExtractToDirectory(fullPath, pathToSave);

            Console.WriteLine("1- routing.zip file grabbed and extracted successfully");
            return RedirectToAction("UpdateTxtFile");
        }

        /// <summary>
        /// Actions on TXT File: removing lines and updating name
        /// </summary>
        [HttpGet("txt")]
        public async Task<ActionResult<String>> UpdateTxtFile()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Resources", "uploads\\"));
            var lines = System.IO.File.ReadAllLines(path  + "routing.txt");

            lines = lines.Take(lines.Length - 3).ToArray();     // removing bottom extra lines
            var newName = "routing " + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
            System.IO.File.WriteAllLines(path + newName, lines.Skip(4).ToArray());      // removing top 4 lines
            System.IO.File.Delete(path + "routing.txt");    // delete old extracted routing.txt file
            
            Console.WriteLine("2- routing.txt file renamed & updated: " + newName);
            return RedirectToAction("ArchiveTable");
        }

        /// <summary>
        /// Actions on .mdb file File: renaming table and archive it
        /// </summary>
        [HttpGet("archive")]
        public async Task<ActionResult<String>> ArchiveTable()
        {
        // RENAMING THE TABLE
            string newTableName = "ZipRouting " + DateTime.Now.ToString("MM-dd-yyyy");
            string qString1 = "SELECT * INTO [" + newTableName + "] FROM ZipRouting";
            string qString2 = "DROP TABLE ZipRouting";
               
            var allQueries = new List<string>(){ qString1, qString2 };

            Common.ExecuteQuery(_configuration, allQueries, "zipRoutings");

        // MOVING TO ZipRoutingArchive.mdb
        // ================================
            //string qStr = "SELECT * INTO [" + newTableName + "] " +
            //    "FROM [MS Access;DATABASE=" + _configuration["ZipRoutingsDB"] + ";].[" + newTableName + "]";

            //Testing
            string qStr = "SELECT * INTO Test1 " +
                    "FROM [MS Access;DATABASE=" + _configuration["ZipRoutingsDB"] + ";].[" + newTableName + "]";

            Common.ExecuteQuery(_configuration, new List<string>() { qStr }, "zipRoutingArchive");

            Console.WriteLine("3- ZipRouting table renamed and archived to ZipRoutingArchive.mdb file");
            return RedirectToAction("DeleteTable");
        }

        /// <summary>
        /// Actions on .mdb file File: Deleting Last week's zip routing table
        /// </summary>
        [HttpGet("delete")]
        public async Task<ActionResult<String>> DeleteTable()
        {
            string newTableName = "ZipRouting " + DateTime.Now.ToString("MM-dd-yyyy");
            string qStr = "DROP TABLE [" + newTableName + "]";
        // Testing   
            //string qStr = "DROP TABLE Test1";            
            Common.ExecuteQuery(_configuration, new List<string>() { qStr }, "zipRoutings");
            
            Console.WriteLine("4- ZipRouting (with today's date) table has been DELETED!");
            return RedirectToAction("ImportTxtToTable");
        }

        /// <summary>
        /// Actions on .mdb file File: Importing new zip routing text file to DB and also reformating the columns
        /// </summary>
        [HttpGet("import/txt")]
        public async Task<ActionResult<String>> ImportTxtToTable()
        {
            // Creating Table ZipRouting (testing with "Test1" name, later change to ZipRouting) to import txt file
            string createTableQuery = "SELECT * INTO ZipRouting " +
                                "FROM ZipRoutings_Backup WHERE [ID]='0'";
            string resizeQuery = "ALTER TABLE ZipRouting ALTER COLUMN Distance TEXT(54)";
            string resizeQuery1 = "ALTER TABLE ZipRouting ALTER COLUMN Rank TEXT(1)";
            string resizeQuery2 = "ALTER TABLE ZipRouting ALTER COLUMN ID IDENTITY NOT NULL";
            
            var allQueries = new List<string>() { createTableQuery, resizeQuery, resizeQuery1, resizeQuery2 };
            Common.ExecuteQuery(_configuration, allQueries, "zipRoutings");

            OleDbConnection conn = new OleDbConnection(_configuration.GetConnectionString("zipRoutings"));
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("INSERT into [ZipRouting] ([Location], [Zip Code], [Distance], [Rank]) " +
                                                "values(@location, @zip, @distance, @rank)", conn);
            cmd.Parameters.Add("@location", OleDbType.VarWChar);
            cmd.Parameters.Add("@zip", OleDbType.VarWChar);
            cmd.Parameters.Add("@distance", OleDbType.VarWChar);
            cmd.Parameters.Add("@rank", OleDbType.VarWChar);

            string[] values;
            foreach (string line in System.IO.File.ReadLines(_configuration["NewTextFilePath"]+ DateTime.Now.ToString("MM-dd-yyyy") +".txt"))
            {
                values = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 6)
                {
                    cmd.Parameters["@zip"].Value = values[2];
                    cmd.Parameters["@location"].Value = values[3];
                    cmd.Parameters["@distance"].Value = values[4];
                    cmd.Parameters["@rank"].Value = values[5];
                } 
                else if (values.Length == 5)
                {
                    cmd.Parameters["@zip"].Value = values[1];
                    cmd.Parameters["@location"].Value = values[2];
                    cmd.Parameters["@distance"].Value = values[3];
                    cmd.Parameters["@rank"].Value = values[4];
                }
                else if (values.Length == 7)
                {
                    cmd.Parameters["@zip"].Value = values[2] + " " + values[3];
                    cmd.Parameters["@location"].Value = values[4];
                    cmd.Parameters["@distance"].Value = values[5];
                    cmd.Parameters["@rank"].Value = values[6];
                }
                cmd.ExecuteNonQuery();
            }
            conn.Close();
            Console.WriteLine("5- new routing(today's date).txt imported into ZipRoutings.mdb and columns are formatted as per requirement.");
            return RedirectToAction("AppendToTable");
        }

        /// <summary>
        /// Actions on .mdb file File: Appending Licensee Zips to ZipRouting Table
        /// </summary>
        [HttpGet("append/licensee")]
        public async Task<ActionResult<String>> AppendToTable()
        {        
            string qStr = "INSERT INTO ZipRouting ([Location], [Zip Code], [Rank], [Distance]) " +
                "SELECT [Location], [ZipCode], [Rank], [Distance] FROM LicenseeZips";
            Common.ExecuteQuery(_configuration, new List<string>() { qStr }, "zipRoutings");

            Console.WriteLine("6- LicenseeZips appended successfully to ZipRouting table");
            return RedirectToAction("CopyDatabase");
        }

        /// <summary>
        /// Copying ZipRoutings.mdb file to other directory
        /// </summary>
        [HttpGet("copy/db")]
        public async Task<ActionResult<String>> CopyDatabase()
        {
            System.IO.File.Copy(_configuration["ZipRoutingsDB"],_configuration["DBCopyToPath"], true);
            Console.WriteLine("7- ZipRoutings.mdb file copied to mentioned directory.");
            return RedirectToAction("CreateTableBackup");
        }

        /// <summary>
        /// Create new Backup of ZipRouting table under PhoneRoutings.mdb
        /// </summary>
        [HttpGet("backup/table")]
        public async Task<ActionResult<String>> CreateTableBackup()
        {
            // Deleting ZipRoutings_Backup Table and renaming ZipRoutings to ZipRoutings_Backup
            string qStr1 = "DROP TABLE ZipRoutings_Backup";
            string qStr2 = "SELECT * INTO ZipRoutings_Backup FROM ZipRoutings";
            string qStr3 = "DROP TABLE ZipRoutings";

            var allQueries = new List<string>() { qStr1, qStr2, qStr3 };
            Common.ExecuteQuery(_configuration, allQueries, "phoneRoutings");
            Console.WriteLine("8- Backup created for ZipRouting table under PhoneRoutings.mdb!");
            return RedirectToAction("ImportTable");
        }

        /// <summary>
        /// Import new ZipRouting table from ZipRoutings.mdb to PhoneRoutings.mdb
        /// </summary>
        [HttpGet("import/table")]
        public async Task<ActionResult<String>> ImportTable()
        {
            // NOTE: Change Test1 to ZipRoutings
            string qStr = "SELECT * INTO ZipRoutings " +
                    "FROM [MS Access;DATABASE=" + _configuration["ZipRoutingsDB"] + ";].[ZipRouting]";

            var allQueries = new List<string>() { qStr };
            Common.ExecuteQuery(_configuration, allQueries, "phoneRoutings");
            Console.WriteLine("9- New ZipRouting Table imported into PhoneRoutings.mdb!");
            Console.WriteLine("Process Completed");
            return "Process Completed!";
        }
    }
}
