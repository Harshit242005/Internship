import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import './App.css';
import Landing from './Landing';
import Signup from './Signup';
import Login from './Login';
import ChatInterface from './ChatInterface';

function App() {
  return (
    <div>
      <Router>
        <Routes>
          <Route path="/" element={<Landing />}/>
          <Route path="/Signup" element={<Signup />}/>
          <Route path="/Login" element={<Login />}/>
          <Route path="/ChatInterface" element={<ChatInterface />}/>
        </Routes>
      </Router>
    </div>
  );
}

export default App;
