import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeeCalculate extends Component {
  static displayName = EmployeeCalculate.name;

  constructor(props) {
    super(props);
    this.state = { employeeTypes: {}, id: 0,fullName: '',birthdate: '',tin: '',typeId: 1,basicSalary: 0,absentDays: 0,workedDays: 0,netIncome: 0, loading: true,loadingCalculate:false };
  }

  componentDidMount() {
    this.setState({ loading: true,loadingSave: false });
    this.getEmployeeTypes();
    this.getEmployee(this.props.match.params.id);
    this.setState({ loading: false,loadingSave: false });
  }
  handleChange(event) {
    this.setState({ [event.target.name] : event.target.value});
  }

  handleSubmit(e){
      e.preventDefault();
    this.calculateSalary();
    return true;
  }

  render() {

    let contents = this.state.loading
    ? <p><em>Loading...</em></p>
    : <div>
    <form>
<div className='form-row'>
<div className='form-group col-md-12'>
  <label>Full Name: <b>{this.state.fullName}</b></label>
</div>

</div>

<div className='form-row'>
<div className='form-group col-md-12'>
  <label >Birthdate: <b>{this.state.birthdate}</b></label>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>TIN: <b>{this.state.tin}</b></label>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>Employee Type: <b>{this.state.employeeTypes[this.state.typeId]}</b></label>
</div>
</div>

{ this.getDisplay() }

<div className="form-row">
{ this.getInputTextbox() }
</div>

<div className="form-row">
            {this.state.netIncome === 0 ? <div></div>
              :
              <div className='form-group col-md-12'>
                <label>Net Income: <b>{this.state.netIncome.toLocaleString('en-US', {
                  style: 'decimal',
                  minimumFractionDigits: 2,
                  maximumFractionDigits: 2
                })}</b></label>
              </div>}
</div>

<button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingCalculate} className="btn btn-primary mr-2">{this.state.loadingCalculate?"Loading...": "Calculate"}</button>
<button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
</form>
</div>;


    return (
        <div>
        <h1 id="tabelLabel" >Employee Calculate Salary</h1>
        <br/>
        {contents}
      </div>
    );
  }

  getDisplay() {
    const basicSalary = this.state.basicSalary.toLocaleString('en-US', {
      style: 'decimal',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });

    if (this.state.typeId === 1) {
      return <div className="form-row">
        <div className='form-group col-md-12'><label>Salary: <b>{basicSalary}</b></label></div>
        <div className='form-group col-md-12'><label>Tax: 12% </label></div>
      </div>
    } else if (this.state.typeId === 2) {
      return <div className="form-row">
        <div className='form-group col-md-12'><label>Rate Per Day: <b>{basicSalary}</b></label></div>
      </div>
    }

    return <div></div>;
  }

  getInputTextbox() {
    if (this.state.typeId === 1) {
      return <div className='form-group col-md-6'>
        <label htmlFor='inputAbsentDays4'>Absent Days: </label>
        <input type='number' min='0' className='form-control' id='inputAbsentDays4' onChange={this.handleChange.bind(this)} value={this.state.absentDays} name="absentDays" placeholder='Absent Days' />
      </div>
    } else if (this.state.typeId === 2) {
      return <div className='form-group col-md-6'>
        <label htmlFor='inputWorkDays4'>Worked Days: </label>
        <input type='number' min='0' className='form-control' id='inputWorkDays4' onChange={this.handleChange.bind(this)} value={this.state.workedDays} name="workedDays" placeholder='Worked Days' />
      </div>
    }

    return <div></div>;
  }

  async calculateSalary() {
    this.setState({ loadingCalculate: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'POST',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`,'Content-Type': 'application/json' },
        body: JSON.stringify({absentDays: this.state.absentDays,workedDays: this.state.workedDays})
    };
    const response = await fetch('api/employees/' + this.state.id + '/calculate',requestOptions);
    const data = await response.json();
    if(response.status === 200){
      this.setState({ netIncome: data.salary });
    }
    else {
        alert(data?.message ?? "There was an error occured.");
        this.setState({ netIncome: 0 });
    }
    this.setState({ loadingCalculate: false });
  }

  async getEmployeeTypes() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/employee-types', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ employeeTypes: Object.fromEntries(data.map(x => [x.id, x.typeName])) });
  }

  async getEmployee(id) {
    this.setState({ loading: true,loadingCalculate: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if(response.status === 200){
        const data = await response.json();
        this.setState({ id: data.id,fullName: data.fullName,birthdate: data.birthdate,tin: data.tin,typeId: data.typeId,basicSalary: data.basicSalary });
    }
    else{
        alert("There was an error occured.");
        this.setState({ loading: false,loadingCalculate: false });
    }
  }
}
