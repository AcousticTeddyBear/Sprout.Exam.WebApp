import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeesIndex extends Component {
  static displayName = EmployeesIndex.name;

  constructor(props) {
    super(props);
    this.state = { employees: [], employeeTypes: {}, loading: true };
  }

  componentDidMount() {
    this.getEmployeeTypes();
    this.populateEmployeeData();
    this.setState({ loading: false });
  }

  static renderEmployeesTable(employees,employeeTypes,parent) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Full Name</th>
            <th>Birthdate</th>
            <th>TIN</th>
            <th>Type</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {employees.map(employee =>
            <tr key={employee.id}>
              <td>{employee.fullName}</td>
              <td>{employee.birthdate}</td>
              <td>{employee.tin}</td>
              <td>{employeeTypes[employee.typeId]}</td>
              <td>
              <button type='button' className='btn btn-info mr-2' onClick={() => parent.props.history.push("/employees/" + employee.id + "/edit")} >Edit</button>
              <button type='button' className='btn btn-primary mr-2' onClick={() => parent.props.history.push("/employees/" + employee.id + "/calculate")}>Calculate</button>
            <button type='button' className='btn btn-danger mr-2' onClick={() => {
              if (window.confirm("Are you sure you want to delete?")) {
                parent.deleteEmployee(employee.id);
              }
            } }>Delete</button></td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : EmployeesIndex.renderEmployeesTable(this.state.employees, this.state.employeeTypes,this);

    return (
      <div>
        <h1 id="tabelLabel" >Employees</h1>
        <p>This page should fetch data from the server.</p>
        <p><button type='button' className='btn btn-success mr-2' onClick={() => this.props.history.push("/employees/create")} >Create</button></p>
        {contents}
      </div>
    );
  }

  async getEmployeeTypes() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/employee-types', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ employeeTypes: Object.fromEntries(data.map(x => [x.id, x.typeName])) });
  }

  async populateEmployeeData() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ employees: data });
  }

  async deleteEmployee(id) {
    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'DELETE',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`,'Content-Type': 'application/json' }
    };
    const response = await fetch('api/employees/' + id,requestOptions);
    if(response.status === 204){
      this.setState({employees: this.state.employees.filter(function(employee) {
        return employee.id !== id
      })});
    }
    else{
      const data = response.json();
      alert(data?.message ?? "There was an error occured.");
    }
  }
}
