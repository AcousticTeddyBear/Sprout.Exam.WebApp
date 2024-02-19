import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeeEdit extends Component {
  static displayName = EmployeeEdit.name;

  constructor(props) {
    super(props);
    this.state = { employeeTypes:[], id: 0,fullName: '',birthdate: undefined,tin: '',typeId: 1, basicSalary: 0, loading: true,loadingSave:false };
  }

  componentDidMount() {
    this.setState({ loading: true,loadingSave: false });
    this.getEmployeeTypes();
    this.getEmployee(this.props.match.params.id);
    this.setState({ loading: false,loadingSave: false });
  }
  handleChange(event) {
    this.setState({ [event.target.name] : event.target.value });
  }
  handleTypeChange(event) {
    this.setState({ [event.target.name]: +event.target.value });
  }

  handleSubmit(e){
      e.preventDefault();
      if (window.confirm("Are you sure you want to save?")) {
        this.saveEmployee();
      }
  }

  render() {

    let contents = this.state.loading
    ? <p><em>Loading...</em></p>
    : <div>
    <form>
<div className='form-row'>
<div className='form-group col-md-6'>
  <label htmlFor='inputFullName4'>Full Name: *</label>
  <input type='text' className='form-control' id='inputFullName4' onChange={this.handleChange.bind(this)} name="fullName" value={this.state.fullName} placeholder='Full Name' />
</div>
<div className='form-group col-md-6'>
  <label htmlFor='inputBirthdate4'>Birthdate: *</label>
  <input type='date' className='form-control' id='inputBirthdate4' onChange={this.handleChange.bind(this)} name="birthdate" value={this.state.birthdate} placeholder='Birthdate' />
</div>
</div>
<div className="form-row">
<div className='form-group col-md-6'>
  <label htmlFor='inputTin4'>TIN: *</label>
  <input type='text' className='form-control' id='inputTin4' onChange={this.handleChange.bind(this)} value={this.state.tin} name="tin" placeholder='TIN' />
</div>
<div className='form-group col-md-6'>
  <label htmlFor='inputEmployeeType4'>Employee Type: *</label>
  <select id='inputEmployeeType4' onChange={this.handleTypeChange.bind(this)} value={this.state.typeId}  name="typeId" className='form-control'>
     {this.state.employeeTypes.map(et => <option value={et.id}>{et.typeName}</option>)}
  </select>
</div>
</div>
<div className="form-row">
<div className='form-group col-md-6'>
  <label htmlFor='inputSalary4'>Basic Salary: *</label>
  <input type='number' min='1' step='0.01' className='form-control' id='inputSalary4' onChange={this.handleChange.bind(this)} value={this.state.basicSalary} name="basicSalary" placeholder='20000' />
</div>
</div>
<button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingSave} className="btn btn-primary mr-2">{this.state.loadingSave?"Loading...": "Save"}</button>
<button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
</form>
</div>;


    return (
        <div>
        <h1 id="tabelLabel" >Employee Edit</h1>
        <p>All fields are required</p>
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
    this.setState({ employeeTypes: data });
  }

  async saveEmployee() {
    this.setState({ loadingSave: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'PUT',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`,'Content-Type': 'application/json' },
        body: JSON.stringify(this.state)
    };
    const response = await fetch('api/employees/' + this.state.id,requestOptions);

    if(response.status === 200){
        alert("Employee successfully saved");
        this.props.history.push("/employees/index");
    }
    else {
      const data = response.json();
        alert(data?.message ?? "There was an error occured.");
    }
    this.setState({ loadingSave: false });
  }

  async getEmployee(id) {
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ id: data.id,fullName: data.fullName,birthdate: data.birthdate,tin: data.tin,typeId: data.typeId,basicSalary: data.basicSalary });
  }
}
