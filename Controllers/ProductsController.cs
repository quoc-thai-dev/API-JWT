using API_JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API_JWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from products";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            var myDict = new Dictionary<string, object>();
            myDict["status"] = true;
            myDict["status_message"] = "Success";
            myDict["message"] = "Thành Công!";
            myDict["data_count"] = table.Rows.Count;
            myDict["data"] = table;
            return new JsonResult(myDict);
        }
        [Route("getbyid/{id}")]
        [HttpGet]
        public JsonResult GetById(int id)
        {
            string query = @"select * from products where Id="+id;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            var myDict = new Dictionary<string, object>();
            myDict["status"] = true;
            myDict["status_message"] = "Success";
            myDict["message"] = "Thành Công!";
            myDict["data_count"] = table.Rows.Count;
            myDict["data"] = table;
            return new JsonResult(myDict);
        }
        [Route("add")]
        [HttpPost]
        public JsonResult AddProduct(Products products)
        {
            string query = @"insert into products (Name, Category, Color, UnitPrice, AvailableQuantity) values (@Name, @Category, @Color, @UnitPrice, @AvailableQuantity)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            MySqlDataReader myReader;
            using (MySqlConnection mycon=new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(MySqlCommand myCommand=new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Name", products.Name);
                    myCommand.Parameters.AddWithValue("@Category", products.Category);
                    myCommand.Parameters.AddWithValue("@Color", products.Color);
                    myCommand.Parameters.AddWithValue("@UnitPrice", products.UnitPrice);
                    myCommand.Parameters.AddWithValue("@AvailableQuantity", products.AvailableQuantity);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            var myDict = new Dictionary<string, object>();
            myDict["status"] = true;
            myDict["status_message"] = "Success";
            myDict["message"] = "Thêm sản phẩm thành công!";
            //myDict["data_count"] = table.Rows.Count;
            //myDict["data"] = myReader;
            return new JsonResult(myDict);
        }
        [Route("update")]
        [HttpPost]
        public JsonResult UpdateProduct(Products products)
        {
            string query = @"Update products set Name=@Name, Category=@Category, Color=@Color, UnitPrice=@UnitPrice, AvailableQuantity=@AvailableQuantity where Id=@Id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Id", products.Id);
                    myCommand.Parameters.AddWithValue("@Name", products.Name);
                    myCommand.Parameters.AddWithValue("@Category", products.Category);
                    myCommand.Parameters.AddWithValue("@Color", products.Color);
                    myCommand.Parameters.AddWithValue("@UnitPrice", products.UnitPrice);
                    myCommand.Parameters.AddWithValue("@AvailableQuantity", products.AvailableQuantity);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            var myDict = new Dictionary<string, object>();
            myDict["status"] = true;
            myDict["status_message"] = "Success";
            myDict["message"] = "Cập nhật sản phẩm thành công!";
            //myDict["data_count"] = table.Rows.Count;
            //myDict["data"] = myReader;
            return new JsonResult(myDict);
        }
        [Route("delete")]
        [HttpPost]
        public JsonResult DeleteProduct(Products products)
        {
            string query = @"delete from products where Id=@Id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Id", products.Id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            var myDict = new Dictionary<string, object>();
            myDict["status"] = true;
            myDict["status_message"] = "Success";
            myDict["message"] = "Xóa sản phẩm thành công!";
            //myDict["data_count"] = table.Rows.Count;
            //myDict["data"] = myReader;
            return new JsonResult(myDict);
        }
    }
}
