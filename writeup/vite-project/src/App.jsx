

import './App.css'
import Landing from './Components/Landing'

import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Signup from './Components/Signup';
import Login from './Components/Login';
import ProfileSetup from './Components/ProfileSetup';
import Home from './Components/Home';
import Create from './Components/Create';
import Blog from './Components/Blog';
import Liked from './Components/Liked';
import Profile from './Components/Profile';
import Bookmark from './Components/Bookmark';

function App() {


  return (
    <>
      <Router>
        <Routes>
          <Route path='/' element={<Landing />} />
          <Route path='/Signup' element={<Signup />} />
          <Route path='/Login' element={<Login />} />
          <Route path='/ProfileSetup/:userId'  element={<ProfileSetup />}/>
          <Route path='/Home/:nickname' element={<Home />}/>
          <Route path='/Create/:nickname' element={ <Create />}/>
          <Route path='/Liked/:nickname' element={<Liked />}/>
          <Route path='/Bookmarks/:nickname' element={<Bookmark />}/>
          {/* changing the route and adding the nickname also */}
          <Route path='/Blog/:number/:nickname' element={<Blog />}/>
          <Route path='/Profile/:nickname' element={<Profile />}/>
        </Routes>
      </Router>
    </>
  )
}

export default App
