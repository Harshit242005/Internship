
// import React, { useEffect, useContext,useState } from 'react';
// import { Link, useLocation, useNavigate } from 'react-router-dom';
// import './Navbar.css';
// import $ from 'jquery';
// import { IdContext } from './IdContext';
// import axios from 'axios';


// function Navbar() {
//   const location = useLocation();

//   useEffect(() => {
//     $(`.links[href="${location.pathname}"]`).addClass('active');
//     $('.links').on('click', function() {
//       $('.links').removeClass('active');
//       $(this).addClass('active');
//     });
//   }, [location]);

//   const navigate = useNavigate();
//   const { id } = useContext(IdContext);
//   const [imageSrc, setImgaeSrc] = useState('');

//   const handleProfileClick = () => {
//     navigate(`/Profile/${id}`);
//   };

//   useEffect(() => {
//     const fetchImage = async() => {
//       try {
//         const response = await axios.get(`http://localhost:5000/getImage/${id}`, {
//           responseType: 'arraybuffer',

//         });
//         const imageBlob = new Blob([response.data], {type: 'image/jpeg' });
//         const imageUrl = URL.createObjectURL(imageBlob);
//         setImgaeSrc(imageUrl);

//       } catch (error) {
//         console.error('Error fetching user image: ', error);
//       }
//     };

//     fetchImage();
//   }, [id]);

//   return (
//     <div className='Navbar'>
//       <Link className='profileImage' to={`/Profile/${id}`}><img src={imageSrc}  onClick={handleProfileClick} alt='profile picture'/></Link>
//       <Link className='links' to="/Create">Create</Link>
//       <Link className='links' to="/Cards">Cards</Link>
//       <Link className='links' to="/Saved">Saved</Link>
//     </div>
//   );
// }

// export default Navbar;


import React, { useEffect, useContext, useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import './Navbar.css';
import $ from 'jquery';
import axios from 'axios';
import { IdContext } from './IdContext';


function Navbar() {
  const location = useLocation();
  const [imageSrc, setImgaeSrc] = useState('');
  const navigate = useNavigate();
  const { id } = useContext(IdContext);
  
  

  useEffect(() => {
    $(`.links[href="${location.pathname}"]`).addClass('active');
    $('.links').on('click', function() {
      $('.links').removeClass('active');
      $(this).addClass('active');
    });
  }, [location]);

  const handleProfileClick = async () => {
    try {
      const response = await axios.get(`http://localhost:5000/checkId/${id}`);
      const { exists } = response.data;
      if (exists) {
        navigate(`/ChangeProfile/${id}`);
        
      } else {
        navigate(`/Profile/${id}`);
      
      }
    } catch (error) {
      console.error('Error checking ID:', error);
    }
  };
  

  useEffect(() => {
    const fetchImage = async () => {
      try {
        const response = await axios.get(`http://localhost:5000/getImage/${id}`, {
          responseType: 'arraybuffer',
        });
        const imageBlob = new Blob([response.data], { type: 'image/jpeg' });
        const imageUrl = URL.createObjectURL(imageBlob);
        setImgaeSrc(imageUrl);
      } catch (error) {
        console.error('Error fetching user image: ', error);
      }
    };

    fetchImage();
  }, [id]);

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light">
      <Link className="navbar-brand" to="#">
        <img src={imageSrc} onClick={handleProfileClick} alt="profile picture" />
      </Link>
      <button
        className="navbar-toggler"
        type="button"
        data-toggle="collapse"
        data-target="#navbarTogglerDemo01"
        aria-controls="navbarTogglerDemo01"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span className="navbar-toggler-icon"></span>
      </button>

      <div className="collapse navbar-collapse" id="navbarTogglerDemo01">
        <ul className="navbar-nav mr-auto mt-2 mt-lg-0">
          
          <li className="nav-item">
            <Link className="nav-link" to="/Create">Create</Link>
          </li>
          <li className="nav-item">
            <Link className="nav-link" to="/Cards">Cards</Link>
          </li>
          <li className="nav-item">
            <Link className="nav-link" to="/Saved">Saved</Link>
          </li>
          <li className="nav-item">
            <Link className="nav-link" to="/">Out</Link>
          </li>
        </ul>
      </div>
    </nav>
  );
}

export default Navbar;
