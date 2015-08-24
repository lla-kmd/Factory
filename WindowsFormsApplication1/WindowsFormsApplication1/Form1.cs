using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Database database = new Database();

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Spoon");
            comboBox1.Items.Add("Fork");
            listView1.Columns.Add("Property Name");
            database.WorkPerformed += new EventHandler<Database.WorkPerformedEventArgs>(worker_WorkPerformed);
            database.GetProductList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.WorkPerformed += new EventHandler<Database.WorkPerformedEventArgs>(worker_WorkPerformed);
            database.AddNewProduct();
        }

        private void worker_WorkPerformed(object sender, Database.WorkPerformedEventArgs e)
        {
            listBox1.Items.Add(e.Name);
        }
    }
}
