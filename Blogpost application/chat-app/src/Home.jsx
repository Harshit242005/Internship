import React from 'react'
import './Home.css';
import { Link } from 'react-router-dom';


function Home() {
  return (
    <>
      <div className='header'>
        <p>Enhancelog</p>
        <Link to="/signup"><button>Sign Up</button></Link>
        <Link to="/login"><button>Login</button></Link>
      </div>
    </>
  )
}

export default Home