import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import "./Verify.css";

function Verify() {
  const navigate = useNavigate();
  const location = useLocation();
  const [verificationCode, setVerificationCode] = useState('');

  const handleVerification = async (event) => {
    event.preventDefault();

    try {
      const { email, password, verificationCode: expectedVerificationCode } = location.state || {};

      if (verificationCode == expectedVerificationCode) {
        await axios.post('http://localhost:5000/insertData', {
          email,
          password
        });

        navigate('/login'); // Navigate to the cards component upon successful insertion
      } else {
        
        throw new Error('Verification code does not match');
      }
    } catch (error) {
      console.error('Error verifying and inserting data:', error);
    }
  };

  return (
    <div>
      <h2>Verify Your Account</h2>
      <form className='verify' onSubmit={handleVerification}>
        <div className='verify-data'>
          <label htmlFor='verificationCode'>Verification Code</label>
          <input
            type='text'
            placeholder='Enter your verification code'
            value={verificationCode}
            onChange={(e) => setVerificationCode(e.target.value)}
          />
        </div>
        <button type='submit'>Verify</button>
      </form>
    </div>
  );
}

export default Verify;
