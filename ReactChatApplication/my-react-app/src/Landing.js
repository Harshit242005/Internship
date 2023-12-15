import React from 'react'
import {Link} from 'react-router-dom'
function Landing() {
  return (
    <div>
        <Link to="/Signup"><button>signup</button></Link>
        <Link to="/Login"><button>login</button></Link>
    </div>
  )
}

export default Landing