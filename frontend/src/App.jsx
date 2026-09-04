import { useEffect, useState } from 'react';
import './App.css';

function App() {
  const [employees, setEmployees] = useState([]);

  const [showForm, setShowForm] = useState(false);

  const [editingId, setEditingId] = useState(null);

  const [formData, setFormData] = useState({
    name: '',
    email: '',
    salary: '',
    departmentID: ''
  });

  // =========================
  // GET EMPLOYEES
  // =========================
  const fetchEmployees = () => {
    fetch('http://localhost:5294/api/Employees')
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to fetch employees');
        }

        return response.json();
      })
      .then(data => {
        setEmployees(data);
      })
      .catch(error => {
        console.error('Error fetching employees:', error);
      });
  };

  useEffect(() => {
    fetchEmployees();
  }, []);

  // =========================
  // HANDLE FORM INPUT
  // =========================
  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  // =========================
  // ADD / UPDATE EMPLOYEE
  // =========================
  const handleSubmit = (e) => {
    e.preventDefault();

    const employee = {
      name: formData.name,
      email: formData.email,
      salary: Number(formData.salary),
      departmentID: Number(formData.departmentID)
    };

    // =========================
    // UPDATE
    // =========================
    if (editingId !== null) {
      fetch(`http://localhost:5294/api/Employees/${editingId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(employee)
      })
        .then(response => {
          if (!response.ok) {
            throw new Error('Failed to update employee');
          }

          return response.json();
        })
        .then(data => {
          console.log('Employee updated:', data);

          resetForm();
          fetchEmployees();
        })
        .catch(error => {
          console.error('Error updating employee:', error);
        });

      return;
    }

    // =========================
    // ADD
    // =========================
    fetch('http://localhost:5294/api/Employees', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(employee)
    })
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to add employee');
        }

        return response.json();
      })
      .then(data => {
        console.log('Employee added:', data);

        resetForm();
        fetchEmployees();
      })
      .catch(error => {
        console.error('Error adding employee:', error);
      });
  };

  // =========================
  // EDIT BUTTON
  // =========================
  const handleEdit = (employee) => {
    setEditingId(employee.id);

    setFormData({
      name: employee.name,
      email: employee.email,
      salary: employee.salary,
      departmentID: employee.departmentID
    });

    setShowForm(true);
  };

  // =========================
  // DELETE EMPLOYEE
  // =========================
  const handleDelete = (id) => {
    const confirmed = window.confirm(
      'Are you sure you want to delete this employee?'
    );

    if (!confirmed) {
      return;
    }

    fetch(`http://localhost:5294/api/Employees/${id}`, {
      method: 'DELETE'
    })
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to delete employee');
        }

        console.log('Employee deleted');

        fetchEmployees();
      })
      .catch(error => {
        console.error('Error deleting employee:', error);
      });
  };

  // =========================
  // RESET FORM
  // =========================
  const resetForm = () => {
    setFormData({
      name: '',
      email: '',
      salary: '',
      departmentID: ''
    });

    setEditingId(null);
    setShowForm(false);
  };

  return (
    <div className="app">

      <h1>Employee Management System</h1>

      <div className="container">

        <h2>Employees</h2>

        {/* ADD / EDIT BUTTON */}

        {!showForm && (
          <button onClick={() => setShowForm(true)}>
            Add Employee
          </button>
        )}

        {/* EMPLOYEE FORM */}

        {showForm && (
          <form onSubmit={handleSubmit}>

            <h3>
              {editingId !== null
                ? 'Edit Employee'
                : 'Add Employee'}
            </h3>

            <div>
              <label>Name</label>

              <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                required
              />
            </div>

            <div>
              <label>Email</label>

              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                required
              />
            </div>

            <div>
              <label>Salary</label>

              <input
                type="number"
                name="salary"
                value={formData.salary}
                onChange={handleChange}
                required
              />
            </div>

            <div>
              <label>Department ID</label>

              <input
                type="number"
                name="departmentID"
                value={formData.departmentID}
                onChange={handleChange}
                required
              />
            </div>

            <button type="submit">
              {editingId !== null
                ? 'Update Employee'
                : 'Save Employee'}
            </button>

            <button
              type="button"
              onClick={resetForm}
            >
              Cancel
            </button>

          </form>
        )}

        {/* EMPLOYEE TABLE */}

        <table>

          <thead>

            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Email</th>
              <th>Salary</th>
              <th>Department</th>
              <th>Actions</th>
            </tr>

          </thead>

          <tbody>

            {employees.map(employee => (

              <tr key={employee.id}>

                <td>{employee.id}</td>

                <td>{employee.name}</td>

                <td>{employee.email}</td>

                <td>{employee.salary}</td>

                <td>
                  {employee.department?.name}
                </td>

                <td>

                  <button
                    onClick={() => handleEdit(employee)}
                  >
                    Edit
                  </button>

                  <button
                    onClick={() => handleDelete(employee.id)}
                  >
                    Delete
                  </button>

                </td>

              </tr>

            ))}

          </tbody>

        </table>

      </div>

    </div>
  );
}

export default App;