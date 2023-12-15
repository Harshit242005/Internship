import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom'; // Import useNavigate
function Signup() {
  const [formData, setFormData] = useState({
    image: null,
    username: '',
    email: '',
    linkedinUrl: '',
    password: '',
  });
  const navigate = useNavigate(); // Create a navigate function

  const handleInputChange = (e) => {
    const { name, value, files } = e.target;

    if (name === 'image') {
      const file = files[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        setFormData((prevData) => ({
          ...prevData,
          image: reader.result.split(',')[1], // Base64 string
        }));
      };
      reader.readAsDataURL(file);
    } else {
      setFormData((prevData) => ({
        ...prevData,
        [name]: value,
      }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      // Make a POST request to your Express.js server
      const response = await axios.post('http://localhost:5000/CreateUser', formData, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.status == 200)
      {
        navigate('/ChatInterface', { state: { email: formData.email } });
      }
      else{
        alert("User is not created");
      }
      // Handle the server response as needed
    } catch (error) {
      console.error('Error submitting form:', error);
      // Handle errors
    }
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <input type='file' name='image' onChange={handleInputChange} />
        <input type='text' name='username' placeholder='username' onChange={handleInputChange} />
        <input type='email' name='email' placeholder='email' onChange={handleInputChange} />
        <input type='url' name='linkedinUrl' placeholder='linkedin url' onChange={handleInputChange} />
        <input type='password' name='password' placeholder='password' onChange={handleInputChange} />
        <button type='submit'>Submit</button>
      </form>
    </div>
  );
}

export default Signup;
