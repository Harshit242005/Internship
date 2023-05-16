import React from 'react'
import { Link } from 'react-router-dom' 

function SecondPage() {
  return (
    <div>
        <p>Error accessing this page without the details of the form from the last page</p>
        <Link to="/"><button style={{ backgroundColor: 'black', color: 'white', padding: '10px 20px', width: '200px', height: '50px' }}>Go back</button></Link>
    </div>
  )
}

export default SecondPage