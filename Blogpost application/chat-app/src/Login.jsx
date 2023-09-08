import React, { useState, useContext} from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Signup.css';
import { IdContext } from './IdContext';



function Login() {
  const navigate = useNavigate();
  const { setId } = useContext(IdContext);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async (event) => {
    event.preventDefault(); // Prevent the default form submission behavior

    try {
      // Perform login logic, such as validating the email and password on the server
      const response = await axios.post('http://localhost:5000/login', {
        email,
        password,
      });

      if (response.status === 200) {
        const id = response.data.id;
        setId(id); 
        console.log('Navigating to /Cards');
        // Redirect to the desired route after successful login
        navigate(`/Create`);
      } else {
        // Handle error response
        throw new Error('Login failed');
      }
    } catch (error) {
      // Handle error
      console.error('Error logging in:', error);
    }
  };

  const back = () => {
    navigate('/');
  };

  return (
    <div className='all'>
      <button className='back' onClick={back}>Back</button>
      <div className='input'>
        <p>Login up to get into enhancelog</p>
        <form onSubmit={handleLogin}> {/* Add onSubmit event handler */}
          <div className='input-data'>
            <label htmlFor="email">Email</label>
            <input
              type='email'
              autoCapitalize='on'
              autoComplete='off'
              placeholder='Enter your email'
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>
          <div className='input-data'>
            <label htmlFor="password">Password</label>
            <input
              type='password'
              autoCapitalize='on'
              autoComplete='off'
              placeholder='Enter your password'
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <button className='backbutton' type="submit">Login</button> {/* Update button type to "submit" */}
        </form>
      </div>
    </div>
  );
}

export default Login;
