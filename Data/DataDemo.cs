using Microsoft.Data.SqlClient;
using SharpKnP321.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnP321.Data
{
    internal class DataDemo
    {
        public void Run()
        {
            DataAccessor dataAccessor = new();


            Console.WriteLine("----------ByMoney-----------");
            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByMoney))
            {
                Console.WriteLine(m);
            }
            Console.WriteLine("---------ByChecks------------");

            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByChecks))
            {
                Console.WriteLine(m);
            }
            Console.WriteLine("--------ByQuantity-------------");

            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByQuantity))
            {
                Console.WriteLine(m);
            }
            Console.WriteLine("---------------------");

            foreach (var s in dataAccessor.EnumSales(10))
            {
                Console.WriteLine(s);
            }
            List<Sale> sales = [.. dataAccessor.EnumSales(10)];   

        }

        public void Run5()   

        {
            DataAccessor dataAccessor = new();
            foreach (var dep in dataAccessor.EnumDepartments())
            {
                Console.WriteLine(dep);

                // System.InvalidOperationException:
                // There is already an open DataReader associated with this Connection
                // which must be closed first.
                String sql = $"SELECT * FROM Managers M WHERE M.DepartmentId = '{dep.Id}'";
                using SqlCommand cmd = new(sql, dataAccessor.connection);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(dataAccessor.FromReader<Manager>(reader));
                }
            }
        }

        private long Fact(uint n)
        {
            if (n < 2) return 1;
            uint m = n - 1;
            return n * Fact(m);
        }

        public void Run4()
        {
            DataAccessor dataAccessor = new();
            List<Department> departments = dataAccessor.GetAll<Department>();
            List<Manager> managers = dataAccessor.GetAll<Manager>();



            foreach(var name in departments.Select(d => d.Name))
            {
                Console.WriteLine(name);
            }
            Console.WriteLine(
                String.Join("\n",
                departments
                .GroupJoin(
                    managers,
                    d => d.Id,
                    m => m.DepartmentId,
                    (d, mans) => new
                    {
                        d.Name,
                        Cnt = mans.Count(),
                        Employee = String.Join("; ", mans.Select(m => m.Name))
                    })
                .OrderByDescending(item => item.Cnt)
                .Select(item => String.Format("{0} ({1} empl): {2}", item.Name, item.Cnt, item.Employee))
            ));
        }



        public void Run3()
        {
            DataAccessor dataAccessor = new();
            // dataAccessor.Install();
            // dataAccessor.Seed();

            // dataAccessor.GetProducts().ForEach(Console.WriteLine);
            dataAccessor.GetAll<Product>().ForEach(Console.WriteLine);
            Console.WriteLine("---------------");

            //dataAccessor.GetDepartments().ForEach(Console.WriteLine);
            dataAccessor.GetAll<Department>().ForEach(Console.WriteLine);
            Console.WriteLine("---------------");

            //dataAccessor.GetManagers().ForEach(Console.WriteLine);
            dataAccessor.GetAll<Manager>().ForEach(Console.WriteLine);
            Console.WriteLine("---------------");

            // dataAccessor.GetNews().ForEach(Console.WriteLine);
            dataAccessor.GetAll<News>().ForEach(Console.WriteLine);
            Console.WriteLine("---------------");

        }

        public void Run2()
        {            
            DataAccessor dataAccessor = new();

            dataAccessor.MonthlySalesByManagersSql(year:2025, month: 1);
            Console.WriteLine("---------------");
            dataAccessor.MonthlySalesByManagersOrm(1).ForEach(Console.WriteLine);












            // List<Product> products = dataAccessor.GetProducts();







        }
        public void Run1()
        {
            Console.WriteLine("Data Demo");
            // Робота з БД проводиться у кілька етапів
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\samoylenko_d\Source\Repos\SharpKnP321\Database1.mdf;Integrated Security=True";

            


            SqlConnection connection = new(connectionString);


            try
            {
                connection.Open();   

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
                return;
            }

            using SqlCommand cmd = new(sql, connection);

            Object scalar;
            try
            {
                scalar = cmd.ExecuteScalar();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
                return;
            }



            DateTime timestamp;
            timestamp = Convert.ToDateTime(scalar);
            Console.WriteLine("Res: {0}", timestamp);



            connection.Close();
        }
    }
}
    }
}
