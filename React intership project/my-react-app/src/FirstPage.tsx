import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';
import './FirstPage.css';
const FirstPage = () => {
  const navigate = useNavigate();

  const [name, setName] = useState('');
  const [phone, setPhone] = useState('');
  const [email, setEmail] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (name && phone && email) {
      localStorage.setItem('name', name);
      localStorage.setItem('phone', phone);
      localStorage.setItem('email', email);
      const storedName = localStorage.getItem('name');
      const storedPhone = localStorage.getItem('phone');
      const storedEmail = localStorage.getItem('email');
      console.log(storedName + " " + storedPhone + " " + storedEmail);
    } else {
      //   alert('Please fill in all the required fields.');
      navigate('/second-page');
    }
  };

  return (
    <div className='App'>
      
        <form onSubmit={handleSubmit}>
          <label><p>Name:</p></label>
          <input className='input' type="text" value={name} onChange={(e) => setName(e.target.value)}  autoComplete="off"/>
          <br />
          <label><p>Phone number:</p></label>
          <input className='input' type="text" value={phone} onChange={(e) => setPhone(e.target.value)} autoComplete="off" />
          <br />
          <label><p>Email:</p></label>
          <input className='input' type="text" value={email} onChange={(e) => setEmail(e.target.value)} autoComplete="off" />
          <br />
          <button className='submit' type="submit">Submit</button>
        </form>
      
      <div className='buttons'>
        <Link className='link' to='/api-data'><button className='button'>See data</button></Link>
        <Link className='link' to='/choices'><button className='button'>See choices</button></Link>
      </div>
    </div>
  );
};

export default FirstPage;
