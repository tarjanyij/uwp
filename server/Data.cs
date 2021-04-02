﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace server
{
    public class Data
    {
        public static void WriteDataToDatabase(int num)
        {
            using (var db = new AppContext())
            {

                db.Numbers.Add(new Number() { Nums = num });
                db.SaveChanges();

            }
        }

        public static int ReadColoumnSum()
        {
            using (var db = new AppContext())
            {
                return db.Numbers.Sum(x => x.Nums);
            }

        }
        public class Number
        {
            [Key]
            public int Id { get; set; }
            public int Nums { get; set; }
        }

        public class AppContext : DbContext
        {
            private string connectionString;

            public AppContext() : base()
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json", optional: false);

                var configuration = builder.Build();

                connectionString = configuration.GetConnectionString("SQLConnection").ToString();

            }


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                //optionsBuilder.UseSqlServer(@"Server=192.168.8.110;Database=Number;User Id=user;Password=Titok12345;");
                optionsBuilder.UseSqlServer(connectionString, 
                    options => options.EnableRetryOnFailure());
            }

            public DbSet<Number> Numbers { get; set; }
        }
    }

}
