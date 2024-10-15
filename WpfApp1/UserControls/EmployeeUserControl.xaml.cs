using System;
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
using WpfApp1.Database;

namespace WpfApp1.UserControls
{
    /// <summary>
    /// Логика взаимодействия для EmployeeUserControl.xaml
    /// </summary>
    public partial class EmployeeUserControl : UserControl
    {
        private Employees _employee;
        public EmployeeUserControl(Employees employee)
        {
            InitializeComponent();
            _employee = employee;
            UpdateEmployeesData();

        }

        private void UpdateEmployeesData()
        {
            EmployeeFullNamText.Text = $"{_employee.LastName} {_employee.FirstName} {_employee.Patronymic}";
            EmployeeYOText.Text = $"{Math.Floor((DateTime.Now - _employee.BirthDate).TotalDays / 365)}";
            EmployeeExpText.Text = $"{_employee.ExperienceYears}";
            string allOperation = WriteAllAvailableOperation();
            CanOperateText.Text = $"{allOperation}";
        }

        private string WriteAllAvailableOperation()
        {
            List<Operations> operations = App.DB.Operations
                .Where(x => App.DB.EmployeeOperations.Where(d => d.EmployeeID == _employee.EmployeeID)
                    .Select(d => d.OperationID).Contains(x.OperationID)).ToList();
            string allOperation = "";
            for (int i = 0; i < operations.Count; i++)
            {
                if (i == 0)
                    allOperation += operations[i].OperationName;
                else
                    allOperation += ", " + operations[i].OperationName;

            }

            return allOperation;
        }
    }
}
