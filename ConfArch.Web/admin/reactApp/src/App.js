import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link
} from "react-router-dom";
import Login from "./Pages/login";
import Home from "./Pages/home";
import React, { useState } from 'react';
import Signout from "./Components/Signout";

function App() {
  const [user, setUser] = useState(null);
  
  const storeUser = (data) => {
    setUser(data);
  }

  return (
    
    <Router>
      <div>
        Navigation menu:
        <ul>
          <li>
            <Link to="/admin">Home</Link>
          </li>
          <li>
            <Link to="/admin/login">Login</Link>
          </li>
          <li>
            <Signout storeUser={storeUser} />
          </li>

        </ul>

        <hr />

        <Switch>
          <Route exact path="/admin/">
            <Home user={user} />
          </Route>
          <Route exact path="/admin/login">
            <Login storeUser={storeUser} />
          </Route>

          {/* Problem: Nedari se mi excludovat podstrom z React routeru: "/admin/api/login" - porad se chyta router */}
        </Switch>
      </div>
    </Router>
  );
}

export default App;
