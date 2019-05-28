﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmTest
{
    public class Demo3_Insertable
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### Insertable Start ####");

            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.PostgreSQL,
                ConnectionString = Config.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        Console.WriteLine(sql);
                        Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                    }
                }
            });

            var insertObj = new Order() { Id = 1, Name = "order1" };
            var updateObjs = new List<Order> {
                 new Order() { Id = 11, Name = "order11" },
                 new Order() { Id = 12, Name = "order12" }
            };

            //Ignore  Price
            db.Insertable(insertObj).IgnoreColumns(it => new { it.Price }).ExecuteReturnIdentity();//get identity
            db.Insertable(insertObj).IgnoreColumns("Name", "TestId").ExecuteReturnIdentity();

            //Only  insert  Name and Price
            db.Insertable(insertObj).InsertColumns(it => new { it.Name, it.Price }).ExecuteReturnIdentity();
            db.Insertable(insertObj).InsertColumns("Name", "SchoolId").ExecuteReturnIdentity();

            //ignore null columns
            db.Insertable(insertObj).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();//get change row count

            //Use Lock
            db.Insertable(insertObj).With(SqlWith.UpdLock).ExecuteCommand();

            Console.WriteLine("#### Insertable End ####");
        }
    }
}