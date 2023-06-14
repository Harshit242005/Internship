import React, { useState } from 'react';
import axios from 'axios';
import "./Profile.css";


import { useParams, useNavigate } from 'react-router-dom';

function Profile() {
  const { id } = useParams();
  const [nickname, setNickname] = useState('');
  const [image, setImage] = useState(null);
  const naviagte = useNavigate();
  const handleSubmit = (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append('id', id);
    formData.append('nickname', nickname);
    formData.append('image', image);

    axios.post('http://localhost:5000/Userdata', formData)
      .then(response => {
        console.log('Data saved successfully');
        naviagte('/Cards');
      })
      .catch(error => {
        console.error('Error saving data:', error);
      });
  };

  const handleNicknameChange = (e) => {
    setNickname(e.target.value);
  };

  const handleImageChange = (e) => {
    setImage(e.target.files[0]);
  };

  return (
    <div>
      <h1>Profile Page</h1>
      
      <form onSubmit={handleSubmit}>
        <div className='profile-name'>
          <label>Nickname:</label>
          <input type="text" value={nickname} onChange={handleNicknameChange} placeholder='Enter your nickname' />
        </div>
        <div className='profile-image'>
          <label>Image:</label>
          <input type="file" onChange={handleImageChange} />
        </div>
        <button className='backbutton' type="submit">Save</button>
      </form>
    </div>
  );
}

export default Profile;
