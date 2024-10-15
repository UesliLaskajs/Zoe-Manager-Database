﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace WpfAppZooManager_Database
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;//Initializes a new instance of the SqlConnection class when given a string that contains the connection string.
        public MainWindow()
        {
            InitializeComponent();

            string DatabaseString = ConfigurationManager.ConnectionStrings["WpfAppZooManager_Database.Properties.Settings.UesliDatabaseConnectionString"].ConnectionString;//Established the ConnectionString of DB
            sqlConnection = new SqlConnection(DatabaseString);//Initiated A connection with Database through the String
            Animals();
            ShowTable();
        }


        private void ShowTable()//Method
        {
            try
            {
                string query = "Select * from zoo";//Made a query to Database
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);//Represents a set of data commands and a database connection that are used to fill the DataSet and update a SQL Server database. 

                using (adapter)//Use of the sqlAdapter to retrieve data from the query given
                {
                    DataTable table = new DataTable();//Created  a table to fill the retrieved data from query
                    adapter.Fill(table);//Filled the table

                    ListZoo.DisplayMemberPath = "Location";//Displays by Location data retrived
                    ListZoo.SelectedValuePath = "Id";//Value when selected is Id
                    ListZoo.ItemsSource = table.DefaultView;//Displays the table data



                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void AssociatedAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animals a INNER JOIN ZooAnimals za ON a.Id = za.AnimalsId WHERE za.ZooId = @ZooId";//ZooId is a parameter wich is stored like a variable

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);//Represents a Transact-SQL statement or stored procedure to execute against a SQL Server database.

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListZoo.SelectedValue);//From the sqlCommander we add a value to parameter of the query

                    DataTable AnimalsTable = new DataTable();

                    adapter.Fill(AnimalsTable);

                    ZooList.DisplayMemberPath = "Name";
                    ZooList.SelectedValuePath = "Id";
                    ZooList.ItemsSource = AnimalsTable.DefaultView;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Animals()
        {
            try
            {
                string query = "SELECT * from Animals";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter Adapter = new SqlDataAdapter(sqlCommand);

                using (Adapter)
                {
                    DataTable Animals = new DataTable();

                    Adapter.Fill(Animals);

                    AnimalsList.DisplayMemberPath = "Name";
                    AnimalsList.ItemsSource = Animals.DefaultView;
                    AnimalsList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }   

        private void ListZoo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AssociatedAnimals();
        }

        private void deleteZooHandler(object sender, EventArgs e) {

            try
            {

                string query = "delete  from Zoo  where Id=@ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoo.SelectedValue);
                sqlCommand.ExecuteScalar();
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                ShowTable();
            }

        }


    }
}
