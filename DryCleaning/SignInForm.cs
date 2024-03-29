﻿using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DryCleaning
{
    public partial class SignInForm : Form
{
        DatabaseConnection DrySQL = new DatabaseConnection();
 
        public SignInForm()
        { 
            InitializeComponent();
            this.TopMost = true; //Вызов окна поверх других
            this.StartPosition = FormStartPosition.CenterScreen; //Расположение окна по центру монитора
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;//Добавление иконки приложения
            Icon iconForm = new Icon(Application.StartupPath + "\\img\\DryCleaning.ico");
            Icon = iconForm;
            tbPassword.PasswordChar = '*';
            this.Text = "Окно Авторизации";
            lblAutorization.Text = "Авторизация";
            lblAutorization.Left = (ClientSize.Width - lblAutorization.Width) / 2; //Расположение в центре экрана
            lblLogin.Text = "Логин";
            LblPassword.Text = "Пароль";
            btnOK.Text = "OK";
            btnClose.Text = "Закрыть";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CheckUsers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CheckUsers()
        {
            try
            {
                string loginCheck = tbLogin.Text;
                string passwordCheck = tbPassword.Text;
                //string passwordCheck = GetHash(textBox2.Text);
                string checkCmd = $"Select [Login_Sotr], [Password_Sotr] from [dbo].[Sotr] where [Login_Sotr] = '{loginCheck}'  and [Password_Sotr] = '{passwordCheck}'";
                string checkRole = $"Select [Dolj_ID] from [dbo].[Sotr] where [Login_Sotr] ='{loginCheck}'";
                var sqlConnection = DrySQL.DatabaseSQL();
                sqlConnection.Open();
                using (sqlConnection)
                {
                    //sqlConnection.Open();
                    var command = new SqlCommand(checkCmd, sqlConnection);
                    var commandRole = new SqlCommand(checkRole, sqlConnection);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    if (loginCheck == (string)command.ExecuteScalar())
                    {

                        Program.ID_Dolj = (int)commandRole.ExecuteScalar();
                        MessageBox.Show("Авторизация успешна");
                        this.Hide();
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Логин или пароль ведены не верно");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        //private string GetHash(string input) //Хэширование пароля в БД
        //{
        //    var md5 = MD5.Create();
        //    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        //    return Convert.ToBase64String(hash);
        //}

    }
}
