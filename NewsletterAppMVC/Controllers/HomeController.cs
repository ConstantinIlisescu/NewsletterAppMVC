﻿using NewsletterAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string conectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Newsletter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string firstName, string lastName, string emailAddress) //the model biding will bind the info passsed from the form, the parameters have to have the exact name as the name in the forms- not necesarly first letter uppercase
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {

                string queryString = @"INSERT INTO SignUps (FirstName, LastName, EmailAddress) VALUES 
                                                    (@FirstName, @LastName, @EmailAddress)";

                using (SqlConnection connection = new SqlConnection(conectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);

                    command.Parameters["@FirstName"].Value = firstName;
                    command.Parameters["@LastName"].Value = lastName;
                    command.Parameters["@EmailAddress"].Value = emailAddress;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return View("Success");
            }
        }

        public ActionResult Admin()
        {
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress from SignUps";
            List<NewsletterSignUp> signups = new List<NewsletterSignUp>();

            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var signup = new NewsletterSignUp();
                    signup.Id = Convert.ToInt32(reader["Id"]);
                    signup.FirstName = reader["FirstName"].ToString();
                    signup.LastName = reader["LastName"].ToString();
                    signup.EmailAddress = reader["EmailAddress"].ToString();

                    signups.Add(signup);
                }
            }

            return View(signups);
        }

    }
}