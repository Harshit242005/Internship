import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import "./Signup.css";


function Signup() {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showVerification, setShowVerification] = useState(true);

  const handleSignup = async (event) => {
    event.preventDefault();

    try {
      const response = await axios.post('http://localhost:5000/signup', {
        email,
        password
      });

      if (response.status === 200) {
        setShowVerification(true);
        console.log(response.data.verificationCode);
        navigate('/Verify', {
          state: { email, password, verificationCode: response.data.verificationCode }
        });
      } else {
        throw new Error('Signup failed');
      }
    } catch (error) {
      console.error('Error signing up:', error);
    }
  };

  const back = () => {
    navigate('/');
  };

  return (
    <div className='all'>
      <button className='back' onClick={back}>Back</button>
      <div className='input'>
        {showVerification ? (
          <form onSubmit={handleSignup}>
            <div className='input-data'>
              <label htmlFor='email'>Email</label>
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
              <label htmlFor='password'>Password</label>
              <input
                type='password'
                autoCapitalize='on'
                autoComplete='off'
                placeholder='Enter your password'
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
            <button className='backbutton' type='submit'>Sign Up</button>
          </form>
        ) : (
          <p>Sign up to get into enhancelog</p>
        )}
      </div>
    </div>
  );
}

export default Signup;
