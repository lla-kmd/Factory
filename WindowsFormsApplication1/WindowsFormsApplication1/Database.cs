using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Database
    {

        //Field
        private SQLiteConnection dbConnection {get; set;}
        private SQLiteCommand command { get; set; }

        private SQLiteDataReader reader { get; set; }

        //Constructor
        public Database()
        {
            try
            {
                dbConnection = DbConnection();
                dbConnection.Open();
            }
            catch (System.ArgumentException)
            {
                SQLiteConnection.CreateFile("Database.sqlite");
            }
            finally
            {
                string sql = @"create table if not exists product_details (product_id int, product_name varchar2(20)); create table if not exists products (product_id int, product_amount int);";
                SqlExecute(sql);
                //string sql1 = "insert into product_details (product_id, product_name) values (1, 'Spoon')";
                //SqlExecute(sql1);
            }
        }


        private SQLiteConnection DbConnection()
        {
            SQLiteConnection dbConnection;
            dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            return dbConnection;
        }

        public void SqlExecute(string sql, bool returnValue = false)
        {
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            if (returnValue == false)
            {
                command.ExecuteNonQuery();
            }
            else
            {
                reader = command.ExecuteReader();
            }         
        }


        public class WorkPerformedEventArgs : EventArgs
        {
            public string Name { get; set; }
            public string Amount { get; set; }

            public WorkPerformedEventArgs(string name, string amount)
            {
                Amount = amount;
                Name = name;
            }
        }

        public event EventHandler<WorkPerformedEventArgs> WorkPerformed;

        public void GetProductList()
        {
            string sql = "select p.product_name, count(*) as Product_Amount from product_details p group by p.product_name";
            SqlExecute(sql, true);
            while (reader.Read())
                OnWorkPerformed(reader["product_name"].ToString(), reader["Product_Amount"].ToString());
        }

        protected virtual void OnWorkPerformed(string name, string amount)
        {
            var del = WorkPerformed as EventHandler<WorkPerformedEventArgs>;
            if (del != null)
            {
                del(this, new WorkPerformedEventArgs(name, amount));
            }
        }

        public void AddNewProduct()
        {
            string sql = "insert into product_details (product_id, product_name) values (3, 'Knife')";
            SqlExecute(sql, false);
            OnWorkPerformed("Knife", "3");
        }




        //string sql1 = "insert into product_details (product_id, product_name) values (2, 'Fork')";
        //SQLiteCommand command1 = new SQLiteCommand(sql1, dbConnection);
        //command1.ExecuteNonQuery();
    }
}
